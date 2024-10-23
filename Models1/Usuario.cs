using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.Arm;
using WEBAPP_NATURPIURA.Controllers;
using WEBAPP_NATURPIURA.Servicios;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class Usuario
    {

        private NatupiuraContext _context = new NatupiuraContext();
        public Usuario()
        {

            CalificacionProductos = new HashSet<CalificacionProducto>();
            Carritos = new HashSet<Carrito>();
            Direccions = new HashSet<Direccion>();
            Kardices = new HashSet<Kardex>();
            Pagos = new HashSet<Pago>();
            Venta = new HashSet<Ventum>();
        }

        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public byte[] Clave { get; set; } = null!;
        public int? IdRol { get; set; }
        public bool? Activo { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? Telefono { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
        public virtual ICollection<CalificacionProducto> CalificacionProductos { get; set; }
        public virtual ICollection<Carrito> Carritos { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
        public virtual ICollection<Kardex> Kardices { get; set; }
        public virtual ICollection<Pago> Pagos { get; set; }
        public virtual ICollection<Ventum> Venta { get; set; }


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
