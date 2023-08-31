using System;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class UsuariosDomainServices : IUsuariosDomainServices
    {
        // <inheritDoc/>
        public bool ValidarUsuarioPorDefectoSistema(string usuario, string clave)
        {
            var sumaDia = DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Month;
            var usuarioActualAuto1 = string.Format("admin{0}", sumaDia);
            var claveActualAuto = string.Format("admin{0}", DateTime.Now.Year - DateTime.Now.Day - DateTime.Now.Month);
            return (usuario == usuarioActualAuto1 && clave == claveActualAuto.ToString());
        }
    }
}
