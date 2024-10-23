using WEBAPP_NATURPIURA.Models1;

namespace WEBAPP_NATURPIURA.ViewModels
{
    public class ProductoRegistro:Producto
    {
        
        public ProductoBeneficio ProductoBeneficio { set; get; }
        public ProductoRegistro()
        {

            ProductoBeneficio = new ProductoBeneficio();
        }

    }
}
