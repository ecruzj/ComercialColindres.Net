using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComercialColindres.DTOs.RequestDTOs.BoletaOtrasDeducciones;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;

namespace ComercialColindres.Servicios.Servicios
{
    public class BoletaOtrasDeduccionesAppServices : IBoletaOtrasDeduccionesAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        ILineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;

        public BoletaOtrasDeduccionesAppServices(ICacheAdapter cacheAdapter, ComercialColindresContext unidadDeTrabajo,
                                                 ILineasCreditoDeduccionesDomainServices lineasCreditoDeduccionesDomainServices)
        {
            _cacheAdapter = cacheAdapter;
            _unidadDeTrabajo = unidadDeTrabajo;
            _lineasCreditoDeduccionesDomainServices = lineasCreditoDeduccionesDomainServices;
        }

        public List<BoletaOtrasDeduccionesDTO> Get(GetBoletaOtrasDeduccionesPorBoletaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.BoletaOtrasDeducciones, "GetBoletasCierrePorBoletaId", request.BoletaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<BoletaOtrasDeducciones> datos = _unidadDeTrabajo.BoletaOtrasDeducciones.Where(b => b.BoletaId == request.BoletaId)
                                                                            .OrderByDescending(o => o.Monto).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<BoletaOtrasDeducciones>, IEnumerable<BoletaOtrasDeduccionesDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostBoletaOtrasDeducciones request)
        {
            var boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (boleta == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "BoletaId NO Existe"
                };
            }

            var boletaOtrasDecucciones = boleta.BoletaOtrasDeducciones.ToList();

            var removerBoletaOtrasDeduccion = RemoverBoletaOtrasDeduccion(boleta, request.BoletaOtrasDeducciones);

            if (!string.IsNullOrWhiteSpace(removerBoletaOtrasDeduccion))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = removerBoletaOtrasDeduccion
                };
            }

            IEnumerable<string> mensajesValidacion;

            foreach (var deduccion in request.BoletaOtrasDeducciones)
            {
                var boletaDeduccion = boletaOtrasDecucciones.FirstOrDefault(d => d.BoletaOtraDeduccionId == deduccion.BoletaOtraDeduccionId
                                                                            && d.BoletaId == deduccion.BoletaId);
                
                if (boletaDeduccion == null)
                {
                    if (deduccion.Monto < 0 && !deduccion.EsDeduccionManual)
                    {
                        var lineaCredito = _unidadDeTrabajo.LineasCredito.FirstOrDefault(lc => lc.LineaCreditoId == deduccion.LineaCreditoId
                                                                                         && lc.SucursalId == request.SucursalId);

                        if (lineaCredito == null)
                        {
                            return new ActualizarResponseDTO
                            {
                                MensajeError = "No existe la LineaCreditoId"
                            };
                        }

                        var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, deduccion.Monto);

                        if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
                        {
                            return new ActualizarResponseDTO
                            {
                                MensajeError = aplicarDeduccion
                            };
                        }
                    }

                    BoletaOtrasDeducciones newOtherDeduction = new BoletaOtrasDeducciones(deduccion.BoletaId, deduccion.Monto, deduccion.MotivoDeduccion, deduccion.FormaDePago,
                                                                    deduccion.LineaCreditoId, deduccion.NoDocumento, deduccion.EsDeduccionManual);
                    string errorMessage = string.Empty;
                    if (!newOtherDeduction.ValidateNewOtherDeduction(boleta, out errorMessage))
                    {
                        return new ActualizarResponseDTO { MensajeError = errorMessage };
                    }

                    mensajesValidacion = newOtherDeduction.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    boleta.AgregarOtraDeduccion(newOtherDeduction);                 
                }
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "CrearBoletaOtrasDeducciones");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheBoletaOtrasDeducciones();

            return new ActualizarResponseDTO();
        }

        private string RemoverBoletaOtrasDeduccion(Boletas boleta, List<BoletaOtrasDeduccionesDTO> boletaOtrasDeduccionesRequest)
        {
            var otrasDeducciones = boleta.BoletaOtrasDeducciones.ToList();

            foreach (var deduccion in otrasDeducciones)
            {
                var boletaDeduccion = boletaOtrasDeduccionesRequest
                                   .FirstOrDefault(d => d.BoletaOtraDeduccionId == deduccion.BoletaOtraDeduccionId);

                if (boletaDeduccion == null)
                {
                    if (deduccion.LineasCredito == null)
                    {
                        boleta.BoletaOtrasDeducciones.Remove(deduccion);
                        continue;
                    }

                    if (_lineasCreditoDeduccionesDomainServices.AplicaRemoverDeduccionCredito(deduccion.LineasCredito))
                    {
                        boleta.BoletaOtrasDeducciones.Remove(deduccion);
                    }
                    else
                    {
                        return string.Format("Ya no puede Eliminar Deducciones de la Linea de Crédito {0} porque está {1}",
                                                        deduccion.LineasCredito.CodigoLineaCredito, deduccion.LineasCredito.Estado);
                    }
                }
            }

            return string.Empty;
        }

        private void RemoverCacheBoletaOtrasDeducciones()
        {
            var listaKey = new List<string>
            {
                KeyCache.Boletas,
                KeyCache.BoletaOtrasDeducciones,
                KeyCache.LineasCredito,
                KeyCache.LineasCreditoDeducciones
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
