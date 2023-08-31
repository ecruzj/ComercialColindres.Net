using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComercialColindres.DTOs.RequestDTOs.BoletaDeduccionManual;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Servicios.Servicios
{
    public class BoletaDeduccionManualAppServices : IBoletaDeduccionManualAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _uOW;

        public BoletaDeduccionManualAppServices(ComercialColindresContext uow, ICacheAdapter cacheAdapter)
        {
            _uOW = uow;
            _cacheAdapter = cacheAdapter;
        }

        public List<BoletaDeduccionManualDto> GetBoletaDeduccionesManualById(GetBoletaDeduccionesManualByBoletaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.BoletaDeduccionesManual, nameof(GetBoletaDeduccionesManualByBoletaId), request.BoletaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<BoletaDeduccionManual> datos = _uOW.BoletaDeduccionManual.Where(b => b.BoletaId == request.BoletaId)
                                                                            .OrderByDescending(o => o.Monto).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<BoletaDeduccionManual>, IEnumerable<BoletaDeduccionManualDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public BoletaDeduccionManualDto SaveBoletaDeduccionesManual(PostBoletadeduccionesManual request)
        {
            return new BoletaDeduccionManualDto();
        }

        private void RemoveCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.Boletas,
                KeyCache.BoletaDeduccionesManual
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
