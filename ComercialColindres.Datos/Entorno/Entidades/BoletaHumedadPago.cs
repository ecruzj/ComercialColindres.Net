using ComercialColindres.Datos.Entorno.Context;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class BoletaHumedadPago : Entity
    {
        public BoletaHumedadPago(Boletas boleta, BoletaHumedad boletaHumedad)
        {
            this.Boleta = boleta;
            this.BoletaHumedad = boletaHumedad;
            this.BoletaId = boleta.BoletaId;
            this.BoletaHumedadId = boletaHumedad.BoletaHumedadId;
        }

        protected BoletaHumedadPago() { }

        public int BoletaHumedadPagoId { get; set; }
        public int BoletaId { get; set; }
        public int BoletaHumedadId { get; set; }
        public virtual BoletaHumedad BoletaHumedad { get; set; }
        public virtual Boletas Boleta { get; set; }
    }
}
