﻿namespace ComercialColindres.Datos.Entorno
{
    public class ADOIdentityGeneratorFactory : IIdentityFactory
    {
        #region Implementation of IIdentityFactory

        /// <summary>
        /// Create a new IIdentityGenerator.
        /// </summary>
        /// <returns>The IIdentityGenerator created.</returns>
        public IIdentityGenerator Create()
        {
            return new ADOIdentityGenerator();
        }

        #endregion
    }
}
