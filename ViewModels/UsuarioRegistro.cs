using WEBAPP_NATURPIURA.Models1;
namespace WEBAPP_NATURPIURA.ViewModels
{
    public class UsuarioRegistro
    {
        public UsuarioLogin Usuario { get; set; }
        public Direccion Direccion { get; set; }
       
        public UsuarioRegistro()
        {
            Usuario = new UsuarioLogin();
            Direccion = new Direccion();
        }
    }
}
