using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class CalificacionProducto
    {
        public int IdCalificacion { get; set; }
        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public int Calificacion { get; set; }
        public string? Comentario { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCalificacion { get; set; }

        public virtual Producto IdProductoNavigation { get; set; } = null!;
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}
