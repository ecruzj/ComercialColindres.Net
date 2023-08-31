using ComercialColindres.Clases;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BillsWithWeightsError;
using ComercialColindres.Enumeraciones;
using GalaSoft.MvvmLight.Command;
using MisControlesWPF.Modelos;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ComercialColindres.ViewModels.Reportes
{
    public class RptBillsWithWeightsErrorViewModel : BaseVM
    {
        private JsonServiceClient _client;
        private readonly IServiciosComunes _serviciosComunes;

        public RptBillsWithWeightsErrorViewModel(IServiciosComunes serviciosComunes)
        {
            _serviciosComunes = serviciosComunes;
            InicializarComandos();
        }

        public List<AutoCompleteEntry> Plantas
        {
            get
            {
                return _plantas;
            }
            set
            {
                _plantas = value;
                RaisePropertyChanged(nameof(Plantas));
            }
        }
        private List<AutoCompleteEntry> _plantas;

        public AutoCompleteEntry PlantaSelected
        {
            get
            {
                return _plantaSelected;
            }
            set
            {
                _plantaSelected = value;
                RaisePropertyChanged(nameof(PlantaSelected));

                CleanInformation();
            }
        }
        private AutoCompleteEntry _plantaSelected;

        public List<RptBillWithWeightsErrorDto> BillsWithWeightsErros
        {
            get { return _billsWithWeightsErros; }
            set
            {
                _billsWithWeightsErros = value;
                RaisePropertyChanged(nameof(BillsWithWeightsErros));
            }
        }
        private List<RptBillWithWeightsErrorDto> _billsWithWeightsErros;

        public decimal Saldo
        {
            get { return _saldo; }
            set
            {
                _saldo = value;
                RaisePropertyChanged(nameof(Saldo));
            }
        }
        private decimal _saldo;

        public RelayCommand CommandGetInformation { get; set; }
        public RelayCommand<string> CommandFindPlanta { get; set; }

        private void InicializarComandos()
        {
            CommandGetInformation = new RelayCommand(GetInformation);
            CommandFindPlanta = new RelayCommand<string>(FindPlanta);
        }

        private void GetInformation()
        {
            CleanInformation();

            var uri = InformacionSistema.Uri_ApiService;
            _client = new JsonServiceClient(uri);

            int plantaId = PlantaSelected != null && PlantaSelected.Id != null
                           ? (int)PlantaSelected.Id
                           : 0;

            var request = new GetBillsWithWeightsError
            {
                PlantaId = plantaId,
                RequestUserInfo = _serviciosComunes.GetRequestUserInfo()
            };

            MostrarVentanaEsperaPrincipal = Visibility.Visible;
            _client.GetAsync(request, res =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                if (!res.Any())
                {
                    _serviciosComunes.MostrarNotificacion(EventType.Warning, "No hay información que mostrar");
                    return;
                }

                BillsWithWeightsErros = res;
                RaisePropertyChanged(nameof(BillsWithWeightsErros));

                Saldo = res.Sum(s => s.Saldo);
            }, (r, ex) =>
            {
                MostrarVentanaEsperaPrincipal = Visibility.Collapsed;
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }

        private void CleanInformation()
        {
            BillsWithWeightsErros = new List<RptBillWithWeightsErrorDto>();

            Saldo = 0.0m;
            RaisePropertyChanged(nameof(Saldo));
        }

        private void FindPlanta(string valorBusqueda)
        {
            var uri = InformacionSistema.Uri_ApiService;
            var client = new JsonServiceClient(uri);
            var request = new GetPlantasPorValorBusqueda
            {
                ValorBusqueda = valorBusqueda
            };
            client.GetAsync(request, res =>
            {
                if (res.Any())
                {
                    Plantas = (from reg in res
                               select new AutoCompleteEntry(reg.NombrePlanta, reg.PlantaId)).ToList();
                }
            }, (r, ex) =>
            {
                _serviciosComunes.MostrarNotificacion(EventType.Error, ex.Message);
            });
        }
    }
}
