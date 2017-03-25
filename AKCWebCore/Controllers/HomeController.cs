using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ViewResult Index() {
            ViewData["Title"] = "Home";
            return View("Index");
        }

        /* TODO Test we can call this view from Main.cshtml (Form, "Parse" button)*/

        public ViewResult TestParse() {
            ViewData["Title"] = "Testing";
            return View("/Views/Shared/Components/Clipper/Results.cshtml");
        }

        //AJAX-friendly component refresh logic
        public IActionResult GetParserComponent(bool parsed = false) {
            return ViewComponent("ParserController", new { parsed = parsed });
        }
    }
}