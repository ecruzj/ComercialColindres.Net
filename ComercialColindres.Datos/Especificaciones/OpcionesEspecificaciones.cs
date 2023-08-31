using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class OpcionesEspecificaciones
    {
        public static Specification<Opciones> FiltroBusqueda(string filtro)
        {
            var especification = new Specification<Opciones>(r => r.OpcionId != (-1));
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var filtroEspecificacion = new Specification<Opciones>(r => r.Nombre.Contains(filtro));
                especification &= filtroEspecificacion;
            }
            return especification;
        }
    }
}
