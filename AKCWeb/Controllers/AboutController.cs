using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {
    public class AboutController : Controller {
        public IActionResult Index() {
            return View("About");
        }
    }
}
