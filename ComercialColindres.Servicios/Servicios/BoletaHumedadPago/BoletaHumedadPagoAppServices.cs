using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using System;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadPago;
using ComercialColindres.DTOs.RequestDTOs.BoletaPagoHumedad;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.ReglasNegocio.DomainServices;
using System.Collections.Generic;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Servicios.Servicios
{
    public class BoletaHumedadPagoAppServices : IBoletaHumedadPagoAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        IBoletaHumedadPagoDomainServices _boletaHumedadPagoDomainServices;

        public BoletaHumedadPagoAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IBoletaHumedadPagoDomainServices boletaHumedadPagoDomainServices)
        {
            _cacheAdapter = cacheAdapter;
            _unidadDeTrabajo = unidadDeTrabajo;
            _boletaHumedadPagoDomainServices = boletaHumedadPagoDomainServices;
        }

        public List<BoletaHumedadPagoDto> GetHumidityPaymentByBoleta(GetHumidityPaymentByBoleta request)
        {
            var cacheKey = $"{KeyCache.BoletasHumedadPago}-{nameof(GetHumidityPaymentByBoleta)}-{request.BoletaId}";
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<BoletaHumedadPago> datos = _unidadDeTrabajo.BoletasHumedadPago.Where(p => p.BoletaId == request.BoletaId &&
                                                                                          p.BoletaHumedad.Estado == Estados.ACTIVO &&
                                                                                          p.BoletaHumedad.BoletaIngresada &&
                                                                                          p.Boleta != null)
                                                                                   .OrderByDescending(o => o.FechaTransaccion).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<BoletaHumedadPago>, IEnumerable<BoletaHumedadPagoDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public BoletaHumedadPagoDto GetHumidityPayment(GetHumidityPayment request)
        {
            var cacheKey = $"{KeyCache.BoletasHumedadPago}-{nameof(GetHumidityPayment)}-{request.BoletaHumedadId}";
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                BoletaHumedadPago datos = _unidadDeTrabajo.BoletasHumedadPago.FirstOrDefault(p => p.BoletaHumedadId == request.BoletaHumedadId);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarComo<BoletaHumedadPago, BoletaHumedadPagoDto>(datos);
                return datosDTO;
            });

            return datosConsulta;
        }

        public BoletaHumedadPagoDto CreateBoletaHumidityPayment(PostBoletaHumedadPago request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.BoletasHumedadPago == null) throw new ArgumentNullException(nameof(request.BoletasHumedadPago));
            if (request.RequestUserInfo == null) throw new ArgumentNullException(nameof(request.RequestUserInfo));
            
            List<int> boletasHumidityId = request.BoletasHumedadPago.Select(h => h.BoletaHumedadId).ToList();
            Boletas boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);
            List<BoletaHumedad> boletasHumedad = _unidadDeTrabajo.BoletasHumedad.Where(h => boletasHumidityId.Contains(h.BoletaHumedadId)).ToList();
            List<int> humidityPaymentsIdRequest = request.BoletasHumedadPago.Select(h => h.BoletaHumedadPagoId).ToList();
            string errorMessage = string.Empty;
            
            _boletaHumedadPagoDomainServices.RemoveOldHimidityPayments(boleta, humidityPaymentsIdRequest);

            foreach (BoletaHumedadPagoDto newHumidityPayment in request.BoletasHumedadPago)
            {
                bool exists = boletasHumedad.Any(h => h.BoletaHumedadPago != null && 
                                                 h.BoletaHumedadPago.BoletaHumedadId == newHumidityPayment.BoletaHumedadId);

                if (exists) continue;
                
                BoletaHumedad boletaHumedad = boletasHumedad.FirstOrDefault(h => h.BoletaHumedadId == newHumidityPayment.BoletaHumedadId);

                if (!_boletaHumedadPagoDomainServices.TryToAssignHumidityToBoletaForPayment(boletaHumedad, boleta, out errorMessage))
                {
                    return new BoletaHumedadPagoDto
                    {
                        MensajeError = errorMessage
                    };
                }
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CreateBoletaHumidityPayment");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheHumidityPayments();

            return new BoletaHumedadPagoDto();
        }
        
        public BoletaHumedadPagoDto DeleteBoletaHumidityPay(DeleteBoletaHumedadPago request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.RequestUserInfo == null) throw new ArgumentNullException(nameof(request.RequestUserInfo));

            BoletaHumedadPago boletaHumidityPayment = _unidadDeTrabajo.BoletasHumedadPago.Find(request.BoletaHumedadPagoId);
            string errorMessage = string.Empty;

            if (!_boletaHumedadPagoDomainServices.CanRemoveBoletaHumidityPayment(boletaHumidityPayment, out errorMessage))
            {
                return new BoletaHumedadPagoDto
                {
                    MensajeError = errorMessage
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "RemoveBoletaHumidityPayment");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheHumidityPayments();

            return new BoletaHumedadPagoDto();
        }

        private void RemoverCacheHumidityPayments()
        {
            var listaKey = new List<string>
            {
                KeyCache.Boletas,
                KeyCache.BoletasHumedad,
                KeyCache.BoletasHumedadPago,
                KeyCache.BoletasHumedadAsignacion
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
