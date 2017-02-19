using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {
    public class AboutController : Controller {
        public IActionResult Index() {
            ViewData["Title"] = "About";
            return View("About");
        }
    }
}
