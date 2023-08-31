using System.Collections.Generic;
using System.Linq;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaPagos;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;

namespace ComercialColindres.Servicios.Servicios
{
    public class AjusteBoletaPagoAppServices : IAjusteBoletaPagoAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        readonly IAjusteBoletaDomainServices _ajusteBoletaDomainServices;

        public AjusteBoletaPagoAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IAjusteBoletaDomainServices ajusteBoletaDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _ajusteBoletaDomainServices = ajusteBoletaDomainServices;
        }

        public List<AjusteBoletaPagoDto> GetAjusteBoletaPagoByDetailId(GetAjusteBoletaPagoByDetailId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.AjusteBoletaPagos, nameof(GetAjusteBoletaPagoByDetailId), request.AjusteBoletaDetalleId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<AjusteBoletaPago> datos = _unidadDeTrabajo.AjusteBoletaPago.Where(b => b.AjusteBoletaDetalle.AjusteBoletaDetalleId == request.AjusteBoletaDetalleId)
                                                                            .OrderByDescending(o => o.Monto).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<AjusteBoletaPago>, IEnumerable<AjusteBoletaPagoDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<AjusteBoletaPagoDto> GetAjusteBoletaPagoByBoletaId(GetAjusteBoletaPagoByBoletaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.AjusteBoletaPagos, nameof(GetAjusteBoletaPagoByBoletaId), request.BoletaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<AjusteBoletaPago> datos = _unidadDeTrabajo.AjusteBoletaPago.Where(b => b.BoletaId == request.BoletaId)
                                                                            .OrderByDescending(o => o.Monto).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<AjusteBoletaPago>, IEnumerable<AjusteBoletaPagoDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public AjusteBoletaPagoDto SaveAjusteBoletaPagos(PostAjusteBoletaPagoByBoleta request)
        {
            Boletas boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);
            string transactionType = string.Empty;

            if (boleta == null)
            {
                return new AjusteBoletaPagoDto { ValidationErrorMessage = "BoletaId no Existe!" };
            }

            if (boleta.Estado != Estados.ACTIVO)
            {
                return new AjusteBoletaPagoDto { ValidationErrorMessage = "El estado de la Boleta debe ser ACTIVO" };
            }

            if (!RemoveOldAjusteBoletaPayment(boleta, request.AjusteBoletaPagos, out string errorMessage))
            {
                return new AjusteBoletaPagoDto { ValidationErrorMessage = errorMessage };
            }

            List<AjusteBoletaPago> currentAjustePayments = boleta.AjusteBoletaPagos.ToList();
            IEnumerable<int> ajusteBoletaDetallesId = request.AjusteBoletaPagos.Select(a => a.AjusteBoletaDetalleId).Distinct();
            List<AjusteBoletaDetalle> ajusteBoletaDetalles = _unidadDeTrabajo.AjusteBoletaDetalle.Where(x => ajusteBoletaDetallesId.Contains(x.AjusteBoletaDetalleId)).ToList();

            foreach (int ajusteDetalle in ajusteBoletaDetallesId)
            {
                var ajusteDetail = ajusteBoletaDetalles.FirstOrDefault(a => a.AjusteBoletaDetalleId == ajusteDetalle);

                if (ajusteDetail == null)
                {
                    return new AjusteBoletaPagoDto { ValidationErrorMessage = "Existe un AjusteDetalleId Inválido" };
                }
            }
            
            foreach (AjusteBoletaPagoDto paymentRequest in request.AjusteBoletaPagos)
            {
                AjusteBoletaPago ajustePayment = currentAjustePayments.FirstOrDefault(p => p.AjusteBoletaPagoId == paymentRequest.AjusteBoletaPagoId);

                if (ajustePayment == null)
                {
                    //New Item
                    AjusteBoletaDetalle ajusteBoletaDetalle = GetAjusteBoletaDetalleById(ajusteBoletaDetalles, paymentRequest.AjusteBoletaDetalleId);

                    ajustePayment = new AjusteBoletaPago(ajusteBoletaDetalle, boleta, paymentRequest.Monto);

                    if (!_ajusteBoletaDomainServices.TryApplyAjusteBoletaPayment(ajustePayment, boleta, out errorMessage))
                    {
                        return new AjusteBoletaPagoDto { ValidationErrorMessage = errorMessage };
                    }

                    transactionType = "AddAjustePayment";
                }
                else
                {
                    //Update
                    if (!_ajusteBoletaDomainServices.TryUpdateAjusteBoletaPago(ajustePayment, boleta, paymentRequest.Monto, out errorMessage))
                    {
                        return new AjusteBoletaPagoDto { ValidationErrorMessage = errorMessage };
                    }

                    transactionType = "UpdateAjustePayment";
                }
            }

            if (string.IsNullOrWhiteSpace(transactionType))
            {
                transactionType = "CleanAjustePayment";
            }

            RemoverCache();
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, transactionType);
            _unidadDeTrabajo.Commit(transaccion);

            return new AjusteBoletaPagoDto();
        }

        private bool RemoveOldAjusteBoletaPayment(Boletas boleta, List<AjusteBoletaPagoDto> ajustePaymentsRequest, out string errorMessage)
        {
            List<AjusteBoletaPago> currentPayments = boleta.AjusteBoletaPagos.ToList();

            foreach (AjusteBoletaPago payment in currentPayments)
            {
                AjusteBoletaPagoDto paymentDto = ajustePaymentsRequest.FirstOrDefault(p => p.AjusteBoletaPagoId == payment.AjusteBoletaPagoId);

                if (paymentDto != null) continue;

                if (!_ajusteBoletaDomainServices.TryRemoveAjusteBoletaPayment(boleta, payment, out errorMessage))
                {
                    return false;
                };
            }

            errorMessage = string.Empty;
            return true;
        }

        private List<AjusteBoletaPago> GetAjusteBoletaPayments(AjusteBoleta ajusteBoleta)
        {
            List<AjusteBoletaDetalle> ajusteDetalles = ajusteBoleta.AjusteBoletaDetalles.ToList();
            List<AjusteBoletaPago> currentPayments = new List<AjusteBoletaPago>();

            foreach (AjusteBoletaDetalle ajusteDetalle in ajusteDetalles)
            {
                foreach (AjusteBoletaPago currentpayment in ajusteDetalle.AjusteBoletaPagos)
                {
                    currentPayments.Add(currentpayment);
                }
            }

            return currentPayments;
        }

        private Boletas GetBoletaById(List<Boletas> boletas, int boletaId)
        {
            return boletas.Where(b => b.BoletaId == boletaId).FirstOrDefault();
        }

        private AjusteBoletaDetalle GetAjusteBoletaDetalleById(List<AjusteBoletaDetalle> ajusteBoletaDetalles, int ajusteBoletaDetalleId)
        {
            return ajusteBoletaDetalles.Where(a => a.AjusteBoletaDetalleId == ajusteBoletaDetalleId).FirstOrDefault();
        }

        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.Boletas,
                KeyCache.AjusteBoletas,
                KeyCache.AjusteBoletaDetalles,
                KeyCache.AjusteBoletaPagos
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
