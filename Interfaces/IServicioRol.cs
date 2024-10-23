using WEBAPP_NATURPIURA.Models1;

namespace WEBAPP_NATURPIURA.Interfaces
{
    public interface IServicioRol
    {
        bool IsUserInRole(string userId, string roleName);
    }
    public class RoleService : IServicioRol
    {
        private readonly NatupiuraContext _context;

        public RoleService(NatupiuraContext context)
        {
            _context = context;
        }

        public bool IsUserInRole(string userId, string roleName)
        {
            var userRole = _context.Usuarios
                .FirstOrDefault(ur => ur.Correo == userId && ur.IdRolNavigation.Descripcion == roleName);
            return userRole != null;
        }
    }
}
