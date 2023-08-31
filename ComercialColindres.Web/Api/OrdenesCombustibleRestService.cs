using ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class OrdenesCombustibleRestService : IService
    {
        IOrdenesCombustibleAppServices _ordenesCombustibleAppServices;

        public OrdenesCombustibleRestService(IOrdenesCombustibleAppServices ordenesCombustibleAppServices)
        {
            _ordenesCombustibleAppServices = ordenesCombustibleAppServices;
        }

        public object Get(GetByValorOrdenesCombustible request)
        {
            return _ordenesCombustibleAppServices.Get(request);
        }

        public object Get(GetOrderFuelByVendorId request)
        {
            return _ordenesCombustibleAppServices.Get(request);
        }

        public object Get(GetOrdenesCombustiblePorGasCreditoId request)
        {
            return _ordenesCombustibleAppServices.Get(request);
        }

        public object Get(GetOrdenesCombustibleByBoletaId request)
        {
            return _ordenesCombustibleAppServices.Get(request);
        }

        public object Put(PutOrdenesCombustibleABoleta request)
        {
            return _ordenesCombustibleAppServices.AssignFuelOrdersToBoleta(request);
        }

        public object Post(PostOrdenesCombustible request)
        {
            return _ordenesCombustibleAppServices.Post(request);
        }

        public object Put(PutOrdenesCombustible request)
        {
            return _ordenesCombustibleAppServices.Put(request);
        }

        public object Delete(DeleteOrdenesCombustible request)
        {
            return _ordenesCombustibleAppServices.Delete(request);
        }
    }
}