using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccesoADatos.Data;
using AccesoADatos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Controllers
{
    public class CuentaController : Controller
    {
        private readonly AppDbContext _context;

        public CuentaController(AppDbContext context)
        {
            _context = context;
        }

        
        private List<string> ObtenerCarreras() => new()
        {
            "Ing en Desarrollo de software",
            "Ing Industrial",
            "Sociologia",
            "Ing Ambiental",
            "Ing en Telematica"
        };

        

        // GET: /Cuenta/Registro
        [HttpGet]
        public IActionResult Registro()
        {
            ViewBag.Carreras = ObtenerCarreras();
            return View(new RegistroViewModel());
        }

        // POST: /Cuenta/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            ViewBag.Carreras = ObtenerCarreras();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validación: correo único
            var yaExiste = await _context.Estudiantes
                .AnyAsync(e => e.Correo == model.Correo);

            if (yaExiste)
            {
                ModelState.AddModelError("Correo", "Este correo ya está registrado.");
                return View(model);
            }

            // Mapear a entidad Estudiante
            var estudiante = new Estudiante
            {
                NombreCompleto = model.NombreCompleto,
                Correo = model.Correo,
                Telefono = model.Telefono,
                Carrera = model.Carrera,
                PasswordHash = model.Password, 
                RolId = 1,                      
                EstadoSolicitud = "Pendiente",
                FechaRegistro = DateTime.Now
            };

            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(RegistroExitoso));
        }

        // GET: /Cuenta/RegistroExitoso
        [HttpGet]
        public IActionResult RegistroExitoso()
        {
            return View();
        }

        

        // GET: /Cuenta/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: /Cuenta/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Estudiantes
                .Include(e => e.Rol)
                .FirstOrDefaultAsync(e =>
                    e.Correo == model.Correo &&
                    e.PasswordHash == model.Password);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View(model);
            }

            // Guardar datos en sesión
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("Nombre", usuario.NombreCompleto);
            HttpContext.Session.SetString("Rol", usuario.Rol.Nombre);

            return RedirectToAction("Index", "Home");
        }

     

        // GET: /Cuenta/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
