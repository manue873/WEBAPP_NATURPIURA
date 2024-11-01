﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WEBAPP_NATURPIURA.Models1;
using WEBAPP_NATURPIURA.Servicios;
using WEBAPP_NATURPIURA.utils;
using WEBAPP_NATURPIURA.ViewModels;


namespace WEBAPP_NATURPIURA.Controllers
{
    /// <summary>
    /// Controlador para los servicios de la cuenta del usuario
    /// </summary>
    public class AccountController : Controller
    {



        static int smsotp = 0;
        static Usuario usuarioGlobal = new Usuario();
        static Direccion direccionGlobal = new Direccion();
        static String cor = "";

         
       
        /// <summary>
        /// Campo de solo lectura que almacena el contexto de la base de datos.
        /// </summary>
        private readonly NatupiuraContext bd;
      
        /// <summary>
        /// Constructor de la clase <see cref="AccountController"/>. Inicializa una nueva instancia del controlador con un contexto de base de datos.
        /// </summary>
        /// <param name="context">Instancia del contexto de base de datos <see cref="NatupiuraContext"/> que se utilizará para acceder a la base de datos.</param>
        public AccountController(NatupiuraContext context)
        {

            bd = context;
        }
        /// <summary>
        /// Encriptacion de la clave mediante AES, la Key y la Iv se almacenan dentro de la misma clave
        /// </summary>
        /// <param name="plainText">recibe la clave ingresada en el formulario de inicio de session en formato String</param>
        /// <returns>Retorna la clave encriptada que se almacenara </returns>
        public static byte[] Encrypt(string plainText)
        {
            //Usa la Libreria de AES Cifrar
            using (Aes aesAlg = Aes.Create())
            {
                // Generar una key y un vector de inicialización (IV) para el cifrado
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();
                // Crear el transformador de cifrado utilizando la key y el IV generados
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Crear un MemoryStream para almacenar los datos cifrados
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Escribir la key y el IV en el stream antes de los datos cifrados
                    msEncrypt.Write(aesAlg.Key, 0, aesAlg.Key.Length);
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    // Crear un CryptoStream para realizar el cifrado en modo de escritura
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Escribir el texto plano en el CryptoStream, que se cifra automáticamente
                        swEncrypt.Write(plainText);
                    }
                    // Devolver el arreglo de bytes que contiene la Key, el IV y los datos cifrados
                    return msEncrypt.ToArray();
                }
            }
        }
        /// <summary>
        ///  Desencriptacion de la clave generada por el metodo <see cref="Encrypt(string)"/> 
        /// </summary>
        /// <param name="cipherText">texto cifrado</param>
        /// <returns>Retorna la clave decifrada</returns>
        public static string Decrypt(byte[] cipherText)
        {
            //creanos un MemoryStream para almacenar la clave que vamos a trabajar
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                // Creamos los arreglos para almacenar la clave (Key) y el vector de inicialización (IV).
                byte[] key = new byte[32];
                byte[] iv = new byte[16];
                // Leemos la clave y el IV desde el inicio del arreglo de bytes.
                msDecrypt.Read(key, 0, key.Length);
                msDecrypt.Read(iv, 0, iv.Length);

                //usamos AES desencriptar 
                using (Aes aesAlg = Aes.Create())
                {
                    //le indicamos cual es la key y la iv y asignamos
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    // Instanciamos en una varianle y le enviamos la key y la iv para la desencriptacion
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Desencriptamos los datos utilizando un CryptoStream.
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Retornamos el texto desencriptado en formato de String 
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReturnUrl">Captura la url para luego mostrarla en el form del login para se redireccione al lugar donde iba a ir luego del loggin</param>
        /// <returns></returns>
        public IActionResult Login(string? ReturnUrl = null)
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = ReturnUrl;
            ViewData["mensaje"] = "";
            return View();
        }
        /// <summary>
        /// Verificación de credenciales del usuario y registro de los archivos en el navegador (cookies).
        /// </summary>
        /// <param name="usuario">Objeto del modelo UsuarioLogin usado para el Login.</param>
        /// <param name="returnUrl">URL actual desde la que se desea el login, se usa para seguir el flujo a donde iba el usuario.</param>
        /// <returns>Si las credenciales son correctas, redirige al home y almacena las cookies en el navegador; caso contrario, se sigue en el bucle.</returns>
        [HttpPost]
        public IActionResult Login(UsuarioLogin usuario, string returnUrl = null)
        {
            try
            {
                Usuario _usuario = new Usuario();

                // Encriptar la clave ingresada por el usuario
                var claveEncriptadaVista = Encriptacion.Encrypt(usuario.Clave);

                // Validar las credenciales del usuario
                if (_usuario.ValidarCredencialesDeIngresoAsync(usuario.Correo, claveEncriptadaVista).Result == true)
                {
                    // Crear la identidad y el principal del usuario
                    var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, usuario.Correo)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    // Registrar la cookie de autenticación
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Redirigir al URL de retorno a donde iba, de lo contrario redirigir al home
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Mensaje de error si las credenciales son incorrectas
                    ViewData["mensaje"] = "Credenciales incorrectas. Intenta nuevamente.";
                }
            }
            catch (Exception e)
            {
                // Mensaje de error en caso de excepción
                ViewData["mensaje"] = "Ocurrió un error inesperado. Por favor, intenta nuevamente. " + e.Message;
            }

            // Mensaje de error genérico
            ViewData["Mensaje"] = "Credenciales Incorrectas";
            return View(usuario);
        }



        /// <summary>
        /// Cierra la session desde los claims 
        /// </summary>
        /// <returns>Redirecciona al Homepage </returns>
        public IActionResult Logout()
        {
            // quita las credenciales de los cookies
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registrar()
        {
            
            return View();
        }

        /// <summary>
        /// Registra un nuevo usuario y su dirección en la base de datos.
        /// </summary>
        /// <param name="model">Objeto del modelo UsuarioRegistro que contiene los datos del usuario y su dirección.</param>
        /// <returns>Redirige al home si el registro es exitoso; en caso contrario, muestra un mensaje de error.</returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarAsync(UsuarioRegistro model)
        {
            Usuario _usuario = new Usuario();

            try
            {
                // Verifica si el modelo es válido
                if (ModelState.IsValid)
                {
                    // Verifica si el correo ya está registrado
                    var usuarioExistente = await _usuario.GetUsuarioByCorreoAsync(model.Usuario.Correo);
                    if (usuarioExistente != null)
                    {
                        ViewData["mensaje"] = "El correo ya está registrado.";
                        return View(model);
                    }

                    var UsuarioVista = model.Usuario;
                    var direccion = model.Direccion;

                    // Crea un nuevo objeto Usuario con los datos del modelo
                    Usuario usuario = new Usuario
                    {
                        Nombres = model.Usuario.Nombres,
                        Apellidos = model.Usuario.Apellidos,
                        Correo = model.Usuario.Correo,
                        NumeroDocumento = model.Usuario.NumeroDocumento,
                        Telefono = model.Usuario.Telefono,
                        TipoDocumento = model.Usuario.TipoDocumento,
                        IdRol = 1,
                        Clave = Encriptacion.Encrypt(model.Usuario.Clave),
                        FechaRegistro = DateTime.Now,
                        Activo = true
                    };

                    // Agrega el usuario a la base de datos
                    bd.Usuarios.Add(usuario);
                    bd.SaveChanges();

                    // Asigna el IdUsuario a la dirección
                    direccion.IdUsuario = usuario.IdUsuario;
                    direccion.FechaRegistro = DateTime.Now;
                    direccion.Activo = true;

                    // Agrega la dirección a la base de datos
                    bd.Direccions.Add(direccion);
                    bd.SaveChanges();

                    // Redirige al home si el registro es exitoso
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                // Maneja cualquier excepción y muestra un mensaje de error
                ViewData["mensaje"] = "Ocurrió un error inesperado. Por favor, intenta nuevamente. " + e.Message;
            }

            // Si el modelo no es válido o ocurre un error, retorna la vista con el modelo
            return View(model);
        }


        public async Task<IActionResult> VerificarOTP()
        {
            ViewBag.Telefono = usuarioGlobal.Telefono;
            return View();

        }
        //prueba
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerificarOTP(string otpCode)
        {
            if (otpCode == smsotp.ToString())
            {


                try
                {

                    Usuario usuario = new Usuario();

                    usuario.Nombres = usuarioGlobal.Nombres;
                    usuario.Apellidos = usuarioGlobal.Apellidos;
                    usuario.NumeroDocumento = usuarioGlobal.NumeroDocumento;
                    usuario.TipoDocumento = usuarioGlobal.TipoDocumento;
                    usuario.Clave = usuarioGlobal.Clave;
                    usuario.Telefono = usuarioGlobal.Telefono;
                    usuario.IdRol = usuarioGlobal.IdRol;
                    usuario.Correo = usuarioGlobal.Correo;


                    bd.Add(usuario);
                    await bd.SaveChangesAsync();
                    var user = bd.Usuarios.OrderByDescending(x => x.FechaRegistro).FirstOrDefault(x => x.Correo == usuarioGlobal.Correo);
                    Direccion direccion = new Direccion();
                    direccion.Provincia = direccionGlobal.Provincia;
                    direccion.Distrito = direccionGlobal.Distrito;
                    direccion.IdUsuario = user.IdUsuario;
                    direccion.Calle = direccionGlobal.Calle;
                    direccion.Activo = true;
                    bd.Add(direccion);
                    await bd.SaveChangesAsync();
                }

                catch (Exception e)
                {
                    return View(e);
                }





                Carrito carrito = new Carrito();
                var usuario1 = bd.Usuarios.OrderByDescending(x => x.FechaRegistro).FirstOrDefault(x => x.Correo == usuarioGlobal.Correo);

                carrito.IdUsuario = usuario1.IdUsuario;



                bd.Add(carrito);
                await bd.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ViewBag.ErrorMessage = "El Codigo es Incorrecto";
                return View();
            }
        }



        private bool UsuarioExists(object idusuario)
        {
            throw new NotImplementedException();
        }
    }
}