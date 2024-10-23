using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Pago
    {
        public Pago()
        {
            Venta = new HashSet<Ventum>();
        }

        public int IdPago { get; set; }
        public string Codigo { get; set; } = null!;
        public string? Tarjeta { get; set; }
        public int? IdUsuario { get; set; }
        public string Metodo { get; set; } = null!;
        public bool? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<Ventum> Venta { get; set; }
    }
}
