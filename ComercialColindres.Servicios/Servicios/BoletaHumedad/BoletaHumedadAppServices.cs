using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.EntornoDatos;
using System.Collections.Generic;
using System.Linq;
using System;
using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.ReglasNegocio.DomainServices;
using ServidorCore.Aplicacion;

namespace ComercialColindres.Servicios.Servicios
{
    public class BoletaHumedadAppServices : IBoletaHumedadAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        IBoletaHumedadDomainServices _boletaHumedadDomainServices;
        IBoletaHumedadAsignacionDomainServices _boletaHumedadAsignacionDomainServices;

        public BoletaHumedadAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IBoletaHumedadDomainServices boletaHumedadDomainServices,
                                        IBoletaHumedadAsignacionDomainServices boletaHumedadAsignacionDomainServices)
        {
            _cacheAdapter = cacheAdapter;
            _unidadDeTrabajo = unidadDeTrabajo;
            _boletaHumedadDomainServices = boletaHumedadDomainServices;
            _boletaHumedadAsignacionDomainServices = boletaHumedadAsignacionDomainServices;
        }
        
        public BusquedaBoletasHumedadDto GetBoletasHumedadPaged(GetByValorBoletasHumedad request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.BoletasHumedad, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = BoletasHumedadSpecification.BoletasHumedadFilter(request.Filtro);
                List<BoletaHumedad> datos = _unidadDeTrabajo.BoletasHumedad.Where(especificacion.EvalFunc).OrderByDescending(o => o.FechaTransaccion).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<BoletaHumedad>, IEnumerable<BoletaHumedadDto>>(datosPaginados.Items as IEnumerable<BoletaHumedad>);

                var dto = new BusquedaBoletasHumedadDto
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<BoletaHumedadDto>(datosDTO)
                };

                return dto;
            });

            return datosConsulta;
        }

        public List<BoletaHumedadDto> CreateBoletasHumedad(PutBoletasHumedad request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.BoletasHumedad == null) throw new ArgumentNullException(nameof(request.BoletasHumedad));
            if (request.RequestUserInfo == null) throw new ArgumentNullException(nameof(request.RequestUserInfo));

            List<BoletaHumedadDto> boletasHumedadDto = new List<BoletaHumedadDto>();
            string toleranceConfiguration = SettingFactory.CreateSetting().GetSettingByIdAndAttribute("ToleranciaHumedad", "Tolerancia", string.Empty);

            if (string.IsNullOrWhiteSpace(toleranceConfiguration))
            {
                boletasHumedadDto.Add(new BoletaHumedadDto { MensajeError = "No está configurado el Porcentaje de Tolerancias de Humedad" });
                return boletasHumedadDto;
            }

            decimal tolerancePercentage = Convert.ToDecimal(toleranceConfiguration);
            ClientePlantas facility = _unidadDeTrabajo.ClientePlantas.Find(request.FacilityDestination);

            if (facility == null)
            {
                boletasHumedadDto.Add(new BoletaHumedadDto { MensajeError = "La planta no existe!" });
                return boletasHumedadDto;
            }

            List<BoletaHumedad> boletasHumedad = _unidadDeTrabajo.BoletasHumedad.Where(h => h.PlantaId == request.FacilityDestination).ToList();
            List<string> boletasHumedadRequest = request.BoletasHumedad.Select(h => h.NumeroEnvio).Distinct().ToList();
            List<Boletas> boletas = _unidadDeTrabajo.Boletas.Where(b => b.PlantaId == request.FacilityDestination && boletasHumedadRequest.Contains(b.NumeroEnvio)).ToList();
            string errorMessage = string.Empty;

            foreach (BoletaHumedadDto humedadInfo in request.BoletasHumedad)
            {
                if (!_boletaHumedadDomainServices.CanCreateBoletaHumedad(boletasHumedad, humedadInfo.NumeroEnvio, facility, out errorMessage))
                {
                    boletasHumedadDto.Add(CreateBoletaHumedadException(humedadInfo.NumeroEnvio, errorMessage));
                    continue;
                }

                Boletas boleta = boletas.FirstOrDefault(b => b.NumeroEnvio == humedadInfo.NumeroEnvio && b.PlantaId == facility.PlantaId);
                BoletaHumedad newBoletaHumidity = new BoletaHumedad(humedadInfo.NumeroEnvio, facility, boleta != null, humedadInfo.HumedadPromedio, tolerancePercentage, humedadInfo.FechaHumedad);

                IEnumerable<string> validations = newBoletaHumidity.GetValidationErrors();

                if (validations.Any())
                {
                    boletasHumedadDto.Add(CreateBoletaHumedadException(humedadInfo.NumeroEnvio, Utilitarios.CrearMensajeValidacion(validations)));
                    continue;
                }

                _boletaHumedadAsignacionDomainServices.TryAssignBoletaHumidityToBoleta(boleta, newBoletaHumidity);
                _unidadDeTrabajo.BoletasHumedad.Add(newBoletaHumidity);
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CreateBoletaHumidity");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheBoletasHumidity();

            return boletasHumedadDto;
        }

        public BoletaHumedadDto DeleteBoletaHumedad(DeleteBoletasHumedad request)
        {
            var boletaHumedad = _unidadDeTrabajo.BoletasHumedad.Find(request.BoletaHumedadId);

            if (boletaHumedad == null)
            {
                return new BoletaHumedadDto { MensajeError = "No existe la Boleta con Humedad" };
            }

            string errorMessage = string.Empty;
            if (!_boletaHumedadDomainServices.TryToRemoveBoletaHumedad(boletaHumedad, out errorMessage))
            {
                return new BoletaHumedadDto { MensajeError = errorMessage };
            }

            _unidadDeTrabajo.BoletasHumedad.Remove(boletaHumedad);
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "DeleteBoletaHumidity");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheBoletasHumidity();

            return new BoletaHumedadDto();
        }

        private BoletaHumedadDto CreateBoletaHumedadException(string numeroEnvio, string message)
        {
            return new BoletaHumedadDto
            {
                NumeroEnvio = numeroEnvio,
                MensajeError = message
            };
        }

        private void RemoverCacheBoletasHumidity()
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
