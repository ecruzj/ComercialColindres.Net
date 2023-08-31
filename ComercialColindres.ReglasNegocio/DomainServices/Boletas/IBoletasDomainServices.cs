using ComercialColindres.Datos.Entorno.Entidades;
using System;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IBoletasDomainServices
    {
        string ActualizarPrecioVentaPorOrdenCompra(Boletas boleta, OrdenesCompraProducto ordenCompraProducto, bool aplicarPrecioManual, decimal precioVenta);
        bool TryEliminarBoleta(Boletas boleta, out string mensajeValidacion);
        bool TryCierreForzado(Boletas boleta, List<Deducciones> deductions, out string mensajeValidacion);
        bool TryOpenBoletaById(Boletas boleta, out string mensajeValidacion);
        bool CanAssignBonusProduct(Boletas boleta, List<BonificacionProducto> bonificacionesProducto, out string errorMessage);
        bool TryUpdateProperties(Boletas boleta, string codigoBoleta, string numeroEnvio, Proveedores vendor, string placaEquipo, string motorista, CategoriaProductos product, 
                                 ClientePlantas factory, decimal pesoEntrada, decimal pesoSalida, decimal cantidadPenalizada, decimal bonus, 
                                 decimal precioProductoCompra, DateTime fechaSalida, byte[] imagen, out string mensajeValidacion);
    }
}
