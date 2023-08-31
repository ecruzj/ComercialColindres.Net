using System;
using System.Collections.Generic;
using ComercialColindres.DTOs.RequestDTOs.Gasolineras;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using System.Linq;
using ComercialColindres.Datos.Especificaciones;
using ServidorCore.Aplicacion;

namespace ComercialColindres.Servicios.Servicios
{
    public class GasolinerasAppServices : IGasolinerasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public GasolinerasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<GasolinerasDTO> Get(GetGasolinerasPorValorBusqueda request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.Gasolineras, "GetGasolinerasPorValorBusqueda", request.ValorBusqueda, request.ConGasCredito);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = GasolinerasEspecificaciones.FiltrarBusqueda(request.ValorBusqueda, request.ConGasCredito);
                var datos = _unidadDeTrabajo.Gasolineras.Where(especificacion.EvalFunc).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Gasolineras>, IEnumerable<GasolinerasDTO>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }

        public GasolinerasDTO Get(GetGasolinera request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Gasolineras, "GetGasolinera", request.GasolineraId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var dato = _unidadDeTrabajo.Gasolineras.FirstOrDefault(g => g.GasolineraId == request.GasolineraId);
                var datoDTO =
                    AutomapperTypeAdapter.ProyectarComo<Gasolineras, GasolinerasDTO>(dato);
                return datoDTO;
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostGasolinera request)
        {
            var gasolinera = _unidadDeTrabajo.Gasolineras.Find(request.Gasolinera.GasolineraId);

            if (gasolinera != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "GasolineraId YA existe"
                };
            }

            gasolinera = new Gasolineras(request.Gasolinera.Descripcion, request.Gasolinera.NombreContacto, request.Gasolinera.TelefonoContacto);

            var mensajesValidacion = gasolinera.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            _unidadDeTrabajo.Gasolineras.Add(gasolinera);
            _unidadDeTrabajo.SaveChanges();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutGasolinera request)
        {
            var gasolinera = _unidadDeTrabajo.Gasolineras.Find(request.Gasolinera.GasolineraId);

            if (gasolinera == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "GasolineraId NO Existe"
                };
            }

            gasolinera.ActualizarGasolinera(request.Gasolinera.Descripcion, request.Gasolinera.NombreContacto, request.Gasolinera.TelefonoContacto);

            var mensajesValidacion = gasolinera.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }
            
            _unidadDeTrabajo.SaveChanges();

            return new ActualizarResponseDTO();
        }
    }
}
