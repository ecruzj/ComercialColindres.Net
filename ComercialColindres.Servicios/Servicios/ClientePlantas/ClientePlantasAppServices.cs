using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;
using System;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Especificaciones;

namespace ComercialColindres.Servicios.Servicios
{
    public class ClientePlantasAppServices : IClientePlantasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public ClientePlantasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<ClientePlantasDTO> Get(GetAllPlantas request)
        {
            var datos = _unidadDeTrabajo.ClientePlantas.ToList();
            var dto = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<ClientePlantas>, IEnumerable<ClientePlantasDTO>>(datos);
            return dto.ToList();
        }

        public List<ClientePlantasDTO> Get(GetPlantasPorValorBusqueda request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Conductores, "GetPlantasPorValorBusqueda", request.ValorBusqueda);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = ClientePlantasEspecificaciones.FiltrarPorValor(request.ValorBusqueda);
                var datos = _unidadDeTrabajo.ClientePlantas.Where(especificacion.EvalFunc).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<ClientePlantas>, IEnumerable<ClientePlantasDTO>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }

        public ClientePlantasDTO Get(GetPlanta request)
        {
            var dato = _unidadDeTrabajo.ClientePlantas.Find(request.PlantaId);
            if (dato == null)
            {
                return new ClientePlantasDTO
                {
                    ValidationErrorMessage = "Planta Id NO existe"
                };
            }

            var dto = AutomapperTypeAdapter.ProyectarComo<ClientePlantas, ClientePlantasDTO>(dato);
            dto.IsShippingNumberRequired = dato.IsShippingNumberRequired();

            return dto;
        }

        public ActualizarResponseDTO Put(PutPlanta request)
        {
            var dato = _unidadDeTrabajo.ClientePlantas.Find(request.Planta.PlantaId);
            if (dato == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Planta Id NO existe"
                };
            }
            
            dato.ActualizarPlanta(request.Planta.RTN, request.Planta.NombrePlanta, request.Planta.Telefonos, request.Planta.Direccion);

            var mensajesValidacion = dato.GetValidationErrors();
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
