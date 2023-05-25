using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using System.Data;
using System.Diagnostics;
using WebAppPixca.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

namespace WebAppPixca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static string cadena = @"Server=MYSQL5045.site4now.net;Database=db_a99a85_epixcan;Uid=a99a85_epixcan;Pwd=Tt7Xfhb@HL_iKB*";

        public HomeController(ILogger<HomeController> logger, PixcaContext context)
        {
            _logger = logger;
            _context = context;
        }

        private readonly PixcaContext _context;

        public async Task<IActionResult> Index()
        {
            var pixcaContext = _context.Productos.Include(p => p.IdCategoriaNavigation).Include(p => 
            p.IdUsuarioNavigation);
            return View(await pixcaContext.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            using (MySqlConnection cn = new MySqlConnection(cadena))
            {

                cn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_ValidarUsuario", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //usuario.Contraseña = ConvertirSha256(usuario.Contraseña);
                    cmd.Parameters.AddWithValue("Email1", usuario.Email);
                    cmd.Parameters.AddWithValue("Contraseña1", usuario.Contraseña);

                    usuario.IdUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
                if (usuario.IdUsuario != 0)
                {
                    HttpContext.Session.SetString("IdUsuario", usuario.IdUsuario.ToString());
                    return RedirectToAction("HomeUser", "Usuarios");
                }
                else
                {
                    TempData["Mensaje"] = "Usuario no encontrado" + "\n" + "Intenta de nuevo";
                    return View();
                }
            }
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

        public IActionResult Carrito()
        {
            TempData["Mensaje"] = "Inicia sesión primero";
            return View("Login");
        }

        public IActionResult Compra()
        {
            TempData["Mensaje"] = "Inicia sesión primero";
            return View("Login");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // buscar por el nombre del producto en la pagina inicial
        [HttpPost]
        public async Task<IActionResult> BuscarPorNombre(string nombre)
        {
            var productos = await _context.Productos.Where(p => p.NombreProduct.Contains(nombre)).Include(p =>
            p.IdCategoriaNavigation).ToListAsync();
            return View("Index", productos);
        }

        public async Task<IActionResult> DetailsProductHome(int? id)
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