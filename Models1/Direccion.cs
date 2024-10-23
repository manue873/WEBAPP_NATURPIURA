using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Direccion
    {
        public int IdDireccion { get; set; }
        public int? IdUsuario { get; set; }
        [Required(ErrorMessage = "La Provincia es obligatoria")]
        [StringLength(50, ErrorMessage = "La Provincia no puede tener más de 50 caracteres")]
        public string? Provincia { get; set; }

        [Required(ErrorMessage = "El Distrito es obligatorio")]
        [StringLength(50, ErrorMessage = "El Distrito no puede tener más de 50 caracteres")]
        public string? Distrito { get; set; }

        [Required(ErrorMessage = "La Calle es obligatoria")]
        [StringLength(100, ErrorMessage = "La Calle no puede tener más de 100 caracteres")]
        public string? Calle { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
