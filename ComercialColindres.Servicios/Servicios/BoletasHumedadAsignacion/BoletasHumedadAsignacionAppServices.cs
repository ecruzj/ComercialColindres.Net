using System.Collections.Generic;
using System.Linq;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadAsignacion;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;

namespace ComercialColindres.Servicios.Servicios
{
    public class BoletasHumedadAsignacionAppServices : IBoletasHumedadAsignacionAppServices
    {
        ComercialColindresContext _unidadDeTrabajo;
        private readonly ICacheAdapter _cacheAdapter;
        IBoletaHumedadAsignacionDomainServices _boletaHumedadAsignacionDomainServices;


        public BoletasHumedadAsignacionAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IBoletaHumedadAsignacionDomainServices boletaHumedadAsignacionDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _boletaHumedadAsignacionDomainServices = boletaHumedadAsignacionDomainServices;
        }

        public List<BoletaHumedadAsignacionDto> GetBoletasHumidityByVendor(GetBoletasHumedadByVendor request)
        {
            var cacheKey = $"{KeyCache.BoletasHumedadAsignacion}-{nameof(GetBoletasHumedadByVendor)}-{request.VendorId}-{request.BoletaId}";
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<BoletaHumedadAsignacion> boletasHumidityAssignment = _unidadDeTrabajo.BoletaHumedadAsignacion.Where(a => a.BoletaHumedad.Estado == Estados.ACTIVO && 
                                                                                                     a.Boleta.ProveedorId == request.VendorId &&
                                                                                                     a.BoletaHumedad.BoletaIngresada)
                                                                                   .OrderByDescending(o => o.FechaTransaccion).ToList();

                _boletaHumedadAsignacionDomainServices.RemoveBoletasHumidityWithPaymentFromOthers(boletasHumidityAssignment, request.BoletaId);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<BoletaHumedadAsignacion>, IEnumerable<BoletaHumedadAsignacionDto>>(boletasHumidityAssignment);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        private void RemoverCacheBoletas()
        {
            var listaKey = new List<string>
            {
                KeyCache.Boletas,
                KeyCache.GasCreditos,
                KeyCache.Prestamos,
                KeyCache.PagoPrestamos,
                KeyCache.BoletasHumedadPago,
                KeyCache.BoletasHumedadAsignacion,
                KeyCache.BoletasHumedad
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
