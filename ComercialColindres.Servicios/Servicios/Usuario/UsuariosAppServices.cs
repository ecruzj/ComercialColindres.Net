using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.DTOs.RequestDTOs.Usuarios;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ServidorCore.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios
{
    public class UsuariosAppServices : IUsuariosAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        IUsuariosDomainServices _usuariosDomainServices;
        ComercialColindresContext _unidadDeTrabajo;

        public UsuariosAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IUsuariosDomainServices usuariosDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _usuariosDomainServices = usuariosDomainServices;

        }

        // <inheritDoc/>
        public ActualizarResponseDTO ActualizarUsuario(UpdateUsuario request)
        {
            var dato = _unidadDeTrabajo.Usuarios.Find(request.Usuario.UsuarioId);
            if (dato == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Usuario Id No existe"
                };
            }

            var existeUsuario = _unidadDeTrabajo.Usuarios.FirstOrDefault(r => r.Usuario == request.Usuario.Usuario &&
            r.UsuarioId != request.Usuario.UsuarioId);
            if (existeUsuario != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Usuario YA existe, n se puede actualizar"
                };
            }

            dato.Actualizar(request.Usuario.Usuario, request.Usuario.Nombre);
            dato.AsignarClave(request.Usuario.Clave);

            var errorValidacion = dato.GetValidationErrors();
            if (errorValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(errorValidacion)
                };
            }

            var listaUsuariosNueva = ObtenerUsuariosOpcionesNuevo(dato.UsuarioId, request.Usuario.UsuariosOpciones);
            dato.ActualizarUsuariosOpciones(listaUsuariosNueva);

            _unidadDeTrabajo.SaveChanges();

            RemoverCacheUsuarios();

            return new ActualizarResponseDTO();
        }

        // <inheritDoc/>
        public ActualizarResponseDTO CrearUsuario(CreateUsuario request)
        {
            var dato = _unidadDeTrabajo.Usuarios.Find(request.Usuario.UsuarioId);
            if (dato != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Usuario ya existe, no se puede crear"
                };
            }
            var existeUsuario = _unidadDeTrabajo.Usuarios.FirstOrDefault(r => r.Usuario == request.Usuario.Usuario &&
            r.UsuarioId != request.Usuario.UsuarioId);
            if (existeUsuario != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Usuario YA existe, n se puede actualizar"
                };
            }

            dato = new Usuarios(request.Usuario.Usuario, request.Usuario.Nombre);
            dato.AsignarClave(request.Usuario.Clave);

            var errorValidacion = dato.GetValidationErrors();
            if (errorValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(errorValidacion)
                };
            }

            var listaUsuariosNueva = ObtenerUsuariosOpcionesNuevo(dato.UsuarioId, request.Usuario.UsuariosOpciones);
            dato.ActualizarUsuariosOpciones(listaUsuariosNueva);

            _unidadDeTrabajo.Usuarios.Add(dato);
            _unidadDeTrabajo.SaveChanges();

            RemoverCacheUsuarios();

            return new ActualizarResponseDTO();
        }

        // <inheritDoc/>
        public EliminarResponseDTO EliminarUsuario(DeleteUsuario request)
        {
            var dato = _unidadDeTrabajo.Usuarios.Find(request.UsuarioId);
            if (dato == null)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "Usuario Id no existe"
                };
            }

            var opcionesUsuario = dato.UsuariosOpciones.ToList();
            dato.RemoverUsuariosOpciones(opcionesUsuario);


            _unidadDeTrabajo.Usuarios.Remove(dato);
            _unidadDeTrabajo.SaveChanges();

            RemoverCacheUsuarios();

            return new EliminarResponseDTO();
        }

        // <inheritDoc/>
        public UsuariosDTO ObtenerUsuario(GetUsuario request)
        {
            if (string.IsNullOrWhiteSpace(request.Usuario))
            {
                return new UsuariosDTO
                {
                    MensajeError = "Debe de especificar un usuario"
                };
            }

            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.Usuarios, request.Usuario, request.Clave, request.SucursalId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var usuarioPorDefecto = _usuariosDomainServices.ValidarUsuarioPorDefectoSistema(request.Usuario, request.Clave);

                Usuarios usuario = null;
                if (usuarioPorDefecto)
                {
                    var usuarioDTO = CrearUsuarioPorDefecto(request.Usuario);
                    return usuarioDTO;
                }
                else
                {
                    var validacion = ValidarUsuario(request.Usuario, request.Clave, request.SucursalId, out usuario);
                    if (!string.IsNullOrWhiteSpace(validacion))
                    {
                        return new UsuariosDTO
                        {
                            MensajeError = validacion
                        };
                    }

                    var opcionesDeOtraEmpresa = usuario.UsuariosOpciones.Where(r => r.SucursalId != request.SucursalId).ToList();
                    usuario.RemoverUsuariosOpciones(opcionesDeOtraEmpresa);
                }

                var datosDTO = AutomapperTypeAdapter.ProyectarColeccionComo<Usuarios, UsuariosDTO>(usuario);

                RemoverCacheUsuarios();
                return datosDTO;
            });

            RemoverCacheUsuarios();
            return datosConsulta;
        }

        // <inheritDoc/>
        public List<UsuariosDTO> ObtenerUsuarios(FindUsuarios request)
        {
            var cacheKey = string.Format("{0}-{1}-{2}", KeyCache.Usuarios, "TODOS", request.Filtro);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var usuarioEspecificacion = UsuariosEspecificaciones.FiltroBusqueda(request.Filtro);
                var usuarios = _unidadDeTrabajo.Usuarios.Where(usuarioEspecificacion.EvalPredicate).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Usuarios>, IEnumerable<UsuariosDTO>>(usuarios);

                RemoverCacheUsuarios();
                return datosDTO.ToList();
            });

            RemoverCacheUsuarios();
            return datosConsulta;
        }

        private UsuariosDTO CrearUsuarioPorDefecto(string usuario)
        {
            var opcionId_MantenimientoUsuarios = Convert.ToInt16(ConfiguracionesSistema.OpcionId_MantenimientoUsuarios);
            var itemUsuario = new UsuariosDTO
            {
                Nombre = "SisAdmin",
                Usuario = usuario,
                UsuariosOpciones = new List<UsuariosOpcionesDTO>
                {
                    new UsuariosOpcionesDTO
                    {
                        OpcionId = opcionId_MantenimientoUsuarios,
                        NombreSucursal = "TODAS",
                        NombreOpcion = "Usuarios"
                    }
                }
            };

            return itemUsuario;
        }

        private string ValidarUsuario(string usuario, string clave, int sucursalId, out Usuarios itemUsuario)
        {
            itemUsuario = _unidadDeTrabajo.Usuarios.FirstOrDefault(r => r.Usuario == usuario);
            if (itemUsuario == null)
            {
                return "Usuario NO Valido";

            }
            var claveAlmacenada = itemUsuario.ObtenerClave();
            if (claveAlmacenada != clave)
            {
                return "Usuario NO Valido";
            }

            if (itemUsuario.UsuariosOpciones != null)
            {
                if (!itemUsuario.UsuariosOpciones.Any(r => r.SucursalId == sucursalId))
                {
                    return "Usuario NO Valido";
                }
            }

            return string.Empty;
        }


        List<UsuariosOpciones> ObtenerUsuariosOpcionesNuevo(int usuarioId, List<UsuariosOpcionesDTO> usuariosOpcionesDTO)
        {
            var lista = new List<UsuariosOpciones>();

            foreach (var item in usuariosOpcionesDTO)
            {
                var itemUsuarioOpcion = new UsuariosOpciones(usuarioId, item.SucursalId, item.OpcionId);
                lista.Add(itemUsuarioOpcion);
            }

            return lista;
        }

        void RemoverCacheUsuarios()
        {
            var listaKey = new List<string>
            {
                KeyCache.Usuarios,
                KeyCache.PagoDescargasDetalle,
                KeyCache.Prestamos,
                KeyCache.PrestamosTransferencias,
                KeyCache.CategoriaProductos,
                KeyCache.PrecioProductos,
                KeyCache.EquiposCategorias,
                KeyCache.Cuadrillas,
                KeyCache.PrecioDescargas,
                KeyCache.GasCreditoPagos,
                KeyCache.GasCreditos,
                KeyCache.OrdenesCombustible,
                KeyCache.LineasCredito,
                KeyCache.LineasCreditoDeducciones,
                KeyCache.Boletas,
                KeyCache.BoletasCierres,
                KeyCache.OrdenescompraProducto,
                KeyCache.OrdenescompraProductoDetalle,
                KeyCache.OrdenescompraDetalleBoleta,
                KeyCache.PagoDescargas,
                KeyCache.PagoDescargasDetalle,
                KeyCache.BoletasHumedad,
                KeyCache.BoletasHumedadAsignacion,
                KeyCache.BoletasHumedadPago,
                KeyCache.Facturas,
                KeyCache.FacturaDetalleBoletas,
                KeyCache.FacturaDetalleItems,
                KeyCache.FacturasCategorias,
                KeyCache.FacturaPago,
                KeyCache.BonificacionProducto,
                KeyCache.AjusteTipos,
                KeyCache.AjusteBoletas,
                KeyCache.AjusteBoletaDetalles,
                KeyCache.AjusteBoletaPagos,
                KeyCache.FuelOrderManualPayments
            };

            _cacheAdapter.Remove(listaKey);
        }
    }
}
