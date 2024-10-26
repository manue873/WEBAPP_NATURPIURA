using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NATURPIURA.ViewModels;
using WEBAPP_NATURPIURA.Models1;
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


        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductosController/Create
        public async Task<ActionResult> AgregarProducto()
        {
            ViewBag.listaCategorias = await (from s in bd.Categoria
                                             orderby s.Descripcion ascending
                                             select new { s.Descripcion, s.IdCategoria }).ToListAsync();

            ViewBag.listaBeneficios = await (from s in bd.Beneficios
                                             orderby s.NombreBeneficio ascending
                                             select new { s.NombreBeneficio, s.IdBeneficio }).ToListAsync();

            return View();
        }



        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarProducto(ProductoRegistro modeloIngreso)
        {
            ViewBag.listaCategorias = await (from s in bd.Categoria
                                             orderby s.Descripcion ascending
                                             select new { s.Descripcion, s.IdCategoria }).ToListAsync();

            ViewBag.listaBeneficios = await (from s in bd.Beneficios
                                             orderby s.NombreBeneficio ascending
                                             select new { s.NombreBeneficio, s.IdBeneficio }).ToListAsync();
            try
            {
                var productoRegistro = modeloIngreso;
                var usuario = new Usuario();
                Usuario CurrentUser = await usuario.GetUsuarioByCorreoAsync(User.Identity.Name);
                Producto producto = new Producto
                {
                    Nombre = modeloIngreso.Nombre,
                    Descripcion = modeloIngreso.Descripcion,
                    Stock = modeloIngreso.Stock,
                    Activo = modeloIngreso.Activo,
                    FechaRegistro = modeloIngreso.FechaRegistro,
                    PrecioUnidadCompra = modeloIngreso.PrecioUnidadCompra,
                    PrecioUnidadVenta = modeloIngreso.PrecioUnidadVenta,
                    ImagenUrl = modeloIngreso.ImagenUrl,
                    IdCategoria = modeloIngreso.IdCategoria
                };
                bd.Productos.Add(producto);
                bd.SaveChanges();

                for (int i = 0; i < modeloIngreso.ListaBeneficios.Count; i++)
                {
                    var beneficioid= modeloIngreso.ListaBeneficios[i];
                    var productoBeneficio= new ProductoBeneficio
                    {
                        IdProducto = producto.IdProducto,
                        IdBeneficio = beneficioid
                    };
                    bd.ProductoBeneficios.Add(productoBeneficio);
                }
                bd.SaveChanges();
                var kardex = new Kardex()
                {
                    IdProducto = modeloIngreso.IdProducto,
                    FechaMovimiento = modeloIngreso.FechaRegistro,
                    TipoMovimiento = "Ingreso",
                    Cantidad = modeloIngreso.Stock,
                    CostoUnitario = modeloIngreso.PrecioUnidadCompra,
                    TotalCostoMovimiento = modeloIngreso.Stock * modeloIngreso.PrecioUnidadCompra,
                    SaldoCantidad = modeloIngreso.Stock,
                    DocumentoReferencia = "000000000000",
                    Observaciones = "No hay observaciones",
                    SaldoCosto= modeloIngreso.Stock * modeloIngreso.PrecioUnidadCompra,
                    IdUsuario = CurrentUser.IdUsuario,
                    IdProductoNavigation=producto
                };

                 await bd.Kardices.AddAsync(kardex);
                await bd.SaveChangesAsync();
                
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
