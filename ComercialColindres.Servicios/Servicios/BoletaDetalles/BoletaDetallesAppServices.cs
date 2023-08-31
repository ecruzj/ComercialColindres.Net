using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.BoletaDetalles;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.ResponseDTOs;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;

namespace ComercialColindres.Servicios.Servicios
{
    public class BoletaDetallesAppServices : IBoletaDetallesAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        IBoletasDetalleDomainServices _boletasDetalleDomainServices;

        public BoletaDetallesAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IBoletasDetalleDomainServices boletasDetalleDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _boletasDetalleDomainServices = boletasDetalleDomainServices;
        }

        public List<BoletaDetallesDTO> Get(GetBoletasDetallePorBoletaId request)
        {
            var boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (boleta == null) return new List<BoletaDetallesDTO>();

            if (boleta.Estado == Estados.CERRADO)
            {
                return BoletaDetalles(boleta);
            }

            return ArmarBoletaDeducciones(boleta);
        }

        private List<BoletaDetallesDTO> BoletaDetalles(Boletas boleta)
        {
            var boletaDetalles = boleta.BoletaDetalles.ToList();
            var listaDeducciones = new List<BoletaDetallesDTO>();

            foreach (var detalle in boletaDetalles)
            {
                var deduccion = new BoletaDetallesDTO
                {
                    DescripcionDeduccion = detalle.Deduccion.Descripcion,
                    FechaTransaccion = detalle.FechaTransaccion,
                    MontoDeduccion = detalle.MontoDeduccion,
                    ModificadoPor = detalle.ModificadoPor,
                    NoDocumento = detalle.NoDocumento,
                    Observaciones = detalle.Observaciones
                };

                listaDeducciones.Add(deduccion);
            }

            return listaDeducciones;
        }

        private List<BoletaDetallesDTO> ArmarBoletaDeducciones(Boletas boleta)
        {            
            var listaDeducciones = _unidadDeTrabajo.Deducciones.ToList();
            var datos = _boletasDetalleDomainServices.ObtenerBoletaDeducciones(boleta, listaDeducciones);

            var detallesDeducciones = from reg in datos
                                      select new BoletaDetallesDTO
                                      {
                                          DeduccionId = reg.DeduccionId,
                                          DescripcionDeduccion = reg.DescripcionDeduccion,
                                          FechaTransaccion = reg.FechaTransaccion,
                                          MontoDeduccion = reg.MontoDeduccion,
                                          ModificadoPor = reg.ModificadoPor,
                                          Observaciones = reg.Observaciones,
                                          NoDocumento = reg.NoDocumento
                                      };

            return detallesDeducciones.ToList();
        }

        public ActualizarResponseDTO Post(PostBoletasDetalle request)
        {
            var boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (boleta == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "BoletaId NO existe"
                };
            }
            
            var listaDetalleBoleta = boleta.BoletaDetalles.ToList();

            //Verificar si removieron Items
            foreach (var detalle in listaDetalleBoleta)
            {
                var detalleBoleta = request.BoletasDetalle
                                   .FirstOrDefault(c => c.CodigoBoleta == detalle.CodigoBoleta);

                if (detalleBoleta == null)
                {
                    boleta.BoletaDetalles.Remove(detalle);
                }
            }

            IEnumerable<string> mensajesValidacion;

            foreach (var detalle in request.BoletasDetalle)
            {
                var detalleBoleta = listaDetalleBoleta
                                     .FirstOrDefault(c => c.CodigoBoleta == detalle.CodigoBoleta);

                //Hubo una Actualizacion
                if (detalleBoleta != null)
                {
                    detalleBoleta.ActualizarBoletaDetalle(detalle.DeduccionId, detalle.MontoDeduccion, detalle.NoDocumento, detalle.Observaciones);
                    mensajesValidacion = detalleBoleta.GetValidationErrors();

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
                    var nuevoDetalle = new BoletaDetalles(request.BoletaId, detalle.DeduccionId, detalle.MontoDeduccion, detalle.NoDocumento, detalle.Observaciones);
                    mensajesValidacion = nuevoDetalle.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    _unidadDeTrabajo.BoletaDetalles.Add(nuevoDetalle);
                }
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "RegistrarBoletaDetalles");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.BoletasDetalles,
                KeyCache.Boletas
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
