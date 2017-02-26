using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {

    public class ClipperController : Controller {
        public ViewResult Index() {
            return View("Main");
        }

        public ActionResult ReadFile(string path, int lines = 39) {
            return null;
        }
    }
}