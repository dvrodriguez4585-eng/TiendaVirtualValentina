using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualValentina.Data;
using TiendaVirtualValentina.Models;
using System.Text.Json;

namespace TiendaVirtualValentina.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context;

        public ProductoController(TiendaContext context)
        {
            _context = context;
        }

        // =========================
        // LISTAR PRODUCTOS
        // =========================
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            var productos = _context.Productos
                .Include(p => p.Categoria)
                .ToList();

            return View(productos);
        }

        // =========================
        // FORMULARIO CREAR
        // =========================
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Categorias = _context.Categorias.ToList();
            return View();
        }

        // =========================
        // CREAR PRODUCTO
        // =========================
        [HttpPost]
        public IActionResult Create(Producto producto, IFormFile imagen)
        {
            if (imagen != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var ruta = Path.Combine(carpeta, imagen.FileName);
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }

                producto.ImagenUrl = "/images/" + imagen.FileName;
            }

            _context.Productos.Add(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // =========================
        // FORMULARIO EDITAR
        // =========================
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            var producto = _context.Productos.Find(id);
            ViewBag.Categorias = _context.Categorias.ToList();
            return View(producto);
        }

        // =========================
        // ACTUALIZAR PRODUCTO
        // =========================
        [HttpPost]
        public IActionResult Edit(Producto producto, IFormFile imagen)
        {
            var productoBD = _context.Productos.Find(producto.Id);
            if (productoBD == null) return NotFound();

            productoBD.Nombre = producto.Nombre;
            productoBD.Precio = producto.Precio;
            productoBD.Stock = producto.Stock;
            productoBD.CategoriaId = producto.CategoriaId;

            if (imagen != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var ruta = Path.Combine(carpeta, imagen.FileName);
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }

                productoBD.ImagenUrl = "/images/" + imagen.FileName;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // =========================
        // ELIMINAR PRODUCTO
        // =========================
        public IActionResult Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador") return RedirectToAction("Index");

            var producto = _context.Productos.Find(id);
            _context.Productos.Remove(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // =========================
        // AGREGAR AL CARRITO
        // =========================
        [HttpPost]
        public IActionResult AgregarCarrito(int id, int cantidad)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null || producto.Stock == 0)
            {
                TempData["Error"] = "Producto sin existencias";
                return RedirectToAction("Index");
            }

            if (cantidad > producto.Stock)
            {
                TempData["Error"] = "No hay disponibles tantas unidades";
                return RedirectToAction("Index");
            }

            var carritoJson = HttpContext.Session.GetString("Carrito");
            List<CarritoItem> carrito = carritoJson == null
                ? new List<CarritoItem>()
                : JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);

            var item = carrito.FirstOrDefault(p => p.ProductoId == id);
            if (item != null)
            {
                if ((item.Cantidad + cantidad) > producto.Stock)
                {
                    TempData["Error"] = "No hay suficientes unidades disponibles";
                    return RedirectToAction("Index");
                }
                item.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem { ProductoId = id, Cantidad = cantidad });
            }

            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
            TempData["Mensaje"] = "Producto agregado al carrito";

            return RedirectToAction("Index");
        }

        // =========================
        // MOSTRAR CARRITO
        // =========================
        public IActionResult Carrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            List<CarritoItem> carrito = carritoJson == null
                ? new List<CarritoItem>()
                : JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);

            var productos = new List<(Producto producto, int cantidad)>();
            foreach (var item in carrito)
            {
                var producto = _context.Productos.Find(item.ProductoId);
                if (producto != null)
                    productos.Add((producto, item.Cantidad));
            }

            return View(productos);
        }

        // =========================
        // COMPRAR PRODUCTOS
        // =========================
        public IActionResult Comprar()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            if (carritoJson == null)
                return RedirectToAction("Index");

            var carrito = JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);

            foreach (var item in carrito)
            {
                var producto = _context.Productos.Find(item.ProductoId);
                if (producto != null && producto.Stock >= item.Cantidad)
                    producto.Stock -= item.Cantidad;
            }

            _context.SaveChanges();
            HttpContext.Session.Remove("Carrito");

            TempData["Mensaje"] = "Compra realizada con éxito";
            return RedirectToAction("Index");
        }
    }
}