namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IUsuariosDomainServices
    {

        /// <summary>
        /// Valida el usuario especificado sea el del sistema
        /// </summary>
        /// <param name="usuario">Usuario</param>
        /// <param name="clave">Clave</param>
        /// <returns>bool</returns>
        bool ValidarUsuarioPorDefectoSistema(string usuario, string clave);
    }
}
