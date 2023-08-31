using System.Collections.Generic;
using System.Linq;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaDetalle;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.Aplicacion;

namespace ComercialColindres.Servicios.Servicios
{
    public class AjusteBoletaDetalleAppServices : IAjusteBoletaDetalleAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        readonly IAjusteBoletaDomainServices _ajusteBoletaDomainServices;

        public AjusteBoletaDetalleAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IAjusteBoletaDomainServices ajusteBoletaDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _ajusteBoletaDomainServices = ajusteBoletaDomainServices;
        }

        public List<AjusteBoletaDetalleDto> GetAjusteBoletaDetallado(GetAjusteBoletaDetalleByVendorId request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.AjusteBoletaDetalles, nameof(GetAjusteBoletaDetalleByVendorId), request.VendorId, request.BoletaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<AjusteBoletaDetalle> datos = _unidadDeTrabajo.AjusteBoletaDetalle.Where(b => b.AjusteBoleta.Estado != Estados.CERRADO && 
                                                                                             b.AjusteBoleta.Boleta.ProveedorId == request.VendorId)
                                                                                      .OrderByDescending(o => o.Monto).ToList();

                List<AjusteBoletaDetalle> datosFiltered = _ajusteBoletaDomainServices.GetAvailableAjustmentDetails(datos, request.BoletaId);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<AjusteBoletaDetalle>, IEnumerable<AjusteBoletaDetalleDto>>(datosFiltered);

                
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<AjusteBoletaDetalleDto> GetAjusteBoletaDetalleByAjusteId(GetAjusteBoletaDetalleByAjusteBoletaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.AjusteBoletaDetalles, nameof(GetAjusteBoletaDetalleByAjusteBoletaId), request.AjusteBoletaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<AjusteBoletaDetalle> datos = _unidadDeTrabajo.AjusteBoletaDetalle.Where(b => b.AjusteBoletaId == request.AjusteBoletaId)
                                                                                      .OrderByDescending(o => o.Monto).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<AjusteBoletaDetalle>, IEnumerable<AjusteBoletaDetalleDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public AjusteBoletaDetalleDto SaveAjusteBoletaDetalle(PostAjusteBoletaDetalle request)
        {
            AjusteBoleta ajusteBoleta = _unidadDeTrabajo.AjusteBoleta.Find(request.AjusteBoletaId);

            if (ajusteBoleta == null) 
            {
                return new AjusteBoletaDetalleDto { ValidationErrorMessage = "AjusteBoletaId no existe!" };
            }

            if (ajusteBoleta.Estado != Estados.NUEVO)
            {
                return new AjusteBoletaDetalleDto { ValidationErrorMessage = "El estado debe ser nuevo!" };
            }

            List<AjusteTipo> ajusteTipos = _unidadDeTrabajo.AjusteTipos.ToList();
            IEnumerable<int?> creditLinesRequest = request.AjusteBoletaDetalles.Select(l => l.LineaCreditoId).Distinct();
            List<LineasCredito> creditLines = _unidadDeTrabajo.LineasCredito.Where(x => creditLinesRequest.Contains(x.LineaCreditoId)).ToList();

            if (!RemoveOldAjusteBoletaDetalle(ajusteBoleta, request.AjusteBoletaDetalles, out string errorMessage))
            {
                return new AjusteBoletaDetalleDto { ValidationErrorMessage = errorMessage };
            }

            List<AjusteBoletaDetalle> ajusteBoletaDetalle = ajusteBoleta.AjusteBoletaDetalles.ToList();

            foreach (AjusteBoletaDetalleDto ajusteDetalleRequest in request.AjusteBoletaDetalles)
            {
                AjusteBoletaDetalle ajusteDetalle = ajusteBoletaDetalle.FirstOrDefault(d => d.AjusteBoletaDetalleId == ajusteDetalleRequest.AjusteBoletaDetalleId);
                AjusteTipo ajusteTipo = ajusteTipos.FirstOrDefault(t => t.AjusteTipoId == ajusteDetalleRequest.AjusteTipoId);
                LineasCredito creditLine = creditLines.FirstOrDefault(x => x.LineaCreditoId == ajusteDetalleRequest.LineaCreditoId);

                if (ajusteDetalle == null)
                {
                    //new item                    
                    if (!_ajusteBoletaDomainServices.TryCreateBoletaAjusteDetalle(ajusteBoleta, ajusteTipo, ajusteDetalleRequest.Monto, ajusteDetalleRequest.Observaciones, creditLine, 
                                                                                  ajusteDetalleRequest.NoDocumento, out errorMessage))
                    {
                        return new AjusteBoletaDetalleDto { ValidationErrorMessage = errorMessage };
                    }

                    //ajusteDetalle = new AjusteBoletaDetalle(ajusteBoleta, ajusteTipo, ajusteDetalleRequest.Monto, ajusteDetalleRequest.Observaciones);
                    //ajusteBoleta.AddAjusteBoletaDetalle(ajusteDetalle);
                }
                else
                {
                    //update item
                    ajusteDetalle.UpdateAjusteBoletaDetalle(ajusteTipo, ajusteDetalleRequest.Monto, ajusteDetalleRequest.Observaciones);

                    IEnumerable<string> validations = ajusteDetalle.GetValidationErrors();

                    if (validations.Any())
                    {
                        return new AjusteBoletaDetalleDto { ValidationErrorMessage = Utilitarios.CrearMensajeValidacion(validations) };
                    }
                }                
            }

            RemoverCache();
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "AddAjusteBoletaDetalle");
            _unidadDeTrabajo.Commit(transaccion);

            return new AjusteBoletaDetalleDto();
        }

        private bool RemoveOldAjusteBoletaDetalle(AjusteBoleta ajusteBoleta, List<AjusteBoletaDetalleDto> ajusteBoletaDetalles, out string errorMessage)
        {
            List<AjusteBoletaDetalle> ajusteDetalles = ajusteBoleta.AjusteBoletaDetalles.ToList();

            foreach (AjusteBoletaDetalle ajusteDetalle in ajusteDetalles)
            {
                AjusteBoletaDetalleDto ajusteDetalleRequest = ajusteBoletaDetalles.FirstOrDefault(a => a.AjusteBoletaDetalleId == ajusteDetalle.AjusteBoletaDetalleId);

                if (ajusteDetalleRequest != null) continue;

                if (!_ajusteBoletaDomainServices.TryDeleteAjusteDetalle(ajusteDetalle, out errorMessage))
                {
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
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
