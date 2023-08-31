namespace ComercialColindres.DTOs.RequestDTOs.Usuarios
{
    public class UsuariosOpcionesDTO
    {
        public int UsuarioId { get; set; }
        public int SucursalId { get; set; }
        public int UsuarioOpcionId { get; set; }
        public int OpcionId { get; set; }

        public string NombreOpcion { get; set; }
        public string NombreSucursal { get; set; }
        public string TipoPropiedad { get; set; }
        public string TipoAcceso { get; set; }
    }
}
