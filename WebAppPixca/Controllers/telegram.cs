using Microsoft.AspNetCore.Mvc;


public class telegram : Controller
{
    public ActionResult RedirectToTelegram()
    {
        string username = "Pixcabot"; // Reemplaza con tu nombre de usuario de Telegram
        string url = "https://t.me/" + username;
        return Redirect(url);
    }
}
