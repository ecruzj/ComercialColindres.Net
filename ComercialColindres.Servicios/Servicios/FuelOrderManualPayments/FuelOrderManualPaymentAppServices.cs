using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs.FuelOrderManualPayments;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios.FuelOrderManualPayments
{
    public class FuelOrderManualPaymentAppServices : IFuelOrderManualPaymentAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unitOfWork;
        IOrdenCombustibleDomainServices _fuelOrderDomainServices;

        public FuelOrderManualPaymentAppServices(ComercialColindresContext unitOfWork, ICacheAdapter cacheAdapter, IOrdenCombustibleDomainServices fuelOrderDomainServices)
        {
            _unitOfWork = unitOfWork;
            _cacheAdapter = cacheAdapter;
            _fuelOrderDomainServices = fuelOrderDomainServices;
        }

        public List<FuelOrderManualPaymentDto> GetFuelOrderManualPayments(GetFuelOrderManualPayments request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var cacheKey = string.Format("{0}{1}{2}", KeyCache.FuelOrderManualPayments, nameof(GetFuelOrderManualPayments), request.FuelOrderId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<FuelOrderManualPayment> datos = _unitOfWork.FuelOrderManualPayments.Where(d => d.FuelOrderId == request.FuelOrderId)
                                                                            .OrderByDescending(o => o.PaymentDate).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<FuelOrderManualPayment>, IEnumerable<FuelOrderManualPaymentDto>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public FuelOrderManualPaymentDto SaveFuelOrderManualPayments(PostFuelOrderManualPayments request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            OrdenesCombustible fuelOrder = _unitOfWork.OrdenesCombustible.Find(request.FuelOrderId);

            if (fuelOrder == null) { return new FuelOrderManualPaymentDto { MensajeError = "Orden de Combustible no existe!" }; }

            List<Bancos> banks = _unitOfWork.Bancos.ToList();
            List<FuelOrderManualPayment> manualPayments = CreateInstanceFuelManualPayments(fuelOrder, banks, request.FuelOrderManualPayments);

            if (!_fuelOrderDomainServices.TryCreateManualPayments(fuelOrder, manualPayments, out string errorMessage))
            {
                return new FuelOrderManualPaymentDto
                {
                    MensajeError = errorMessage
                };
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CreateFuelOrderManualPayments");
            _unitOfWork.Commit(transaccion);

            RemoverCache();

            return new FuelOrderManualPaymentDto();
        }

        private List<FuelOrderManualPayment> CreateInstanceFuelManualPayments(OrdenesCombustible fuelOrder, List<Bancos> banks, List<FuelOrderManualPaymentDto> fuelOrderManualPayments)
        {
            return fuelOrderManualPayments.Select(paymentItem =>
            {
                Bancos bank = banks.FirstOrDefault(b => b.BancoId == paymentItem.BankId);
                return new FuelOrderManualPayment(
                    paymentItem.FuelOrderManualPaymentId,
                    fuelOrder,
                    paymentItem.WayToPay,
                    bank,
                    paymentItem.PaymentDate,
                    paymentItem.BankReference,
                    paymentItem.Amount,
                    paymentItem.Observations
                );
            }).ToList();
        }

        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.OrdenesCombustible,
                KeyCache.FuelOrderManualPayments
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
