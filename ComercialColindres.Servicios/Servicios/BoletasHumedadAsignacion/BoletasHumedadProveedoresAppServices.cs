using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.BoletasHumedadProveedores;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Servicios.Servicios
{
    public class BoletasHumedadProveedoresAppServices : IBoletasHumedadProveedoresAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public BoletasHumedadProveedoresAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _cacheAdapter = cacheAdapter;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public List<BoletaHumedadProveedorDto> GetBoletasHumidityByVendor(GetBoletasHumidityByVendorId request)
        {
            var cacheKey = $"{KeyCache.BoletasHumedadProveedor}-{nameof(GetBoletasHumidityByVendorId)}-{request.VendorId}-{request.DestinationFacility}";
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<BoletaHumedadProveedor> datos = _unidadDeTrabajo.BoletasHumedadProveedor.Where(p => p.ProveedorId == request.VendorId && 
                                                                                                    p.BoletaHumedad.Estado == Estados.ACTIVO &&
                                                                                                    p.BoletaHumedad.PlantaId == request.DestinationFacility)
                                                                            .OrderByDescending(o => o.FechaTransaccion).ToList();

                List<string> outStandingBoletasHumidity = datos.Select(n => n.BoletaHumedad.NumeroEnvio).Distinct().ToList();
                List<Boletas> boletas = _unidadDeTrabajo.Boletas.Where(b => outStandingBoletasHumidity.Contains(b.NumeroEnvio) && b.PlantaId == request.DestinationFacility).ToList();

                foreach (BoletaHumedadProveedor outStanding in datos)
                {
                    Boletas boleta = boletas.FirstOrDefault(b => b.NumeroEnvio == outStanding.BoletaHumedad.NumeroEnvio);

                    if (boleta == null) continue;

                    outStanding.OutStandingPay = outStanding.BoletaHumedad.CalculateHumidityPricePayment();
                }

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<BoletaHumedadProveedor>, IEnumerable<BoletaHumedadProveedorDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }
    }
}
