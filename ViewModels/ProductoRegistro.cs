using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;
using WEBAPP_NATURPIURA.Models1;

namespace WEBAPP_NATURPIURA.ViewModels
{
    public class ProductoRegistro:Producto
    {
        
        public ProductoBeneficio ProductoBeneficio { set; get; }
        public List<Int32> ListaBeneficios { set; get; }
        public string Observaciones { set; get; }
        public string DocumentoReferencia { get; set; }
        
        public ProductoRegistro()
        {
            
            ListaBeneficios = new List<Int32>();
            ProductoBeneficio = new ProductoBeneficio();
        }

    }
}
