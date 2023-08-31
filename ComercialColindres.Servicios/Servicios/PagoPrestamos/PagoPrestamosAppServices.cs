using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.PagoPrestamos;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;
using ComercialColindres.ReglasNegocio.DomainServices;
using System;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Servicios.Servicios
{
    public class PagoPrestamosAppServices : IPagoPrestamosAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        IBoletasDetalleDomainServices _boletasDetalleDomainSerives;
        IPagoPrestamosDomainServices _pagoPrestamosDomainServices;

        public PagoPrestamosAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter,
                                        IBoletasDetalleDomainServices boletasDetalleDomainSerives,
                                        IPagoPrestamosDomainServices pagoPrestamosDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _boletasDetalleDomainSerives = boletasDetalleDomainSerives;
            _pagoPrestamosDomainServices = pagoPrestamosDomainServices;
        }

        public List<PagoPrestamosDTO> Get(GetPagoPrestamosPorBoletaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.PagoPrestamos, "GetPagoPrestamosPorBoletaId", request.BoletaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<PagoPrestamos> datos = _unidadDeTrabajo.PagoPrestamos.Where(b => b.BoletaId == request.BoletaId)
                                                                            .OrderByDescending(o => o.FechaTransaccion).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<PagoPrestamos>, IEnumerable<PagoPrestamosDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<PagoPrestamosDTO> Get(GetPagoPrestamos request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}{4}", KeyCache.PagoPrestamos, "GetPagoPrestamosPorPrestamoId", request.PrestamoId, request.FiltrarAbonosPorBoletas, request.MostrarTodosLosAbonos);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<PagoPrestamos> datos = new List<PagoPrestamos>();

                if (request.FiltrarAbonosPorBoletas && !request.MostrarTodosLosAbonos)
                {
                    datos = _unidadDeTrabajo.PagoPrestamos.Where(p => p.PrestamoId == request.PrestamoId && p.Boleta != null)
                                                          .OrderByDescending(o => o.FechaTransaccion).ToList();
                }

                if (!request.FiltrarAbonosPorBoletas && !request.MostrarTodosLosAbonos)
                {
                    datos = _unidadDeTrabajo.PagoPrestamos.Where(p => p.PrestamoId == request.PrestamoId && p.Boleta == null)
                                                          .OrderByDescending(o => o.FechaTransaccion).ToList();
                }

                if (request.MostrarTodosLosAbonos)
                {
                    datos = _unidadDeTrabajo.PagoPrestamos.Where(p => p.PrestamoId == request.PrestamoId)
                                                          .OrderByDescending(o => o.FechaTransaccion).ToList();
                }

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<PagoPrestamos>, IEnumerable<PagoPrestamosDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }
        
        public PagoPrestamosDTO Post(PostPagosPrestamoPorBoletaId request)
        {
            var boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (boleta == null)
            {
                return new PagoPrestamosDTO
                {
                    ValidationErrorMessage = "BoletaId NO existe"
                };
            }

            if (boleta.Estado != Estados.ACTIVO)
            {
                return new PagoPrestamosDTO { ValidationErrorMessage = "El estado de la Boleta debe ser ACTIVO" };
            }

            var pagoPrestamos = boleta.PagoPrestamos.ToList();

            if (!request.PagoPrestamos.Any())
            {
                foreach (var item in pagoPrestamos)
                {
                    _unidadDeTrabajo.PagoPrestamos.Remove(item);
                }

                pagoPrestamos = new List<PagoPrestamos>();
            }

            //Verificar si removieron Items
            foreach (var pago in pagoPrestamos)
            {
                var abonoPrestamo = request.PagoPrestamos.FirstOrDefault(c => c.PagoPrestamoId == pago.PagoPrestamoId);

                if (abonoPrestamo == null)
                {
                    _unidadDeTrabajo.PagoPrestamos.Remove(pago);
                }
            }

            var listaPrestamos = request.PagoPrestamos.Select(p => p.PrestamoId).Distinct();

            foreach (var prestamoIdItem in listaPrestamos)
            {
                var prestamo = _unidadDeTrabajo.Prestamos.Find(prestamoIdItem);

                if (prestamo == null)
                {
                    return new PagoPrestamosDTO
                    {
                        ValidationErrorMessage = "PrestamoId NO Existe"
                    };
                }

                var listaPagoPrestamos = boleta.PagoPrestamos.Where(p => p.PrestamoId == prestamoIdItem);
                var listaPagosPrestamoRequest = request.PagoPrestamos.Where(p => p.PrestamoId == prestamoIdItem);

                IEnumerable<string> mensajesValidacion;

                foreach (var pago in listaPagosPrestamoRequest)
                {
                    var abonoPrestamo = listaPagoPrestamos.FirstOrDefault(c => c.PagoPrestamoId == pago.PagoPrestamoId);

                    //Hubo una Actualizacion
                    if (abonoPrestamo != null)
                    {
                        if (!TryActulizarAbonoPorBoleta(abonoPrestamo, pago, out mensajesValidacion))
                        {
                            return new PagoPrestamosDTO { ValidationErrorMessage = Utilitarios.CrearMensajeValidacion(mensajesValidacion) };
                        }
                    }
                    else
                    {
                        if (!TryCrearAbonoPorBoleta(boleta, pago, out mensajesValidacion))
                        {
                            return new PagoPrestamosDTO { ValidationErrorMessage = Utilitarios.CrearMensajeValidacion(mensajesValidacion) };
                        }
                    }
                }

                var mensajeValidacion = string.Empty;
                if (!_pagoPrestamosDomainServices.TryValidateAbonosPrestamoPorBoleta(prestamo, boleta, out mensajeValidacion))
                {
                    return new PagoPrestamosDTO { ValidationErrorMessage = mensajeValidacion };
                }
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "AbonoPrestamoPorBoleta");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new PagoPrestamosDTO();
        }

        public PagoPrestamosDTO Post(PostOtrosAbonosPrestamo request)
        {
            if (request.RequestUserInfo == null) throw new ArgumentNullException("RequestUserInfo");
            if (request.PagoPrestamos == null) throw new ArgumentNullException("PagoPrestamos");

            var prestamo = _unidadDeTrabajo.Prestamos.Find(request.PrestamoId);

            if (prestamo == null) return new PagoPrestamosDTO { ValidationErrorMessage = "PrestamoId NO Existe!" }; 

            if (prestamo.Estado != Estados.ACTIVO) return new PagoPrestamosDTO { ValidationErrorMessage = "El estado del préstamo debe ser ACTIVO" };
            
            var abonoPrestamos = prestamo.PagoPrestamos.Where(a => a.Boleta == null).ToList();
            var listaPagosPrestamoRequest = request.PagoPrestamos.ToList();

            if (!listaPagosPrestamoRequest.Any())
            {
                foreach (var item in abonoPrestamos)
                {
                    prestamo.RemoverOtroAbono(item);
                }

                abonoPrestamos = new List<PagoPrestamos>();
            }

            //Verificar si removieron Items
            foreach (var pago in abonoPrestamos)
            {
                var abonoPrestamo = request.PagoPrestamos.FirstOrDefault(c => c.PagoPrestamoId == pago.PagoPrestamoId);

                if (abonoPrestamo == null)
                {
                    prestamo.RemoverOtroAbono(pago);
                }
            }

            IEnumerable<string> mensajesValidacion;

            foreach (var pago in listaPagosPrestamoRequest)
            {
                var abonoPrestamo = abonoPrestamos.FirstOrDefault(c => c.PagoPrestamoId == pago.PagoPrestamoId);

                //Hubo una Actualizacion
                if (abonoPrestamo != null)
                {
                    abonoPrestamo.ActualizarAbonoPrestamo(pago.FormaDePago, pago.BancoId, pago.NoDocumento, null, pago.MontoAbono, pago.Observaciones);
                    mensajesValidacion = abonoPrestamo.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new PagoPrestamosDTO { ValidationErrorMessage = Utilitarios.CrearMensajeValidacion(mensajesValidacion) };
                    }
                }
                else
                {
                    //Se agrego un nuevo Abono
                    var nuevoAbonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, pago.FormaDePago, pago.BancoId, null, pago.MontoAbono, pago.NoDocumento, pago.Observaciones);
                    mensajesValidacion = nuevoAbonoPrestamo.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new PagoPrestamosDTO { ValidationErrorMessage = Utilitarios.CrearMensajeValidacion(mensajesValidacion) };
                    }

                    prestamo.AgregarOtroAbono(nuevoAbonoPrestamo);
                }
            }

            var mensajeValidacion = string.Empty;
            if (!_pagoPrestamosDomainServices.TryValidateAbonosPrestamo(prestamo, out mensajeValidacion))
            {
                return new PagoPrestamosDTO { ValidationErrorMessage = mensajeValidacion };
            }

            prestamo.CerrarPrestamo();

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "OtrosAbonosPrestamo");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new PagoPrestamosDTO();
        }

        public PagoPrestamosDTO Put(PutAbonosPrestamoPorBoletas request)
        {
            if (request.RequestUserInfo == null) throw new ArgumentNullException("RequestUserInfo");
            if (request.PagoPrestamos == null) throw new ArgumentNullException("PagoPrestamos");

            var prestamo = _unidadDeTrabajo.Prestamos.Find(request.PrestamoId);

            if (prestamo == null) return new PagoPrestamosDTO { ValidationErrorMessage = "PrestamoId NO Existe!" };

            if (prestamo.Estado != Estados.ACTIVO) return new PagoPrestamosDTO { ValidationErrorMessage = "El estado del Prestamo debe ser ACTIVO" };

            var abonosPorBoleta = prestamo.PagoPrestamos.Where(b => b.BoletaId != null).Select( l => l.BoletaId).Distinct().ToList();
                        
            var mensajeValidacion = string.Empty;
            
            foreach (var abonoPorBoletaId in abonosPorBoleta)
            {
                var abonosPorBoletaRequest = request.PagoPrestamos.Where(b => b.BoletaId == (int)abonoPorBoletaId).ToList();

                if (!ActualizarAbonoPorBoleta((int)abonoPorBoletaId, prestamo, abonosPorBoletaRequest, out mensajeValidacion))
                {
                    return new PagoPrestamosDTO { ValidationErrorMessage = mensajeValidacion };
                }
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "UpdateAbonoPorBoleta");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new PagoPrestamosDTO();
        }

        private bool ActualizarAbonoPorBoleta(int boletaId, Prestamos prestamo, List<PagoPrestamosDTO> requestPagoPrestamos, out string mensajeValidacion)
        {
            if (requestPagoPrestamos == null) throw new ArgumentNullException("requestPagoPrestamos");

            var boleta = _unidadDeTrabajo.Boletas.Find(boletaId);

            if (boleta == null)
            {
                mensajeValidacion = "BoletaId NO existe";
                return false;          
            }

            if (boleta.Estado != Estados.ACTIVO)
            {
                mensajeValidacion = string.Format("El estado de la Boleta {0} debe ser ACTIVO!", boleta.CodigoBoleta);
                return false;
            }
            
            //Verificar si removieron Items
            EliminarAbonosPorBoleta(boleta, prestamo, requestPagoPrestamos);

            var listaPagoPrestamos = boleta.PagoPrestamos.Where(p => p.PrestamoId == prestamo.PrestamoId);
            var listaPagosPrestamoRequest = requestPagoPrestamos.Where(p => p.PrestamoId == prestamo.PrestamoId);

            IEnumerable<string> mensajesValidacion;

            foreach (var abono in listaPagosPrestamoRequest)
            {
                var abonoPrestamo = listaPagoPrestamos.FirstOrDefault(c => c.PagoPrestamoId == abono.PagoPrestamoId);

                //Hubo una Actualizacion
                if (abonoPrestamo == null) continue;
                
                if (!TryActulizarAbonoPorBoleta(abonoPrestamo, abono, out mensajesValidacion))
                {
                    mensajeValidacion = Utilitarios.CrearMensajeValidacion(mensajesValidacion);
                    return false;
                }
            }

            return _pagoPrestamosDomainServices.TryValidateAbonosPrestamoPorBoleta(prestamo, boleta, out mensajeValidacion);
        }

        private void EliminarAbonosPorBoleta(Boletas boleta, Prestamos prestamo, List<PagoPrestamosDTO> pagoPrestamos)
        {
            var abonosPorBoleta = boleta.PagoPrestamos.Where(p => p.PrestamoId == prestamo.PrestamoId).ToList();

            foreach (var abonoPorBoleta in abonosPorBoleta)
            {
                var abonoPrestamo = pagoPrestamos.FirstOrDefault(c => c.PagoPrestamoId == abonoPorBoleta.PagoPrestamoId);

                if (abonoPrestamo == null)
                {
                    boleta.RemoverAbonoPrestamo(abonoPorBoleta);
                }
            }
        }

        private bool TryActulizarAbonoPorBoleta(PagoPrestamos abonoPrestamo, PagoPrestamosDTO abonoPrestamoDTO, out IEnumerable<string> mensajesValidacion)
        {
            abonoPrestamo.ActualizarAbonoPrestamo(abonoPrestamoDTO.FormaDePago, abonoPrestamoDTO.BancoId, abonoPrestamoDTO.NoDocumento, 
                                                  abonoPrestamoDTO.BoletaId, abonoPrestamoDTO.MontoAbono, abonoPrestamoDTO.Observaciones);
            mensajesValidacion = abonoPrestamo.GetValidationErrors();

            if (mensajesValidacion.Any())
            {
                return false;
            }

            return true;
        }

        private bool TryCrearAbonoPorBoleta(Boletas boleta, PagoPrestamosDTO abonoPrestamo, out IEnumerable<string> mensajesValidacion)
        {
            var nuevoAbonoPrestamo = new PagoPrestamos(abonoPrestamo.PrestamoId, abonoPrestamo.FormaDePago, abonoPrestamo.BancoId, abonoPrestamo.BoletaId, 
                                                       abonoPrestamo.MontoAbono, abonoPrestamo.NoDocumento, abonoPrestamo.Observaciones);
            mensajesValidacion = nuevoAbonoPrestamo.GetValidationErrors();

            if (mensajesValidacion.Any())
            {
                return false;
            }

            boleta.AgregarAbonoPrestamo(nuevoAbonoPrestamo);

            return true;
        }
        
        private void RemoverCache()
        {
            var listaKey = new List<string>
            {                
                KeyCache.PagoPrestamos,
                KeyCache.Prestamos,
                KeyCache.Boletas
            };
            _cacheAdapter.Remove(listaKey);
        }        
    }
}
