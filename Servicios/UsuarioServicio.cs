using Microsoft.EntityFrameworkCore;
using WEBAPP_NATURPIURA.Models1;

namespace WEBAPP_NATURPIURA.Servicios
{
    public class UsuarioServicio
    {
        private readonly NatupiuraContext _context;

        public UsuarioServicio(NatupiuraContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuarioByCorreoAsync(string correo)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Correo.Equals(correo));
        }

        public async Task<bool> ValidarCredencialesDeIngresoAsync(string correo, byte[] clave)
        {
            try
            {
                var usuario_DB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Correo == correo);

                if (usuario_DB != null)
                {
                    string claveDesencriptadaControlador = Encriptacion.Decrypt(clave);
                    string claveDesencriptadaModelo = Encriptacion.Decrypt(usuario_DB.Clave);

                    return usuario_DB.Correo == correo && claveDesencriptadaControlador == claveDesencriptadaModelo;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return false;
            }
        }
    }
}
