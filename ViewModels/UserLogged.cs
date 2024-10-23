namespace WEBAPP_NATURPIURA.ViewModels
{


    public class UserLogged
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string smscode { get; set; }
        public string smscodeverify { get; set; }

        public bool isLogged
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Nombre);
            }
        }

    }
}
