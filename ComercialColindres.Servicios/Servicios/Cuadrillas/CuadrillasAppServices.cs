using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.DTOs.RequestDTOs.Cuadrillas;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.Aplicacion;
using System.Collections.Generic;
using System.Linq;
using System;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Servicios.Servicios
{
    public class CuadrillasAppServices : ICuadrillasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public CuadrillasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public BusquedaCuadrillasDTO Get(GetAllCuadrillas request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.Cuadrillas, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {

                var especificacion = CuadrillasEspecificaciones.FiltrarCuadrilla(request.Filtro);
                List<Cuadrillas> datos = _unidadDeTrabajo.Cuadrillas.Where(especificacion.EvalFunc).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Cuadrillas>, IEnumerable<CuadrillasDTO>>(datosPaginados.Items as IEnumerable<Cuadrillas>);

                var dto = new BusquedaCuadrillasDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<CuadrillasDTO>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public List<CuadrillasDTO> Get(GetCuadrillasPorPlantaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Cuadrillas, "GetCuadrillasPorPlantaId", request.PlantaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<Cuadrillas> datos = _unidadDeTrabajo.Cuadrillas.Where(c => c.PlantaId == request.PlantaId).OrderByDescending(o => o.NombreEncargado).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Cuadrillas>, IEnumerable<CuadrillasDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<CuadrillasDTO> Get(GetCuadrillasPorValorBusqueda request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.Cuadrillas, "GetCuadrillasPorValorBusqueda", request.ValorBusqueda, request.PlantaId);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = CuadrillasEspecificaciones.FiltrarPorPlanta(request.ValorBusqueda, request.PlantaId);
                var datos = _unidadDeTrabajo.Cuadrillas.Where(especificacion.EvalFunc).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Cuadrillas>, IEnumerable<CuadrillasDTO>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }

        public ActualizarResponseDTO Post(PostCuadrillas request)
        {
            var planta = _unidadDeTrabajo.ClientePlantas.Find(request.PlantaId);

            if (planta == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "La PlantaId no existe"
                };
            }

            var listaCuadrillas = planta.Cuadrillas.ToList();

            //Verificar si removieron Items
            foreach (var itemCuadrilla in listaCuadrillas)
            {
                var cuadrilla = request.Cuadrillas
                                   .FirstOrDefault(c => c.CuadrillaId == itemCuadrilla.CuadrillaId);

                if (cuadrilla == null)
                {
                    planta.Cuadrillas.Remove(itemCuadrilla);
                }
            }

            IEnumerable<string> mensajesValidacion;

            foreach (var itemCuadrilla in request.Cuadrillas)
            {
                var cuadrilla = listaCuadrillas
                                     .FirstOrDefault(c => c.CuadrillaId == itemCuadrilla.CuadrillaId);

                //Hubo una Actualizacion
                if (cuadrilla != null)
                {
                    cuadrilla.NombreEncargado = cuadrilla.NombreEncargado;
                    mensajesValidacion = cuadrilla.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }
                }
                else
                {
                    //Se agrego una nueva cuenta
                    var nuevoConductor = new Cuadrillas(itemCuadrilla.NombreEncargado, request.PlantaId, itemCuadrilla.AplicaPago);
                    mensajesValidacion = nuevoConductor.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    _unidadDeTrabajo.Cuadrillas.Add(nuevoConductor);
                }
            }

            _unidadDeTrabajo.SaveChanges();

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.Cuadrillas
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
