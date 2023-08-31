using System.Collections.Generic;
using System.Linq;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.DTOs.RequestDTOs.NotasCredito;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.Aplicacion;

namespace ComercialColindres.Servicios.Servicios
{
    public class NotaCreditoServices : INotaCreditoServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _uOW;

        public NotaCreditoServices(ComercialColindresContext uow, ICacheAdapter cacheAdapter)
        {
            _uOW = uow;
            _cacheAdapter = cacheAdapter;
        }

        public List<NotaCreditoDto> GetNotasCreditoByInvoiceId(GetNotasCreditoByInvoiceId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.NotasCredito, nameof(GetNotasCreditoByInvoiceId), request.InvoiceId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<NotaCredito> datos = _uOW.NotaCredito.Where(f => f.FacturaId == request.InvoiceId).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<NotaCredito>, IEnumerable<NotaCreditoDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public NotaCreditoDto SaveNotasCredito(PostNotasCredito request)
        {
            Factura invoice = _uOW.Facturas.Find(request.InvoiceId);

            if (invoice == null)
            {
                return new NotaCreditoDto { MensajeError = "FacturaId no existe!" };
            }

            if (invoice.Estado != Estados.ACTIVO)
            {
                return new NotaCreditoDto { MensajeError = "La Factura debe estar Activa" };
            }

            RemoveNotasCredito(invoice, request.NotasCredito);
            List<NotaCredito> currentNotasCredito = invoice.NotasCredito.ToList();
            List<NotaCredito> notasCredito = _uOW.NotaCredito.ToList();

            foreach (NotaCreditoDto notaRequest in request.NotasCredito)
            {
                NotaCredito nota = currentNotasCredito.FirstOrDefault(n => n.NotaCreditoId == notaRequest.NotaCreditoId);

                if (nota == null)
                {
                    nota = new NotaCredito(invoice, notaRequest.NotaCreditoNo, notaRequest.Monto, notaRequest.Observaciones);
                    invoice.AddNotaCredito(nota);
                }
                else
                {
                    nota.UpdateNotaCredito(notaRequest.NotaCreditoNo, notaRequest.Monto, notaRequest.Observaciones);
                }

                if (nota.GetValidationErrors().Any())
                {
                    return new NotaCreditoDto { MensajeError = Utilitarios.CrearMensajeValidacion(nota.GetValidationErrors()) };
                }

                if (AlreadyExistNotaCredito(notaRequest.NotaCreditoNo, nota, notasCredito))
                {
                    return new NotaCreditoDto { MensajeError = "La NotaCreditoNo ya existe!" };
                }
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CreateNotasCredito");
            _uOW.Commit(transaccion);

            RemoveCache();

            return new NotaCreditoDto();
        }

        private bool AlreadyExistNotaCredito(string notaCreditoNo, NotaCredito notaCredito, List<NotaCredito> notasCredito)
        {
            if (notaCredito.NotaCreditoId > 0)
            {
                return notasCredito.Any(n => n.NotaCreditoNo == notaCreditoNo && n.NotaCreditoId != notaCredito.NotaCreditoId);
            }

            return notasCredito.Any(n => n.NotaCreditoNo == notaCreditoNo);
        }

        private void RemoveNotasCredito(Factura invoice, List<NotaCreditoDto> notasCreditoRequest)
        {
            List<NotaCredito> currentNotasCredito = invoice.NotasCredito.ToList();

            foreach (NotaCredito notaCredito in currentNotasCredito)
            {
                NotaCreditoDto notaRequest = notasCreditoRequest.FirstOrDefault(n => n.NotaCreditoId == notaCredito.NotaCreditoId);

                if (notaRequest == null)
                {
                    invoice.RemoveNotaCredito(notaCredito);
                }
            }
        }

        private void RemoveCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.Facturas,
                KeyCache.NotasCredito
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
