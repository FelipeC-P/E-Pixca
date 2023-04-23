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
            var pixcaContext = _context.Carritos.Include(c => c.IdProductNavigation).Include(c => 
            c.IdUsuarioNavigation).Where(c => c.IdUsuario == id);

            int cantidad = pixcaContext.Count();
            ViewBag.CantidadCarrito = cantidad;
            return View(await pixcaContext.ToListAsync());
        }

        // GET: Carritoes/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Carritoes/Create
        public IActionResult Create()
        {
            ViewData["IdProduct"] = new SelectList(_context.Productos, "IdProduct", "IdProduct");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: Carritoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCarrito,CantidadProductos,TotalPrecio,Fecha,IdProduct,IdUsuario")] Carrito carrito, int id)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrito);
                await _context.SaveChangesAsync();
                return RedirectToAction();
            }
            ViewData["IdProduct"] = new SelectList(_context.Productos, "IdProduct", "IdProduct", carrito.IdProduct);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", carrito.IdUsuario);
            return View(carrito);
        }

        // GET: Carritoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            ViewData["IdProduct"] = new SelectList(_context.Productos, "IdProduct", "IdProduct", carrito.IdProduct);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", carrito.IdUsuario);
            return View(carrito);
        }

        // POST: Carritoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCarrito,CantidadProductos,TotalPrecio,Fecha,IdProduct,IdUsuario")] Carrito carrito)
        {
            if (id != carrito.IdCarrito)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoExists(carrito.IdCarrito))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProduct"] = new SelectList(_context.Productos, "IdProduct", "IdProduct", carrito.IdProduct);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", carrito.IdUsuario);
            return View(carrito);
        }

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
                return Problem("Entity set 'PixcaContext.Carritos'  is null.");
            }
            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito != null)
            {
                _context.Carritos.Remove(carrito);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarritoExists(int id)
        {
            return (_context.Carritos?.Any(e => e.IdCarrito == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> AddCarrito(int id, int cantidad)
        {
            var producto = _context.Productos.Find(id);
            Carrito carrito = new Carrito();
            carrito.IdProduct = producto.IdProduct;
            carrito.IdUsuario = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));
            carrito.CantidadProductos = cantidad;
            carrito.TotalPrecio = producto.Precio;
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
