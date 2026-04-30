using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualValentina.Data;
using TiendaVirtualValentina.Helpers;
using TiendaVirtualValentina.Models;

namespace TiendaVirtualValentina.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly TiendaContext _context;

        public UsuarioController(TiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Aplicar hash solo una vez
                usuario.Clave = HashHelper.ObtenerHash(usuario.Clave);

                // Normalizar correo
                usuario.Correo = usuario.Correo.Trim().ToLower();

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var usuario = _context.Usuarios.Find(id);
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Si la contraseña fue cambiada, aplicar hash
                var usuarioOriginal = _context.Usuarios.AsNoTracking()
                                      .FirstOrDefault(u => u.Id == usuario.Id);
                if (usuario.Clave != usuarioOriginal.Clave)
                {
                    usuario.Clave = HashHelper.ObtenerHash(usuario.Clave);
                }

                usuario.Correo = usuario.Correo.Trim().ToLower();

                _context.Usuarios.Update(usuario);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        public IActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}