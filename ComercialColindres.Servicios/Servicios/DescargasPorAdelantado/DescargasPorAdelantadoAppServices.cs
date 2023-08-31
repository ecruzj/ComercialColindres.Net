using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.DescargasPorAdelantado;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using System;
using ServidorCore.Aplicacion;
using ComercialColindres.ReglasNegocio.DomainServices;

namespace ComercialColindres.Servicios.Servicios
{
    public class DescargasPorAdelantadoAppServices : IDescargasPorAdelantadoAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        readonly ComercialColindresContext _unidadDeTrabajo;
        readonly IBoletasAppServices _boletasAppServices;
        readonly IDescargadoresDomainServices _descargadoresDomainServices;

        public DescargasPorAdelantadoAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IBoletasAppServices boletasAppServices,
                                                 IDescargadoresDomainServices descargadoresDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _boletasAppServices = boletasAppServices;
            _descargadoresDomainServices = descargadoresDomainServices;
        }

        public List<DescargasPorAdelantadoDTO> Get(GetDescargasAdelantadasPorPagoDescargadaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.DescargasPorAdelantado, "GetDescargasAdelantadasPorPagoDescargadaId", request.PagoDescargaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<DescargasPorAdelantado> datos = _unidadDeTrabajo.DescargasPorAdelantado.Where(d => d.PagoDescargaId == request.PagoDescargaId).OrderByDescending(o => o.PrecioDescarga).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<DescargasPorAdelantado>, IEnumerable<DescargasPorAdelantadoDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<DescargasPorAdelantadoDTO> Get(GetDescargasAdelantadasPendientes request)
        {
            var cacheKey = string.Format("{0}{1}", KeyCache.DescargasPorAdelantado, "GetDescargasAdelantadasPendientes");
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<DescargasPorAdelantado> datos = _unidadDeTrabajo.DescargasPorAdelantado.Where(d => d.Estado == Estados.PENDIENTE).OrderBy(o => o.FechaCreacion).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<DescargasPorAdelantado>, IEnumerable<DescargasPorAdelantadoDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }
        
        public DescargasPorAdelantadoDTO Post(PostDescargasPorAdelantado request)
        {
            if (request.RequestUserInfo == null) throw new ArgumentNullException("RequestUserInfo");
            if (request.DescargasPorAdelantado == null) throw new ArgumentNullException("DescargasPorAdelantado");
            
            var pagoDescarga = _unidadDeTrabajo.PagoDescargadores.Find(request.PagoDescargaId);
            
            if (pagoDescarga == null)
            {
                return new DescargasPorAdelantadoDTO { ValidationErrorMessage = "PagoDescargaId NO Existe!" };
            }

            if (pagoDescarga.Estado != Estados.ACTIVO)
            {
                return new DescargasPorAdelantadoDTO { ValidationErrorMessage = "El estado del Pago de Descargas debe ser ACTIVO!" };
            }

            ClientePlantas planta = pagoDescarga.Cuadrilla.ClientePlanta;
            bool mustEvaluateShippingNumber = planta.IsShippingNumberRequired();

            //Remover elementos eliminados
            string mensajeValidacion = string.Empty;
            if (!TryRemoverDescargas(pagoDescarga, request.DescargasPorAdelantado, out mensajeValidacion))
            {
                return new DescargasPorAdelantadoDTO
                {
                    ValidationErrorMessage = mensajeValidacion
                };
            }

            foreach (var itemDescarga in request.DescargasPorAdelantado)
            {
                var descarga = mustEvaluateShippingNumber
                               ? pagoDescarga.DescargasPorAdelantado.FirstOrDefault(d => d.NumeroEnvio == itemDescarga.NumeroEnvio)
                               : pagoDescarga.DescargasPorAdelantado.FirstOrDefault(d => d.CodigoBoleta == itemDescarga.CodigoBoleta);

                //Nuevo elemento
                if (descarga == null)
                {
                    if (ExisteBoletaIngresada(itemDescarga, mustEvaluateShippingNumber, out mensajeValidacion))
                    {
                        return new DescargasPorAdelantadoDTO { ValidationErrorMessage = mensajeValidacion };
                    }

                    var descargaPorAdelantado = new DescargasPorAdelantado(itemDescarga.NumeroEnvio, itemDescarga.CodigoBoleta, pagoDescarga, itemDescarga.PrecioDescarga, itemDescarga.FechaCreacion);

                    var validaciones = descargaPorAdelantado.GetValidationErrors();

                    if (validaciones.Any())
                    {
                        return new DescargasPorAdelantadoDTO { ValidationErrorMessage = Utilitarios.CrearMensajeValidacion(validaciones) };
                    }

                    pagoDescarga.AddDescargaPorAdelanto(descargaPorAdelantado);
                }
                else
                {
                    //Modifico un elemento
                    descarga.ActualizarPrecioDescargaPorAdelantado(itemDescarga.PrecioDescarga);

                    var validaciones = descarga.GetValidationErrors();

                    if (validaciones.Any())
                    {
                        return new DescargasPorAdelantadoDTO { ValidationErrorMessage = Utilitarios.CrearMensajeValidacion(validaciones) };
                    }

                    if (_boletasAppServices.ExisteBoleta(itemDescarga.CodigoBoleta, itemDescarga.NumeroEnvio, itemDescarga.PlantaId))
                    {
                        var boleta = mustEvaluateShippingNumber
                                     ? _unidadDeTrabajo.Boletas.First(b => b.NumeroEnvio == itemDescarga.NumeroEnvio && b.PlantaId == itemDescarga.PlantaId)
                                     : _unidadDeTrabajo.Boletas.First(b => b.CodigoBoleta == itemDescarga.CodigoBoleta && b.PlantaId == itemDescarga.PlantaId);

                        if (!_descargadoresDomainServices.TryActualizarPrecioDescargaProducto(boleta, itemDescarga.PrecioDescarga, out mensajeValidacion))
                        {
                            return new DescargasPorAdelantadoDTO { ValidationErrorMessage = mensajeValidacion };
                        }                       
                    }                    
                }
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "DescargasPorAdelantado");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new DescargasPorAdelantadoDTO();
        }

        private bool ExisteBoletaIngresada(DescargasPorAdelantadoDTO descargaPorAdelanto, bool mustEvaluateShippingNumber, out string mensajeValidacion)
        {
            if (descargaPorAdelanto == null) throw new ArgumentNullException(nameof(descargaPorAdelanto));

            string codigoBoleta = mustEvaluateShippingNumber ? descargaPorAdelanto.NumeroEnvio : descargaPorAdelanto.CodigoBoleta;

            if (_boletasAppServices.ExisteBoleta(descargaPorAdelanto.CodigoBoleta, descargaPorAdelanto.NumeroEnvio, descargaPorAdelanto.PlantaId))
            {
                mensajeValidacion = $"Ya existe ingresada la Boleta {codigoBoleta}";
                return true;
            }

            if (ExisteDescargaPorAdelantado(descargaPorAdelanto, mustEvaluateShippingNumber))
            {
                mensajeValidacion = $"Ya existe la Boleta {codigoBoleta} como Descarga por Adelantado";
                return true;
            }
            
            mensajeValidacion = string.Empty;
            return false;
        }

        private bool ExisteDescargaPorAdelantado(DescargasPorAdelantadoDTO descargaPorAdelanto, bool mustEvaluateShippingNumber)
        {
            var descargaAdelantada = mustEvaluateShippingNumber
                                     ? _unidadDeTrabajo.DescargasPorAdelantado.FirstOrDefault(b => b.NumeroEnvio == descargaPorAdelanto.NumeroEnvio && b.PlantaId == descargaPorAdelanto.PlantaId)
                                     : _unidadDeTrabajo.DescargasPorAdelantado.FirstOrDefault(b => b.CodigoBoleta == descargaPorAdelanto.CodigoBoleta && b.PlantaId == descargaPorAdelanto.PlantaId);

            if (descargaAdelantada == null) return false;

            if (descargaPorAdelanto.EsActualizacion && descargaAdelantada != null)
            {
                return descargaAdelantada.DescargaPorAdelantadoId != descargaPorAdelanto.DescargaPorAdelantadoId;
            }

            return descargaAdelantada.DescargaPorAdelantadoId != descargaPorAdelanto.DescargaPorAdelantadoId;
        }

        private bool TryRemoverDescargas(PagoDescargadores pagoDescarga, List<DescargasPorAdelantadoDTO> descargasPorAdelantadoRequest, out string mensajeValidacion)
        {
            List<DescargasPorAdelantado> descargasporAdelantado = pagoDescarga.DescargasPorAdelantado.ToList();

            foreach (DescargasPorAdelantado itemDescarga in descargasporAdelantado)
            {
                DescargasPorAdelantadoDTO descargaPorAdelantado = descargasPorAdelantadoRequest.FirstOrDefault(d => d.DescargaPorAdelantadoId == itemDescarga.DescargaPorAdelantadoId);

                if (descargaPorAdelantado != null) continue;

                if (!_descargadoresDomainServices.TryRemoverDescargas(pagoDescarga, itemDescarga, out mensajeValidacion))
                {
                    return false;
                }
            }

            mensajeValidacion = string.Empty;
            return true;
        }
        
        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.DescargasPorAdelantado,
                KeyCache.Descargadores,
                KeyCache.PagoDescargas,
                KeyCache.PagoDescargasDetalle,
                KeyCache.Boletas,
                KeyCache.BoletasDetalles
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
