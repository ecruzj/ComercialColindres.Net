using System;

namespace ComercialColindres.Datos.Entorno.Context
{
    public abstract class Entity
    {
        protected Entity() { }

        public Entity(string modificadoPor, string tipoTransaccion)
        {
            this.ModificadoPor = modificadoPor;
            this.DescripcionTransaccion = tipoTransaccion;
        }

        public string ModificadoPor { get; set; }
        public string TipoTransaccion { get; set; }
        public string DescripcionTransaccion { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public Guid TransaccionUId { get; set; }
    }
}
