using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class UsuariosMap : EntityTypeConfiguration<Usuarios>
    {
        public UsuariosMap()
        {
            // Primary Key
            this.HasKey(t => t.UsuarioId);

            // Properties
            this.Property(t => t.Usuario)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Clave)
                .IsRequired();

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);


            // Table & Column Mappings
            this.ToTable("Usuarios");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
            this.Property(t => t.Nombre).HasColumnName("Nombre");
            this.Property(t => t.Clave).HasColumnName("Clave");
            this.Property(t => t.Estado).HasColumnName("Estado");

            // Relationships
        }
    }
}
