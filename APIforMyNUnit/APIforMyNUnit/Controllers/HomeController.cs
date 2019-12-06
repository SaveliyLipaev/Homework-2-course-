using Microsoft.AspNetCore.Mvc;

namespace APIforMyNUnit.Controllers
{
    /// <summary>
    /// Controller to display the start page
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult HomePage()
        {
            return View();
        }
    }
}