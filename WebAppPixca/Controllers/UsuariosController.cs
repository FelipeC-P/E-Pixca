﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppPixca.Models;
using System.Security.Cryptography;

namespace WebAppPixca.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly PixcaContext _context;

        public UsuariosController(PixcaContext context)
        {
            _context = context;
        }

        public IActionResult HomeUser()
        {
            return View();
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
              return _context.Usuarios != null ? 
                          View(await _context.Usuarios.ToListAsync()) :
                          Problem("Entity set 'PixcaContext.Usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));

            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,NombreUsuario,ApellidoPater,ApellidoMater,NumeroTelefono,Curp,Rfc,Email,Contraseña")] Usuario usuario)
        {
            //usuario.Contraseña = ConvertirSha256(usuario.Contraseña);
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Home");
            }
            return View(usuario);
        }

        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        private string? HomeController(object login)
        {
            throw new NotImplementedException();
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,NombreUsuario,ApellidoPater,ApellidoMater,NumeroTelefono,Curp,Rfc,Email,Contraseña")] Usuario usuario)
        {
            id = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            }
            return View(usuario);
        }

        //update be seller
        public async Task<IActionResult> BeSeller(int? id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        //Edit post seller
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BeSeller(int id, string curp, string rfc)
        {
            id = Convert.ToInt32(HttpContext.Session.GetString("IdUsuario"));
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Curp = curp;
            usuario.Rfc = rfc;

            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Sus datos seran revisados";
            return RedirectToAction(nameof(BeSeller));

        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'PixcaContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }

        public IActionResult Productos()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                TempData["Mensaje"] = "Sesión no iniciada";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult CerrarSesion()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
            {
                HttpContext.Session.Remove("IdUsuario");
                TempData["Mensaje"] = "Sesión cerrada";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                TempData["Mensaje"] = "Sesión no iniciada";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
