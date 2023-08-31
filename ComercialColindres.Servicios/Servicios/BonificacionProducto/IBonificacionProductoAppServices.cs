using ComercialColindres.DTOs.RequestDTOs.Bonificaciones;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBonificacionProductoAppServices
    {
        BonificacionProductoDto GetBonificacion(GetBonificacionProducto request);
    }
}
