using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class ProductoBeneficio
    {
        public int IdProducto { get; set; }
        public int IdBeneficio { get; set; }
        public bool? Activo { get; set; }

        public virtual Beneficio IdBeneficioNavigation { get; set; } = null!;
        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}
