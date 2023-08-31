using ComercialColindres.DTOs.RequestDTOs.Opciones;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IOpcionesAppServices
    {
        /// <summary>
        /// Metodo que devuelve todas las opciones del sistema
        /// </summary>
        /// <param name="request">FindOpciones</param>
        /// <returns>OpcionesDTO</returns>
        List<OpcionesDTO> Get(FindOpciones request);
        ActualizarResponseDTO Post(PostOpcion request);
        ActualizarResponseDTO Put(PutOpcion request);
        EliminarResponseDTO Delete(DeleteOpcion request);
    }
}
