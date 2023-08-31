using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Usuarios
{
    [Route("/usuarios/buscar", "GET")]
    public class GetUsuario : IReturn<UsuariosDTO>
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public int SucursalId { get; set; }
    }

    [Route("/usuarios/", "GET")]
    public class FindUsuarios : IReturn<List<UsuariosDTO>>
    {
        public string Filtro { get; set; }
    }

    [Route("/usuarios/", "POST")]
    public class CreateUsuario : IReturn<ActualizarResponseDTO>
    {
        public UsuariosDTO Usuario { get; set; }
    }

    [Route("/usuarios/", "PUT")]
    public class UpdateUsuario : IReturn<ActualizarResponseDTO>
    {
        public UsuariosDTO Usuario { get; set; }
    }

    [Route("/usuarios/", "DELETE")]
    public class DeleteUsuario : IReturn<EliminarResponseDTO>
    {
        public int UsuarioId { get; set; }
    }
}
