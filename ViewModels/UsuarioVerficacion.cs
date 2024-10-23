using System.ComponentModel.DataAnnotations;

namespace WEBAPP_NATURPIURA.ViewModels
{
    public class UsuarioVerficacion
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "No tiene el formato de e-mail")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [RegularExpression("[a-zA-Z ]{2,20}", ErrorMessage = "Solo admite letras entre 2 y 20")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [RegularExpression("[a-zA-Z ]{2,20}", ErrorMessage = "Solo admite letras entre 2 y 20")]
        public string Apellidos { get; set; }


        public string TipoDocumento { get; set; }
        [Required(ErrorMessage = "El numero de documento es necesario")]
        public string NumeroDocumento { get; set; }

        [Required(ErrorMessage = "El teléfono es necesario")]
        [Range(1, 999999999, ErrorMessage = "El valor debe estar entre 1 y 999999999")]
        public string Telefono { get; set; }

    }
}