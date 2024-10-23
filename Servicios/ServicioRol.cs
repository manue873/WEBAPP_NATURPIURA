using Microsoft.EntityFrameworkCore;

using WEBAPP_NATURPIURA.Models1;

namespace WEBAPP_NATURPIURA.Servicios
{
    public class ServicioRol
    {
        private readonly NatupiuraContext _context;

        public ServicioRol(NatupiuraContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            var userRole = await _context.Usuarios
                .FirstOrDefaultAsync(ur => ur.Correo == userId && ur.IdRolNavigation.Descripcion == roleName);
            return userRole != null;
        }
    }
}
