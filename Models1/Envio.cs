using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Envio
    {
        public int IdEnvio { get; set; }
        public int IdVenta { get; set; }
        public int? IdDireccion { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string? EstadoEnvio { get; set; }
        public string? CodigoSeguimiento { get; set; }
        public bool? Activo { get; set; }

        public virtual Ventum IdVentaNavigation { get; set; } = null!;
    }
}
