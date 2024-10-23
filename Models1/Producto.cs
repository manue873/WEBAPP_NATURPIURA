using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Producto
    {
        public Producto()
        {
            CalificacionProductos = new HashSet<CalificacionProducto>();
            Detalles = new HashSet<Detalle>();
            Kardices = new HashSet<Kardex>();
            ProductoBeneficios = new HashSet<ProductoBeneficio>();
        }

        public int IdProducto { get; set; }
        public string? ImagenUrl { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int? IdCategoria { get; set; }
        public decimal PrecioUnidadCompra { get; set; }
        public decimal PrecioUnidadVenta { get; set; }
        public long? Stock { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Categorium? IdCategoriaNavigation { get; set; }
        public virtual ICollection<CalificacionProducto> CalificacionProductos { get; set; }
        public virtual ICollection<Detalle> Detalles { get; set; }
        public virtual ICollection<Kardex> Kardices { get; set; }
        public virtual ICollection<ProductoBeneficio> ProductoBeneficios { get; set; }
    
        
    }
}
