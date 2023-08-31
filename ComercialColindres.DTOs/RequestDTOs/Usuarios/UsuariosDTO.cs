using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Usuarios
{
    public class UsuariosDTO : BaseDTO
    {
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public string Estado { get; set; }

        public List<UsuariosOpcionesDTO> UsuariosOpciones { get; set; }
        public List<UsuariosSucursalesAsignadasDTO> UsuariosSucursalesAsignadas { get; set; }
    }
}
