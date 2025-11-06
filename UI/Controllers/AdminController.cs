using AccesoADatos.Data;
using AccesoADatos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool EsAdmin()
        {
            var rol = HttpContext.Session.GetString("Rol");
            return rol == "Administrador";
        }

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            if (!EsAdmin())
                return RedirectToAction(nameof(NoAutorizado));

            var solicitudes = await _context.Estudiantes
                .Include(e => e.Rol)
                .OrderBy(e => e.EstadoSolicitud)
                .ToListAsync();

            return View(solicitudes);
        }

        public IActionResult NoAutorizado()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
        {
            if (!EsAdmin())
                return RedirectToAction(nameof(NoAutorizado));

            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
                return NotFound();

            estudiante.EstadoSolicitud = nuevoEstado;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
