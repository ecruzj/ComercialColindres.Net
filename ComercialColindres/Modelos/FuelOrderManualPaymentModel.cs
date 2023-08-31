using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class FuelOrderManualPaymentModel : ObservableObject
    {
        public int FuelOrderManualPaymentId { get; set; }
        public int FuelOrderId { get; set; }
        public string WayToPay { get; set; }
        public int? BankId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string BankName { get; set; }

        public string BankReference
        {
            get { return _bankReference; }
            set { _bankReference = value;
                RaisePropertyChanged(nameof(BankReference));
            }
        }
        private string _bankReference;

        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value;
                RaisePropertyChanged(nameof(Amount));
            }
        }
        private decimal _amount;

        public string Observations
        {
            get { return _observations; }
            set { _observations = value;
                RaisePropertyChanged(nameof(Observations));
            }
        }
        private string _observations;

        public decimal TotalPayments
        {
            get { return _totalPayments; }
            set { _totalPayments = value;
                RaisePropertyChanged(nameof(TotalPayments));
            }
        }
        private decimal _totalPayments;

    }
}
