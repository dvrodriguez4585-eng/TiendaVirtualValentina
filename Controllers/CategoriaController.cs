using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualValentina.Data;
using TiendaVirtualValentina.Models;

namespace TiendaVirtualValentina.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly TiendaContext _context;

        public CategoriaController(TiendaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var categoria = _context.Categorias
                .ToList();

            return View(categoria);
        }
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]

        public IActionResult Create(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var categoria = _context.Categorias.Find(id);

            return View(categoria);
        }

        [HttpPost]
        public IActionResult Edit(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            
            var categoria = _context.Categorias.Find(id);

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
