using ComercialColindres.Datos.Clases;
using ComercialColindres.Datos.Entorno.Modelos;
using ComercialColindres.Datos.Recursos;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class Usuarios : IValidacionesEntidades
    {
        public Usuarios(string usuario, string nombre)
        {
            Usuario = usuario;
            Nombre = nombre;
            Estado = Estados.ACTIVO;
            this.UsuariosOpciones = new List<UsuariosOpciones>();
        }
        protected Usuarios()
        {

        }
        public int UsuarioId { get; private set; }
        public string Usuario { get; private set; }
        public string Nombre { get; private set; }
        public string Clave { get; private set; }
        public string Estado { get; private set; }
        public virtual ICollection<UsuariosOpciones> UsuariosOpciones { get; set; }

        public IEnumerable<string> GetValidationErrors()
        {
            var listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(Usuario))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Usuario");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Nombre");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Clave))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Clave");
                listaErrores.Add(mensaje);
            }

            if (string.IsNullOrWhiteSpace(Estado))
            {
                var mensaje = string.Format(MensajesValidacion.Campo_Requerido, "Estado");
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        public bool TienePermisoAdministrativo()
        {
            if (UsuariosOpciones == null || !UsuariosOpciones.Any())
            {
                return false;
            }

            return UsuariosOpciones.Any(p => p.Opcion.Nombre == "CuentasAdministrativas");
        }

        public IEnumerable<string> GetValidationErrorsDelete()
        {
            var listaErrores = new List<string>();

            if (UsuariosOpciones.Any())
            {
                listaErrores.Add(MensajesValidacion.Validacion_NoSePuedeEliminar + ", existen opciones registradas para este usuario");
            }

            return listaErrores;
        }

        public void DesactivarUsuario()
        {
            Estado = Estados.INACTIVO;
        }

        public void AsignarClave(string clave)
        {
            var nuevaClave = ClaseCifradoContenido.CifrarTextoAES(clave);
            Clave = nuevaClave;
        }

        public string ObtenerClave()
        {
            var nuevaClave = ClaseCifradoContenido.DescifrarTextoAES(Clave);
            return nuevaClave;
        }

        public List<UsuariosSucursalesAsignadas> ObtenerSucursalesAsignadas()
        {
            if (UsuariosOpciones == null)
            {
                return new List<UsuariosSucursalesAsignadas>();
            }
            return (from reg in UsuariosOpciones
                    group reg by new
                    {
                        reg.SucursalId,
                        reg.Sucursal.Nombre
                    } into grupo
                    select new UsuariosSucursalesAsignadas
                    {
                        SucursalId = (int)grupo.Key.SucursalId,
                        Nombre = grupo.Key.Nombre
                    }).ToList();
        }

        public void Actualizar(string usuario, string nombre)
        {
            Usuario = usuario;
            Nombre = nombre;
        }

        public void ActualizarUsuariosOpciones(List<UsuariosOpciones> usuariosOpcionesNueva)
        {
            var opcionesOriginal = UsuariosOpciones.ToList();
            foreach (var opcionUsuario in opcionesOriginal)
            {
                var opcionVigente = usuariosOpcionesNueva.Any(r => r.UsuarioOpcionId == opcionUsuario.UsuarioOpcionId);
                if (opcionVigente == false)
                {
                    RemoverOpcion(opcionUsuario);
                }
            }

            foreach (var opcionUsuario in usuariosOpcionesNueva)
            {
                var yaExiste = UsuariosOpciones.Any(r => r.OpcionId == opcionUsuario.OpcionId && r.SucursalId == opcionUsuario.SucursalId);
                if (yaExiste == false)
                {
                    AgregarOpcion(opcionUsuario.SucursalId, opcionUsuario.OpcionId);
                }
            }
        }

        public void RemoverUsuariosOpciones(List<UsuariosOpciones> usuariosOpcionesARemover)
        {
            foreach (var usuarioOpcion in usuariosOpcionesARemover)
            {
                RemoverOpcion(usuarioOpcion);
            }
        }

        void AgregarOpcion(int empresaId, int opcionId)
        {
            var nuevaOpcion = new UsuariosOpciones(UsuarioId, empresaId, opcionId);
            UsuariosOpciones.Add(nuevaOpcion);
        }

        void RemoverOpcion(UsuariosOpciones usuariosOpciones)
        {
            UsuariosOpciones.Remove(usuariosOpciones);
        }
    }
}
