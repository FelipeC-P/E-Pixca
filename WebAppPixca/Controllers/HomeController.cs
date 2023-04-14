﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using System.Data;
using System.Diagnostics;
using WebAppPixca.Models;

namespace WebAppPixca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
            {
                return RedirectToAction("Details", "Usuarios");
            }
            else
            {
                TempData["Mensaje"] = "Sesión no iniciada";
                return RedirectToAction("Index", "Home");
            }
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        static string cadena = "server=localhost;port=3306;database=pixca;uid=root;password=12345";

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            using (MySqlConnection cn = new MySqlConnection(cadena))
            {

                cn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_ValidarUsuario", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("Email1", usuario.Email);
                    cmd.Parameters.AddWithValue("Contraseña1", usuario.Contraseña);

                    usuario.IdUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
                if (usuario.IdUsuario != 0)
                {
                    HttpContext.Session.SetString("IdUsuario", usuario.IdUsuario.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Mensaje"] = "Usuario no encontrado" + "\n" + "Intenta de nuevo";
                    return View();
                }
            }
        }
    }
}