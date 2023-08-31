using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class CorrelativosMap : EntityTypeConfiguration<Correlativos>
    {
        public CorrelativosMap()
        {
            HasKey(t => t.CorrelativoId);

            ToTable("Correlativos");
            Property(e => e.CorrelativoId).HasColumnName("CorrelativoId").IsRequired();
            Property(e => e.CodigoCorrelativo).HasColumnName("CodigoCorrelativo").IsRequired().HasMaxLength(10);
            Property(e => e.Prefijo).HasColumnName("Prefijo").IsRequired();
            Property(e => e.Letra).HasColumnName("Letra").IsRequired();
            Property(e => e.Tamaño).HasColumnName("Tamaño").IsRequired();
            Property(e => e.CorrelativoActual).HasColumnName("CorrelativoActual").IsRequired();
            Property(e => e.DefinidoPorUsuario).HasColumnName("DefinidoPorUsuario").IsRequired();
            Property(e => e.ControlarPorPrefijo).HasColumnName("ControlarPorPrefijo").IsRequired();
            Property(e => e.CorrelativoInicialPermitido).HasColumnName("CorrelativoInicialPermitido").IsRequired();
            Property(e => e.CorrelativoFinalPermitido).HasColumnName("CorrelativoFinalPermitido").IsRequired();

            HasRequired(r => r.Sucursal)
                .WithMany(t => t.Correlativos)
                .HasForeignKey(k => k.SucursalId);
        }
    }
}
