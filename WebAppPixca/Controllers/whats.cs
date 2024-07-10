using Microsoft.AspNetCore.Mvc;

    public class whats : Controller
    {
        public ActionResult RedirectToWhatsApp()
        {
            string phoneNumber = "2441246731"; // Reemplaza con tu número de WhatsApp
            string url = "https://api.whatsapp.com/send?phone=" + phoneNumber;
            return Redirect(url);
        }
    }
