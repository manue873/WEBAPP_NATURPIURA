using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Ventum
    {
        public Ventum()
        {
            Detalles = new HashSet<Detalle>();
            Envios = new HashSet<Envio>();
        }

        public int IdVenta { get; set; }
        public string Codigo { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string? UrlSustento { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEnvio { get; set; }
        public string? TipoDocumento { get; set; }
        public int CantidadProducto { get; set; }
        public decimal TotalCosto { get; set; }
        public int? IdPago { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Pago? IdPagoNavigation { get; set; }
        public virtual Usuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<Detalle> Detalles { get; set; }
        public virtual ICollection<Envio> Envios { get; set; }
    }
}
