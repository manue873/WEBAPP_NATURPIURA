using System;
using System.Collections.Generic;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Beneficio
    {
        public Beneficio()
        {
            ProductoBeneficios = new HashSet<ProductoBeneficio>();
        }

        public int IdBeneficio { get; set; }
        public string NombreBeneficio { get; set; } = null!;
        public bool? Activo { get; set; }

        public virtual ICollection<ProductoBeneficio> ProductoBeneficios { get; set; }
    }
}
