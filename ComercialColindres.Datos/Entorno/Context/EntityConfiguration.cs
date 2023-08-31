using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Context
{
    public class EntityConfiguration<TEntity> : EntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        //protected EntityConfiguration()
        //{
        //    Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
        //    Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion").IsRequired().IsUnicode(false).HasMaxLength(50);
        //    //Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion").IsRequired().IsUnicode(false).HasMaxLength(50);
        //    Property(t => t.ModificadoPor).HasColumnName("ModificadoPor").IsRequired().IsUnicode(false).HasMaxLength(15);
        //    //Property(t => t.TransaccionUId).HasColumnName("TransaccionUId").IsRequired();
        //}
    }
}
