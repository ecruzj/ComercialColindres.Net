using ComercialColindres.Datos.Entorno.Context;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class BoletaHumedadAsignacion : Entity
    {
        public BoletaHumedadAsignacion(Boletas boleta, BoletaHumedad boletaHumedad)
        {
            this.Boleta = boleta;
            this.BoletaId = boleta.BoletaId;
            this.BoletaHumedad = boletaHumedad;
            this.BoletaHumedadId = boletaHumedad.BoletaHumedadId;
        }

        protected BoletaHumedadAsignacion() { }

        public int BoletaHumedadAsignacionId { get; set; }
        public int BoletaHumedadId { get; set; }
        public int BoletaId { get; set; }
        public virtual Boletas Boleta { get; set; }
        public virtual BoletaHumedad BoletaHumedad { get; set; }

        public bool IsHumidityPaid(Boletas boletaPayment)
        {
            if (BoletaHumedad == null) return false;

            return BoletaHumedad.BoletaHumedadPago != null && BoletaHumedad.BoletaHumedadPago.BoletaId != boletaPayment.BoletaId;
        }
    }
}
