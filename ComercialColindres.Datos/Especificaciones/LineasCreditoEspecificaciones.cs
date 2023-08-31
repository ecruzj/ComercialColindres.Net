using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class LineasCreditoEspecificaciones
    {
        public static Specification<LineasCredito> FiltrarLineasCreditoBusqueda(string valorBusqueda, int sucursalId, Usuarios usuario)
        {
            var specification = new Specification<LineasCredito>(lc => lc.CodigoLineaCredito != string.Empty && lc.SucursalId == sucursalId);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            var permisoAdministrativo = new Specification<LineasCredito>(b => b.CuentasFinanciera.EsCuentaAdministrativa == usuario.TienePermisoAdministrativo()
                || b.CuentasFinanciera.EsCuentaAdministrativa == false);
            specification &= permisoAdministrativo;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<LineasCredito>(b => b.CodigoLineaCredito.ToUpper().Contains(valorBuscar)
                || b.CuentasFinanciera.CuentaNo.ToUpper().Contains(valorBuscar)
                || b.CuentasFinanciera.CuentasFinancieraTipos.Descripcion.ToUpper().Contains(valorBuscar)
                || b.NoDocumento.ToUpper().Contains(valorBuscar));
                specification &= valorBusquedaSpecification;
            }

            return specification;
        }

        public static Specification<LineasCredito> FiltrarLineasCreditoPorBancoPorTipoCuenta(int sucursalId, Usuarios usuario, int bancoId, int cuentaFinancieraTipoId)
        {
            var specification = new Specification<LineasCredito>(lc => lc.CodigoLineaCredito != string.Empty && lc.SucursalId == sucursalId);
            var tienePermisoAdministrativo = usuario != null ? usuario.TienePermisoAdministrativo() : false;

            if (bancoId > 0 && cuentaFinancieraTipoId > 0)
            {
                var valorBusquedaSpecification = new Specification<LineasCredito>(l => l.CuentasFinanciera.CuentasFinancieraTipos.RequiereBanco == true
                                                                   && l.CuentasFinanciera.BancoId == bancoId
                                                                   && l.EsLineaCreditoActual == true
                                                                   && l.CuentasFinanciera.CuentaFinancieraTipoId == cuentaFinancieraTipoId
                                                                   && (l.CuentasFinanciera.EsCuentaAdministrativa == tienePermisoAdministrativo
                                                                       || l.CuentasFinanciera.EsCuentaAdministrativa == false));
                specification &= valorBusquedaSpecification;
            }

            return specification;
        }
    }
}
