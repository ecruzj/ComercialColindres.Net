namespace ComercialColindres.Datos.Entorno
{
    public interface IIdentityGenerator
    {
        /// <summary>
        /// Generates a new secuential transaction identities with GUIDs across system boundaries, ideal for databases. 
        /// </summary>
        /// <returns></returns>
        TransactionIdentity NewSequentialTransactionIdentity();        
    }
}
