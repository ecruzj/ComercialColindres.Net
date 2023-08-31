using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.FuelOrderManualPayments
{
    public class FuelOrderManualPaymentDto : BaseDTO
    {
        public int FuelOrderManualPaymentId { get; set; }
        public int FuelOrderId { get; set; }
        public string WayToPay { get; set; }
        public int? BankId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string BankReference { get; set; }
        public decimal Amount { get; set; }
        public string Observations { get; set; }


        public string BankName { get; set; }

        public bool IsFuelOrderClosed()
        {
            throw new NotImplementedException();
        }
    }
}
