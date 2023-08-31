using System.Collections.Generic;
using System.Linq;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.DTOs.RequestDTOs.FacturaDetalleItems;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.Aplicacion;

namespace ComercialColindres.Servicios.Servicios
{
    public class FacturaDetalleItemAppServices : IFacturaDetalleItemAppServices
    {
        ComercialColindresContext _unitOfWork;
        private readonly ICacheAdapter _cacheAdapter;        

        public FacturaDetalleItemAppServices(ComercialColindresContext unitOfWork, ICacheAdapter cacheAdapter)
        {
            _unitOfWork = unitOfWork;
            _cacheAdapter = cacheAdapter;
        }

        public List<FacturaDetalleItemDto> GetDetailItemsByInvoiceId(GetDetailItemsByInvoiceId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.FacturaDetalleItems, nameof(GetDetailItemsByInvoiceId), request.InvoiceId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<FacturaDetalleItem> datos = _unitOfWork.FacturaDetalleItem.Where(b => b.FacturaId == request.InvoiceId)
                                                                                 .OrderBy(o => o.Precio).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<FacturaDetalleItem>, IEnumerable<FacturaDetalleItemDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public FacturaDetalleItemDto SaveInvoiceDetailItems(PostInvoiceDetailItems request)
        {
            Factura invoice = _unitOfWork.Facturas.Find(request.InvoiceId);

            if (invoice == null) { return new FacturaDetalleItemDto { MensajeError = "FacturaId NO EXISTE!" }; }

            if (invoice.Estado != Estados.NUEVO) { return new FacturaDetalleItemDto { MensajeError = "Factura debe estar en estado Nuevo" }; }

            RemoveInvoiceDetail(invoice, request.FacturaDetalleItems);
            List<FacturaDetalleItem> currentInvoiceDetail = invoice.FacturaDetalleItems.ToList();
            List<CategoriaProductos> productCategories = _unitOfWork.CategoriaProductos.ToList();
            IEnumerable<string> validationMessage;

            foreach (FacturaDetalleItemDto item in request.FacturaDetalleItems)
            {
                FacturaDetalleItem detailItem = currentInvoiceDetail.FirstOrDefault(d => d.FacturaDetalleItemId == item.FacturaDetalleItemId);
                CategoriaProductos productCategory = productCategories.FirstOrDefault(p => p.CategoriaProductoId == item.CategoriaProductoId);

                //new item
                if (detailItem == null)
                {
                    FacturaDetalleItem newInvoiceDetail = new FacturaDetalleItem(invoice, item.Cantidad, productCategory, item.Precio);
                    validationMessage = newInvoiceDetail.GetValidationErrors();

                    if (validationMessage.Any()) { return new FacturaDetalleItemDto { MensajeError = Utilitarios.CrearMensajeValidacion(validationMessage) }; }

                    invoice.AddDetailItem(newInvoiceDetail);
                    continue;
                }

                detailItem.UpdateInvoiceItem(item.Cantidad, productCategory, item.Precio);
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CreateInvoiceDetailItem");
            _unitOfWork.Commit(transaccion);

            RemoverCacheFacturaDetalleItems();

            return new FacturaDetalleItemDto();
        }

        private void RemoveInvoiceDetail(Factura invoice, List<FacturaDetalleItemDto> detailRequest)
        {
            List<FacturaDetalleItem> currentInvoiceDetail = invoice.FacturaDetalleItems.ToList();

            foreach(FacturaDetalleItem currentDetail in currentInvoiceDetail)
            {
                FacturaDetalleItemDto detail = detailRequest.FirstOrDefault(d => d.FacturaDetalleItemId == currentDetail.FacturaDetalleItemId);

                if (detail == null)
                {
                    invoice.RemoveDetailItem(currentDetail);
                }
            }
        }

        private void RemoverCacheFacturaDetalleItems()
        {
            var listaKey = new List<string>
            {
                KeyCache.Facturas,
                KeyCache.FacturaDetalleBoletas,
                KeyCache.FacturaDetalleItems
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
