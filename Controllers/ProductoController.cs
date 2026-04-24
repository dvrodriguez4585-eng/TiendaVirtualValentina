using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualValentina.Data;
using TiendaVirtualValentina.Models;    

namespace TiendaVirtualValentina.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context;

        public ProductoController(TiendaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .ToList();

            return View(productos);
        }
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Categorias = _context.Categorias.ToList();
            return View();
        }

        [HttpPost]

        public IActionResult Create(Producto producto)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var producto = _context.Productos.Find(id);
            ViewBag.Categorias = _context.Categorias.ToList();

            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            _context.Productos.Update(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if(rol != "Admin")
            {
                return RedirectToAction("Index");
            }

            var producto = _context.Productos.Find(id);

            _context.Productos.Remove(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
