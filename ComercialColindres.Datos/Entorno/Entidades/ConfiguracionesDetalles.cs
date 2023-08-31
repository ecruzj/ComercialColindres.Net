namespace ComercialColindres.Datos.Entorno.Entidades
{
    public partial class ConfiguracionesDetalles
    {
        public int ConfiguracionDetalleId { get; set; }
        public string CodigoConfiguracion { get; set; }
        public string Atributo { get; set; }
        public string Valor { get; set; }
        public bool EsRequerido { get; set; }
        public virtual Configuraciones Configuracion { get; set; }
    }
}
