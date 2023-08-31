using ComercialColindres.DTOs.RequestDTOs.Sucursales;
using ComercialColindres.DTOs.RequestDTOs.Usuarios;

namespace ComercialColindres.Clases
{
    public class InformacionSistema
    {
        public static string Uri_ApiService { get; set; }
        public static string RutaReporte { get; set; }
        public static UsuariosDTO UsuarioActivo { get; set; }
        public static SucursalesDTO SucursalActiva { get; set; }
    }
}
