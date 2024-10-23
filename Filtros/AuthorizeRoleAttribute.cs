namespace WEBAPP_NATURPIURA.Filtros;
using Microsoft.AspNetCore.Mvc;

public class AuthorizeRoleAttribute : TypeFilterAttribute
{
    public AuthorizeRoleAttribute(string role) : base(typeof(AuthorizeRoleFilter))
    {
        Arguments = new object[] { role };
    }
}
