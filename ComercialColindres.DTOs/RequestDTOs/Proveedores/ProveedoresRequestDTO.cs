using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.DTOs.RequestDTOs.Proveedores
{
    [Route("/proveedores/{proveedorId}", "GET")]
    public class GetProveedor : IReturn<ProveedoresDTO>
    {
        public int ProveedorId { get; set; }
    }

    [Route("/proveedores/buscar-por-valor", "GET")]
    public class GetByValorProveedores : BusquedaRequestBaseDTO, IReturn<BusquedaProveedoresDTO>
    {
    }

    [Route("/proveedores/busqueda/por-valor", "GET")]
    public class GetProveedoresPorValorBusqueda : IReturn<List<ProveedoresDTO>>
    {
        public string ValorBusqueda { get; set; }
    }

    [Route("/proveedores/", "PUT")]
    public class PutProveedor : IReturn<ActualizarResponseDTO>
    {
        public ProveedoresDTO Proveedor { get; set; }
    }

    [Route("/proveedores/", "POST")]
    public class PostProveedor : IReturn<ActualizarResponseDTO>
    {
        public ProveedoresDTO Proveedor { get; set; }
    }

    [Route("/proveedores/", "DELETE")]
    public class DeleteProveedor : IReturn<EliminarResponseDTO>
    {
        public int ProveedorId { get; set; }
    }    
}
