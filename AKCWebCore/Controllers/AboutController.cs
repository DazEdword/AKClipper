using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {
    public class AboutController : Controller {

        [Route("/about")]
        public IActionResult Index() {
            ViewData["Title"] = "About";
            return View("About");
        }
    }
}
