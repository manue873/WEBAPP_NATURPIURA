using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using WEBAPP_NATURPIURA.Interfaces;

using WEBAPP_NATURPIURA.Models1;

namespace WEBAPP_NATURPIURA.Controllers
{

    public class HomeController : Controller
    {
        private readonly IServicioRol _roleService;
        private readonly NatupiuraContext _context;

        public HomeController(IServicioRol roleService, NatupiuraContext context)
        {
            _context = context;
            _roleService = roleService;
        }
        public static string NormalizarString(string input)
        {
            return new string(input
                .Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .ToLower();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = User.Identity.Name;//                       Rol que se quiere bloquear
            if (userId != null && _roleService.IsUserInRole(userId, ""))
            {
                if (context.ActionDescriptor.RouteValues["action"] != "Restriccion")
                {
                    context.Result = RedirectToAction("Restriccion", "Home");
                }
            }

            base.OnActionExecuting(context);
        }

        
        public async Task<IActionResult> Index(string cadenaBusqueda)
        {
            
            ViewData["cadenaBusqueda"] = cadenaBusqueda;
            List<Producto> listaProductosCatalogo = new List<Producto>();
            if (cadenaBusqueda==null)
            {
                var fechaLimite = DateTime.Now.AddMonths(-1);
                 listaProductosCatalogo = await _context.Productos
                    .Where(p => p.FechaRegistro >= fechaLimite)
                    .ToListAsync();
            }
            else
            {
                cadenaBusqueda = NormalizarString(cadenaBusqueda);
                listaProductosCatalogo = await _context.Productos
        .Where(p => EF.Functions.Like(EF.Functions.Collate(p.Nombre, "SQL_Latin1_General_CP1_CI_AI").ToLower(), $"%{cadenaBusqueda}%") ||
                    EF.Functions.Like(EF.Functions.Collate(p.Descripcion, "SQL_Latin1_General_CP1_CI_AI").ToLower(), $"%{cadenaBusqueda}%") ||
                    EF.Functions.Like(EF.Functions.Collate(p.IdCategoriaNavigation.Descripcion, "SQL_Latin1_General_CP1_CI_AI").ToLower(), $"%{cadenaBusqueda}%") ||
                    p.ProductoBeneficios.Any(pb => EF.Functions.Like(EF.Functions.Collate(pb.IdBeneficioNavigation.NombreBeneficio, "SQL_Latin1_General_CP1_CI_AI").ToLower(), $"%{cadenaBusqueda}%")))
        .Include(p => p.ProductoBeneficios)
        .ThenInclude(pb => pb.IdBeneficioNavigation)
        .ToListAsync();
            }
            return View(listaProductosCatalogo);
        }
        
        public IActionResult Restriccion()
        {
            if ()
            {

            }
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
