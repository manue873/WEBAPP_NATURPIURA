using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Detalle
    {
        public int IdDetalles { get; set; }
        public int IdCarrito { get; set; }
        public int? IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal Subtotal { get; set; }
        public bool? Activo { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Carrito IdCarritoNavigation { get; set; } = null!;
        public virtual Producto IdProductoNavigation { get; set; } = null!;
        public virtual Ventum? IdVentaNavigation { get; set; }
    }
}
