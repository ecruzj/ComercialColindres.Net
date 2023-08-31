using ComercialColindres.DTOs.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.DTOs.RequestDTOs.Recibos
{
    public class RecibosDTO : BaseDTO
    {
        public int ReciboId { get; set; }
        public int SucursalId { get; set; }
        public bool EsAnticipo { get; set; }
        public string AplicaA { get; set; }
        public int? PrestamoId { get; set; }
        public int? FacturaId { get; set; }
        public string NumeroRecibo { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
    }
}
