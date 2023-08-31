using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class UsuariosEspecificaciones
    {
        public static Specification<Usuarios> FiltroBusqueda(string filtro)
        {
            var especification = new Specification<Usuarios>(r => r.UsuarioId != (-1));
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var filtroEspecificacion = new Specification<Usuarios>(r => r.Nombre.Contains(filtro) || r.Usuario.Contains(filtro));
                especification &= filtroEspecificacion;
            }
            return especification;
        }
    }
}
