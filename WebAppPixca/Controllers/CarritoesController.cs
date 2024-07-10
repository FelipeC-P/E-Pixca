using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppPixca.Models;

namespace WebAppPixca.Controllers
{
    public class CarritoesController : Controller
    {
        private readonly PixcaContext _context;

        public CarritoesController(PixcaContext context)
        {
            _context = context;
        }

        // GET: Carritoes
        public async Task<IActionResult> Index()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));
            var pixcaContext = _context.Carritos.Include(c => c.IdProductNavigation).Include(c => c.IdUsuarioNavigation).Where(c => c.IdUsuario == id);

            int cantidad = pixcaContext.Sum(c => c.CantidadProductos); // Suma la cantidad de productos en el carrito
            ViewBag.CantidadCarrito = cantidad;
            ViewBag.CarritoTotalPrecio = pixcaContext.Sum(c => c.TotalPrecio); // Calcula el precio total del carrito
            return View(await pixcaContext.ToListAsync());
        }

        // Resto del código del controlador...

        // GET: Carritoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.IdProductNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdCarrito == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // POST: Carritoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carritos == null)
            {
                return Problem("Entity set 'PixcaContext.Carritos' is null.");
            }

            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }

            _context.Carritos.Remove(carrito); // Elimina el carrito de la base de datos
            await _context.SaveChangesAsync();

            // Calcular y establecer el precio total del carrito después de eliminar un producto
            int userId = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));
            ViewBag.CarritoTotalPrecio = _context.Carritos.Where(c => c.IdUsuario == userId).Sum(c => c.TotalPrecio);

            return RedirectToAction(nameof(Index));
        }

        // Resto del código del controlador...

        // GET: Carritoes/AddCarrito/5
        public async Task<IActionResult> AddCarrito(int id, int cantidad)
        {
            var producto = _context.Productos.Find(id);
            Carrito carrito = new Carrito();
            carrito.IdProduct = producto.IdProduct;
            carrito.IdUsuario = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));
            carrito.CantidadProductos = cantidad;
            carrito.TotalPrecio = producto.Precio * cantidad; // Actualiza el total de precio según la cantidad de productos
            carrito.Fecha = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(carrito);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("DetailsProduct", "Usuarios", new { id = id });
        }
    }
}

