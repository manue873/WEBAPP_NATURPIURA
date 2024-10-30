using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NATURPIURA.ViewModels;
using System.Globalization;
using System.Runtime.CompilerServices;
using WEBAPP_NATURPIURA.Models1;
using WEBAPP_NATURPIURA.Servicios;
using WEBAPP_NATURPIURA.ViewModels;

namespace WEBAPP_NATURPIURA.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly NatupiuraContext bd;
        public ProductosController(NatupiuraContext context)
        {
            bd = context;
        }

        //Servicios que deberian estar en otro paquete
        //------------------------------------------------------------------------------------------
        public async Task<ProductoRegistro> ObtenerProductoRegistroPorId(int id)
        {
            try
            {
                var productoActual = await bd.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);
                if (productoActual == null)
                {
                    return null; // Maneja el caso donde el producto no se encuentra
                }

                var beneficiosActuales = await bd.ProductoBeneficios
                    .Where(x => x.IdProducto == id)
                    .Select(x => x.IdBeneficio)
                    .ToListAsync();

                var kardexActual = await bd.Kardices.FirstOrDefaultAsync(x => x.IdProducto == id);

                var productoRegistro = new ProductoRegistro
                {
                    Nombre = productoActual.Nombre,
                    Activo = productoActual.Activo,
                    Descripcion = productoActual.Descripcion,
                    ImagenUrl = productoActual.ImagenUrl,
                    DocumentoReferencia = kardexActual?.DocumentoReferencia ?? "sin datos",
                    Observaciones = kardexActual?.Observaciones ?? "sin datos",
                    PrecioUnidadCompra = productoActual.PrecioUnidadCompra,
                    PrecioUnidadVenta = productoActual.PrecioUnidadVenta,
                    Stock = productoActual.Stock,
                    FechaRegistro = productoActual.FechaRegistro,
                    IdCategoria = productoActual.IdCategoria,
                    ListaBeneficios = beneficiosActuales,
                    IdProducto = id
                };

                return productoRegistro;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Ocurrió un error al intentar obtener el producto. Inténtalo de nuevo.");
                Console.WriteLine(e.Message);
                return new ProductoRegistro(); // Devuelve un objeto vacío en caso de error
            }
        }
        //--------------------------------------------------------------------

        public async Task<List<Categorium>> ListarCategorias()
        {
            return await (from s in bd.Categoria
                          orderby s.Descripcion ascending
                          select s).ToListAsync();
        }
        public async Task<List<Beneficio>> ListarBeneficios()
        {
            return await (from s in bd.Beneficios
                          orderby s.NombreBeneficio ascending
                          select s).ToListAsync();
        }
        
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewBag.Producto = 1;
            try
            {

                var BusquedaProductos = from s in bd.Productos.Include(u => u.IdCategoriaNavigation) select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    BusquedaProductos = BusquedaProductos.Where(s => s.IdProducto.ToString().Contains(searchString) || s.Nombre.Contains(searchString) || s.IdCategoriaNavigation.Descripcion.Contains(searchString));
                    var countpro = BusquedaProductos.Count();
                    if (countpro == 0)
                    {
                        ViewBag.Producto = 0;
                    }
                }
                return View(await BusquedaProductos.ToListAsync());
            }
            catch (Exception)
            {
                return View();
            }
        }
        public async Task<ActionResult> AgregarProducto()
        {
            ViewBag.listaCategorias = await ListarCategorias();

            ViewBag.listaBeneficios = await ListarBeneficios();
            return View();
        }        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarProducto(ProductoRegistro modeloIngreso)
        {
            ViewBag.listaCategorias = await ListarCategorias();

            ViewBag.listaBeneficios = await ListarBeneficios();
            try
            {
                var usuario = new Usuario();
                Usuario CurrentUser = await usuario.GetUsuarioByCorreoAsync(User.Identity.Name);
                ProductServicio productServicio = new ProductServicio(bd);
                await productServicio.ActualizarYCrearProducto(modeloIngreso,CurrentUser.IdUsuario);
                
             

                return RedirectToAction("Index", "Productos");
            }

            catch (Exception e)
            {
                ModelState.AddModelError("", "Ocurrió un error al intentar registrar el producto. Inténtalo de nuevo.");
                Console.WriteLine(e.Message);
            }

            return View(modeloIngreso);
        }

        

        // GET: ProductosController/Edit/5
        public async Task<ActionResult> ModificarProducto(int id)
        {
            ViewBag.listaCategorias = await ListarCategorias();

            ViewBag.listaBeneficios = await ListarBeneficios();

            var productoRegistro = await ObtenerProductoRegistroPorId(id);
            ViewBag.PrecioUnidadCompra = productoRegistro.PrecioUnidadCompra.ToString(CultureInfo.InvariantCulture);
            ViewBag.PrecioUnidadVenta = productoRegistro.PrecioUnidadVenta.ToString(CultureInfo.InvariantCulture);

            if (productoRegistro == null)
            {
                return NotFound();
            }
            return View(productoRegistro);
        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ModificarProducto( ProductoRegistro modeloIngreso)
        {
            ViewBag.listaCategorias = await ListarCategorias();
            ViewBag.listaBeneficios = await ListarBeneficios();
            try
            {
                var usuario = new Usuario();
                Usuario CurrentUser = await usuario.GetUsuarioByCorreoAsync(User.Identity.Name);
                ProductServicio productServicio = new ProductServicio(bd);
                await productServicio.ActualizarYCrearProducto(modeloIngreso, CurrentUser.IdUsuario);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController/Delete/5
       
    }
}
