using System.ComponentModel.DataAnnotations;


namespace WEBAPP_NATURPIURA.ViewModels
{
    public class UsuarioLogin
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [RegularExpression("[a-zA-Z ]{2,20}", ErrorMessage = "Solo admite letras")]
        public string Nombres { get; set; } = null!;
        [Required(ErrorMessage = "El Apellido es obligatorio")]
        [RegularExpression("[a-zA-Z ]{2,20}", ErrorMessage = "Solo admite letras")]
        public string Apellidos { get; set; } = null!;
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "No tiene el formato de e-mail")]
        public string Correo { get; set; } = null!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La contraseña es necesaria")]
        public string Clave { get; set; } = null!;
        public int? IdRol { get; set; }
        public bool? Activo { get; set; }
        public string? TipoDocumento { get; set; }
        [Required(ErrorMessage = "El numero de documento es necesario")]
        public string? NumeroDocumento { get; set; }
        [Required(ErrorMessage = "El teléfono es necesario")]
        [Range(1, 999999999, ErrorMessage = "El valor debe estar entre 1 y 999999999")]
        public string? Telefono { get; set; }
        public DateTime? FechaRegistro { get; set; }
        

    }
}
