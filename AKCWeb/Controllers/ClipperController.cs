using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {

    public class ClipperController : Controller {
        public ViewResult Index() {
            return View("Main");
        }
    }
}