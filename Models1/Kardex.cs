using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Kardex
    {
        public int IdKardex { get; set; }
        public int IdProducto { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; } = null!;
        public decimal? Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal? TotalCostoMovimiento { get; set; }
        public decimal? SaldoCantidad { get; set; }
        public decimal? SaldoCosto { get; set; }
        public string DocumentoReferencia { get; set; } = null!;
        public string? Observaciones { get; set; }
        public int IdUsuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistroSistema { get; set; }

        public virtual Producto IdProductoNavigation { get; set; } = null!;
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}
