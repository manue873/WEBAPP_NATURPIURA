using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Carrito
    {
        public Carrito()
        {
            Detalles = new HashSet<Detalle>();
        }

        public int IdCarrito { get; set; }
        public int IdUsuario { get; set; }
        public decimal Total { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
        public virtual ICollection<Detalle> Detalles { get; set; }
    }
}
