using ComercialColindres.Datos.Entorno.Context;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class BoletaDeduccionManual : Entity
    {
        public BoletaDeduccionManual(Boletas boleta, decimal monto, string motivoDeduccion)
        {
            Boleta = boleta;
            BoletaId = boleta.BoletaId;
            Monto = monto;
            MotivoDeduccion = motivoDeduccion;
        }

        public int DeduccionManualId { get; set; }
        public int BoletaId { get; set; }
        public decimal Monto { get; set; }
        public string MotivoDeduccion { get; set; }

        public virtual Boletas Boleta { get; set; }
    }
}
