using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class UsuariosOpciones
    {
        public UsuariosOpciones(int usuarioId, int sucursalId, int opcionId)
        {
            UsuarioId = usuarioId;
            SucursalId = sucursalId;
            OpcionId = opcionId;
        }

        protected UsuariosOpciones()
        {

        }

        public int UsuarioOpcionId { get; set; }
        public int UsuarioId { get; private set; }
        public int SucursalId { get; private set; }
        public int OpcionId { get; private set; }
        public virtual Sucursales Sucursal { get; set; }
        public virtual Opciones Opcion { get; set; }
        public virtual Usuarios Usuario { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            throw new NotImplementedException();
        }
    }
}
