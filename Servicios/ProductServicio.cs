using Microsoft.EntityFrameworkCore;
using WEBAPP_NATURPIURA.Models1;
using WEBAPP_NATURPIURA.ViewModels;

namespace WEBAPP_NATURPIURA.Servicios
{
    public class ProductServicio
    {
        private readonly NatupiuraContext bd;
        public ProductServicio(NatupiuraContext context)
        {
            bd = context;
        }

        /// <summary>
        /// Actualiza o crea productos y sus beneficios asociados en la base de datos.
        /// </summary>
        /// <param name="modeloIngreso">Modelo que contiene los datos del producto a registrar o actualizar.</param>
        /// <param name="idUsuario">ID del usuario que realiza la operación.</param>
        public async Task ActualizarYCrearProducto(ProductoRegistro modeloIngreso, int idUsuario)
        {
            try
            {
                var producto = MapearProducto(modeloIngreso);
                var kardex = CrearKardex(modeloIngreso, idUsuario, producto);

                if (await ProductoExiste(modeloIngreso.IdProducto))
                {
                    await ActualizarProducto(producto);
                    await ActualizarKardex(kardex);
                    await ActualizarBeneficios(modeloIngreso);
                }
                else
                {
                    await CrearProducto(producto);
                    await CrearBeneficios(modeloIngreso, producto.IdProducto);
                    await CrearKardex(kardex);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Mapea los datos del modelo de ingreso a una entidad Producto.
        /// </summary>
        /// <param name="modeloIngreso">Modelo que contiene los datos del producto a registrar o actualizar.</param>
        /// <returns>Entidad Producto con los datos mapeados.</returns>
        private Producto MapearProducto(ProductoRegistro modeloIngreso)
        {
            return new Producto
            {
                IdProducto = modeloIngreso.IdProducto,
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
        }

        /// <summary>
        /// Crea una entidad Kardex con los datos del modelo de ingreso.
        /// </summary>
        /// <param name="modeloIngreso">Modelo que contiene los datos del producto a registrar o actualizar.</param>
        /// <param name="idUsuario">ID del usuario que realiza la operación.</param>
        /// <param name="producto">Entidad Producto asociada al Kardex.</param>
        /// <returns>Entidad Kardex con los datos mapeados.</returns>
        private Kardex CrearKardex(ProductoRegistro modeloIngreso, int idUsuario, Producto producto)
        {
            return new Kardex
            {
                IdProducto = modeloIngreso.IdProducto,
                FechaMovimiento = modeloIngreso.FechaRegistro,
                TipoMovimiento = "Ingreso",
                Cantidad = modeloIngreso.Stock,
                CostoUnitario = modeloIngreso.PrecioUnidadCompra,
                TotalCostoMovimiento = modeloIngreso.Stock * modeloIngreso.PrecioUnidadCompra,
                SaldoCantidad = modeloIngreso.Stock,
                DocumentoReferencia = modeloIngreso.DocumentoReferencia,
                Observaciones = modeloIngreso.Observaciones,
                SaldoCosto = modeloIngreso.Stock * modeloIngreso.PrecioUnidadCompra,
                IdUsuario = idUsuario,
                IdProductoNavigation = producto
            };
        }

        /// <summary>
        /// Verifica si un producto existe en la base de datos.
        /// </summary>
        /// <param name="idProducto">ID del producto a verificar.</param>
        /// <returns>True si el producto existe, de lo contrario False.</returns>
        private async Task<bool> ProductoExiste(int idProducto)
        {
            if (bd == null)
            {
                throw new InvalidOperationException("El contexto de base de datos no está inicializado.");
            }

            return await bd.Productos.AnyAsync(x => x.IdProducto == idProducto);
        }

        /// <summary>
        /// Actualiza los datos de un producto en la base de datos.
        /// </summary>
        /// <param name="producto">Entidad Producto con los datos a actualizar.</param>
        private async Task ActualizarProducto(Producto producto)
        {
            bd.Productos.Update(producto);
            await bd.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza los datos de un Kardex en la base de datos.
        /// </summary>
        /// <param name="kardex">Entidad Kardex con los datos a actualizar.</param>
        private async Task ActualizarKardex(Kardex kardex)
        {
            bd.Kardices.Update(kardex);
            await bd.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza los beneficios asociados a un producto en la base de datos.
        /// </summary>
        /// <param name="modeloIngreso">Modelo que contiene los datos del producto y sus beneficios a actualizar.</param>
        private async Task ActualizarBeneficios(ProductoRegistro modeloIngreso)
        {
            var listaBeneficiosAnteriores = await bd.ProductoBeneficios.Where(x => x.IdProducto == modeloIngreso.IdProducto).ToListAsync();
            bd.ProductoBeneficios.RemoveRange(listaBeneficiosAnteriores);
            await bd.SaveChangesAsync();

            var listaBeneficios = modeloIngreso.ListaBeneficios.Select(beneficioid => new ProductoBeneficio
            {
                IdProducto = modeloIngreso.IdProducto,
                IdBeneficio = beneficioid
            }).ToList();

            bd.ProductoBeneficios.AddRange(listaBeneficios);
            await bd.SaveChangesAsync();
        }

        /// <summary>
        /// Crea un nuevo producto en la base de datos.
        /// </summary>
        /// <param name="producto">Entidad Producto con los datos a registrar.</param>
        private async Task CrearProducto(Producto producto)
        {
            bd.Productos.Add(producto);
            await bd.SaveChangesAsync();
        }

        /// <summary>
        /// Crea los beneficios asociados a un producto en la base de datos.
        /// </summary>
        /// <param name="modeloIngreso">Modelo que contiene los datos del producto y sus beneficios a registrar.</param>
        /// <param name="idProducto">ID del producto al que se asociarán los beneficios.</param>
        private async Task CrearBeneficios(ProductoRegistro modeloIngreso, int idProducto)
        {
            var listaBeneficios = modeloIngreso.ListaBeneficios.Select(beneficioid => new ProductoBeneficio
            {
                IdProducto = idProducto,
                IdBeneficio = beneficioid
            }).ToList();

            bd.ProductoBeneficios.AddRange(listaBeneficios);
            await bd.SaveChangesAsync();
        }

        /// <summary>
        /// Crea un nuevo Kardex en la base de datos.
        /// </summary>
        /// <param name="kardex">Entidad Kardex con los datos a registrar.</param>
        private async Task CrearKardex(Kardex kardex)
        {
            await bd.Kardices.AddAsync(kardex);
            await bd.SaveChangesAsync();
        }

    }
}
