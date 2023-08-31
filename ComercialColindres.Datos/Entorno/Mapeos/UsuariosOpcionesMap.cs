using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class UsuariosOpcionesMap : EntityTypeConfiguration<UsuariosOpciones>
    {
        public UsuariosOpcionesMap()
        {
            // Primary Key
            this.HasKey(t => t.UsuarioOpcionId);

            // Properties
            // Table & Column Mappings
            this.ToTable("UsuariosOpciones");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.SucursalId).HasColumnName("SucursalId");
            this.Property(t => t.UsuarioOpcionId).HasColumnName("UsuarioOpcionId");
            this.Property(t => t.OpcionId).HasColumnName("OpcionId");

            // Relationships
            this.HasRequired(t => t.Opcion)
                .WithMany(t => t.UsuariosOpciones)
                .HasForeignKey(d => d.OpcionId);

            this.HasRequired(t => t.Sucursal)
               .WithMany(t => t.UsuariosOpciones)
               .HasForeignKey(d => d.SucursalId);

            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.UsuariosOpciones)
                .HasForeignKey(d => d.UsuarioId);
        }
    }
}
