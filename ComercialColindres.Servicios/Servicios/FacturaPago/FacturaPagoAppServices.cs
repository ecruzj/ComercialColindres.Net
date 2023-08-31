using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.Aplicacion;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios
{
    public class FacturaPagoAppServices : IFacturaPagoAppServices
    {
        ComercialColindresContext _unitOfWork;
        private readonly ICacheAdapter _cacheAdapter;

        public FacturaPagoAppServices(ComercialColindresContext unitOfWork, ICacheAdapter cacheAdapter)
        {
            _unitOfWork = unitOfWork;
            _cacheAdapter = cacheAdapter;
        }

        public List<FacturaPagoDto> GetInvoicePaymentByInvoiceId(GetPagosByInvoiceId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.FacturaPago, nameof(GetPagosByInvoiceId), request.InvoiceId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<FacturaPago> datos = _unitOfWork.FacturaPago.Where(b => b.FacturaId == request.InvoiceId).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<FacturaPago>, IEnumerable<FacturaPagoDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public FacturaPagoDto SaveInvoicePayment(PostInvoicePagos request)
        {
            Factura invoice = _unitOfWork.Facturas.Find(request.InvoiceId);

            if (invoice == null) { return new FacturaPagoDto { MensajeError = "FacturaId NO EXISTE!" }; }

            if (invoice.Estado == Estados.NUEVO || invoice.Estado == Estados.CERRADO) { return new FacturaPagoDto { MensajeError = "Factura debe estar en estado ACTIVO o En Proceso" }; }

            RemoveInvoicePayment(invoice, request.FacturaPagos);
            List<FacturaPago> currentInvoicePayments = invoice.FacturaPagos.ToList();
            List<Bancos> banks = _unitOfWork.Bancos.ToList();
            IEnumerable<string> validationMessage;

            foreach (FacturaPagoDto item in request.FacturaPagos)
            {
                FacturaPago itemPayment = currentInvoicePayments.FirstOrDefault(p => p.FacturaPagoId == item.FacturaPagoId);
                Bancos bank = banks.FirstOrDefault(b => b.BancoId == item.BancoId);

                //new item
                if (itemPayment == null)
                {
                    FacturaPago newPayment = new FacturaPago(invoice, bank, item.FormaDePago, item.ReferenciaBancaria, item.Monto, item.FechaDePago);
                    validationMessage = newPayment.GetValidationErrors();

                    if (validationMessage.Any()) { return new FacturaPagoDto { MensajeError = Utilitarios.CrearMensajeValidacion(validationMessage) }; }

                    invoice.AddInvoicePayment(newPayment);
                    continue;
                }

                //update item
                itemPayment.UpdateInvoicePayment(bank, item.ReferenciaBancaria, item.Monto);
            }

            if (!invoice.UpdateStatus(out string validation))
            {
                return new FacturaPagoDto { ValidationErrorMessage = validation };
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CreateInvoicePayment");
            _unitOfWork.Commit(transaccion);

            RemoverCacheFacturaPagos();

            return new FacturaPagoDto();
        }

        private void RemoveInvoicePayment(Factura invoice, List<FacturaPagoDto> paymentRequest)
        {
            List<FacturaPago> currentPayments = invoice.FacturaPagos.ToList();

            foreach (FacturaPago itemPayment in currentPayments)
            {
                FacturaPagoDto payment = paymentRequest.FirstOrDefault(p => p.FacturaPagoId == itemPayment.FacturaPagoId);

                if (payment == null)
                {
                    invoice.RemoveInvoicePayment(itemPayment);
                }
            }
        }

        private void RemoverCacheFacturaPagos()
        {
            var listaKey = new List<string>
            {
                KeyCache.Facturas,
                KeyCache.FacturaPago
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
