using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComercialColindres.DTOs.RequestDTOs.CuentasBancarias;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;

namespace ComercialColindres.Servicios.Servicios
{
    public class CuentasBancariasAppServices : ICuentasBancariasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public CuentasBancariasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<CuentasBancariasDTO> Get(GetCuentasBancariasPorProveedorId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.CuentasBancarias, "GetCuentasBancariasPorClienteId", request.ProveedorId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<CuentasBancarias> datos = _unidadDeTrabajo.CuentasBancarias.Where(cb => cb.ProveedorId == request.ProveedorId).OrderByDescending(o => o.NombreAbonado).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<CuentasBancarias>, IEnumerable<CuentasBancariasDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostCuentasBancarias request)
        {
            var cliente = _unidadDeTrabajo.Proveedores.Find(request.ProveedorId);

            if (cliente == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "El Cliente no existe"
                };
            }

            var listaCuentasBancarias = cliente.CuentasBancarias.ToList();

            //Verificar si removieron Items
            foreach (var cuenta in listaCuentasBancarias)
            {
                var cuentaBancaria = request.CuentasBancarias
                                   .FirstOrDefault(c => c.CuentaId == cuenta.CuentaId);

                if (cuentaBancaria == null)
                {
                    cliente.CuentasBancarias.Remove(cuenta);
                }
            }

            IEnumerable<string> mensajesValidacion;

            foreach (var cuenta in request.CuentasBancarias)
            {
                var cuentaBancaria = listaCuentasBancarias
                                     .FirstOrDefault(c => c.CuentaId == cuenta.CuentaId); 

                //Hubo una Actualizacion
                if (cuentaBancaria != null)
                {
                    cuentaBancaria.ActualizarCuentaBancaria(cuenta.NombreAbonado, cuenta.CedulaNo, cuenta.BancoId, cuenta.CuentaNo, cuenta.Estado);
                    mensajesValidacion = cuentaBancaria.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }
                }
                else
                {
                    //Se agrego una nueva cuenta
                    var nuevaCuenta = new CuentasBancarias(request.ProveedorId, cuenta.BancoId, cuenta.NombreAbonado, cuenta.CedulaNo, cuenta.CuentaNo);
                    mensajesValidacion = nuevaCuenta.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    _unidadDeTrabajo.CuentasBancarias.Add(nuevaCuenta);
                }
            }
                        
            _unidadDeTrabajo.SaveChanges();

            RemoverCache();

            return new ActualizarResponseDTO();
        }
        
        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.CuentasBancarias
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
