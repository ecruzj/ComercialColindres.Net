using ComercialColindres.Datos.Entorno.Context;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Entidades
{
    public class SubPlanta
    {
        public SubPlanta(int plantaId, string nombreSubPlanta, string rtn, string direccion, bool isExonerado, string registroExoneracion)
        {
            PlantaId = plantaId;
            NombreSubPlanta = nombreSubPlanta;
            Rtn = rtn;
            Direccion = direccion;
            EsExonerado = isExonerado;
            RegistroExoneracion = registroExoneracion;

            this.Facturas = new List<Factura>();
        }

        protected SubPlanta() { }

        public int SubPlantaId { get; set; }
        public int PlantaId { get; set; }
        public string NombreSubPlanta { get; set; }
        public string Rtn { get; set; }
        public string Direccion { get; set; }
        public bool EsExonerado { get; set; }
        public string RegistroExoneracion { get; set; }

        public virtual ClientePlantas Planta { get; set; }
        public virtual ICollection<Factura> Facturas { get; set; }
    }
}
