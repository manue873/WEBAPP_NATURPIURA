using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Rol
    {
        public Rol()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }
        public string Descripcion { get; set; } = null!;
        public string Permisos { get; set; } = null!;
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
