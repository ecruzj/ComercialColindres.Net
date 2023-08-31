using ComercialColindres.DTOs.RequestDTOs.Usuarios;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IUsuariosAppServices
    {
        /// <summary>
        /// Metodo que se encarga de actualizar la informacion de un usuario
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ActualizarResponseDTO ActualizarUsuario(UpdateUsuario request);

        /// <summary>
        /// Metodo que se encarga de crear un usuario
        /// </summary>
        /// <param name="request">CreateUsuarios</param>
        /// <returns>ActualizarResponse</returns>
        ActualizarResponseDTO CrearUsuario(CreateUsuario request);

        /// <summary>
        /// Metodo que se encarga de buscar un determinado usuario
        /// </summary>
        /// <param name="request">GetUsuario</param>
        /// <returns>UsuariosDTO</returns>
        UsuariosDTO ObtenerUsuario(GetUsuario request);

        /// <summary>
        /// Elimina un Usuario
        /// </summary>
        /// <param name="request">DeleteUsuario</param>
        /// <returns>EliminarResponse</returns>
        EliminarResponseDTO EliminarUsuario(DeleteUsuario request);

        /// <summary>
        /// Metodo que devuelve todos los usuarios regisrados en el restaurante
        /// </summary>
        /// <param name="request">FindUsuarios</param>
        /// <returns>UsuariosDTO</returns>
        List<UsuariosDTO> ObtenerUsuarios(FindUsuarios request);
    }
}
