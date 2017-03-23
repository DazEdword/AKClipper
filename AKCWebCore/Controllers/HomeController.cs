using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ViewResult Index() {
            ViewData["Title"] = "Home";
            return View("Index");
        }

        public ViewResult TestParse() {
            ViewData["Title"] = "Testing";
            return View("/Views/Shared/Components/Clipper/Results.cshtml");
        }

        //AJAX-friendly component refresh logic
        public IActionResult GetParserComponent(bool parsed = false) {
            return ViewComponent("ParserController", new {parsed = parsed});
        }
    }
}