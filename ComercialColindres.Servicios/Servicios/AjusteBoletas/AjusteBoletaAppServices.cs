using System.Collections.Generic;
using System.Linq;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletas;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Servicios.Servicios
{
    public class AjusteBoletaAppServices : IAjusteBoletaAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        private readonly IAjusteBoletaDomainServices _ajusteBoletaDomainServices;

        public AjusteBoletaAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IAjusteBoletaDomainServices ajusteBoletaDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _ajusteBoletaDomainServices = ajusteBoletaDomainServices;
        }

        public BusquedaAjusteBoletas GetAjusteBoletas(GetByValorAjusteBoletas request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.AjusteBoletas, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = AjusteBoletasSpecification.FilterAjusteBoletas(request.Filtro);
                List<AjusteBoleta> datos = _unidadDeTrabajo.AjusteBoleta.Where(especificacion.EvalFunc).OrderByDescending(o => o.FechaTransaccion).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<AjusteBoleta>, IEnumerable<AjusteBoletaDto>>(datosPaginados.Items as IEnumerable<AjusteBoleta>);

                var dto = new BusquedaAjusteBoletas
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<AjusteBoletaDto>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public AjusteBoletaDto CreateAjusteBoleta(PostAjusteBoleta request)
        {
            Boletas boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (!_ajusteBoletaDomainServices.TryCreateBoletaAjuste(boleta, out string errorMessage))
            {
                return new AjusteBoletaDto { ValidationErrorMessage = errorMessage };
            }

            RemoverCacheAjusteBoletas();

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CreateAjusteBoleta");
            _unidadDeTrabajo.Commit(transaccion);

            return new AjusteBoletaDto();
        }
        
        public AjusteBoletaDto DeleteAjusteBoleta(DeleteAjusteBoleta request)
        {
            AjusteBoleta ajusteBoleta = _unidadDeTrabajo.AjusteBoleta.Find(request.AjusteBoletaId);

            if (!_ajusteBoletaDomainServices.TryDeleteAjusteBoleta(ajusteBoleta, out string errorMessage))
            {
                return new AjusteBoletaDto { ValidationErrorMessage = errorMessage };
            }

            RemoverCacheAjusteBoletas();

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "DeleteAjusteBoleta");
            _unidadDeTrabajo.Commit(transaccion);

            return new AjusteBoletaDto();
        }

        public AjusteBoletaDto ActiveAjusteBoleta(PostActiveAjusteBoleta request)
        {
            AjusteBoleta ajusteBoleta = _unidadDeTrabajo.AjusteBoleta.Find(request.AjusteBoletaId);

            if (!_ajusteBoletaDomainServices.TryActiveAjusteBoleta(ajusteBoleta, out string errorMessage))
            {
                return new AjusteBoletaDto { ValidationErrorMessage = errorMessage };
            }

            RemoverCacheAjusteBoletas();

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "ActivateAjusteBoleta");
            _unidadDeTrabajo.Commit(transaccion);

            return new AjusteBoletaDto();
        }

        private void RemoverCacheAjusteBoletas()
        {
            var listaKey = new List<string>
            {
                KeyCache.Boletas,
                KeyCache.AjusteBoletas,
                KeyCache.AjusteBoletaDetalles,
                KeyCache.AjusteBoletaPagos
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
