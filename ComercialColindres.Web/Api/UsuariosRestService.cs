using ComercialColindres.DTOs.RequestDTOs.Usuarios;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class UsuariosService : IService
    {
        private readonly IUsuariosAppServices _usuariosServicioAplicacion;

        public UsuariosService(IUsuariosAppServices usuariosServicioAplicacion)
        {
            _usuariosServicioAplicacion = usuariosServicioAplicacion;
        }

        public object Delete(DeleteUsuario request)
        {
            return _usuariosServicioAplicacion.EliminarUsuario(request);
        }

        public object Get(FindUsuarios request)
        {
            return _usuariosServicioAplicacion.ObtenerUsuarios(request);
        }

        public object Get(GetUsuario request)
        {
            return _usuariosServicioAplicacion.ObtenerUsuario(request);
        }

        public object Post(CreateUsuario request)
        {
            return _usuariosServicioAplicacion.CrearUsuario(request);
        }

        public object Put(UpdateUsuario request)
        {
            return _usuariosServicioAplicacion.ActualizarUsuario(request);
        }
    }
}