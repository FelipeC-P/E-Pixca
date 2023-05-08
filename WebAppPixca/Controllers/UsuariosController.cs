using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppPixca.Models;
using System.Security.Cryptography;
using MySqlConnector;
using System.Data;

namespace WebAppPixca.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly PixcaContext _context;
        static string cadena = "server=localhost;port=3306;database=pixca;uid=root;password=12345";
        int Number;
        public UsuariosController(PixcaContext context)
        {
            _context = context;
        }



        //Buscar producto
        [HttpPost]
        public async Task<IActionResult> BuscarPorNombre(string nombre)
        {
            var productos = await _context.Productos.Where(p => p.NombreProduct.Contains(nombre)).Include(p =>
            p.IdCategoriaNavigation).ToListAsync();
            return View("HomeUser", productos);
        }




        public async Task<IActionResult> HomeUser()
        {
            var pixcaContext = _context.Productos.Include(p => p.IdCategoriaNavigation).Include(p => 
            p.IdUsuarioNavigation);
            return View(await pixcaContext.ToListAsync());
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
            using (MySqlConnection cn = new MySqlConnection(cadena))
            {

                cn.Open();

                using (MySqlCommand cmd = new MySqlCommand("Comparar_Informacion", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //usuario.Contraseña = ConvertirSha256(usuario.Contraseña);
                    cmd.Parameters.AddWithValue("NumberPhone", usuario.NumeroTelefono);
                    cmd.Parameters.AddWithValue("Email2", usuario.Email);

                    

                    Number = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
                if ( Number == 0)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(usuario);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Login", "Home");
                    }
                    //return View(usuario);
                    return RedirectToAction("HomeUser", "Usuarios");
                }
                else
                {
                    TempData["Mensaje"] = "Datos ya existentes" + "\n" + "Intenta de nuevo";
                    return View();
                }
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

        //Actualizar a vendedor 
        public async Task<IActionResult> SerVendedor(int? id)
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

        //Edit post ser vendedor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SerVendedor(int id, string curp, string rfc)
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
            return RedirectToAction(nameof(SerVendedor));

        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }

        //Close session
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

        public async Task<IActionResult> DetailsProduct(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdProduct == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

    }
}
