using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ParserWebHelper Helper;

        public HomeController(ParserWebHelper helper) {
            Helper = helper;
        }

        public ViewResult Index(bool parsed = false) {
            ViewData["Title"] = "Home";

            if (parsed == true) {
                Helper.parsed = true;
            }

            return View("Index", Helper);
        }

        /* TODO Test we can call this view from Main.cshtml (Form, "Parse" button)*/
        //public ViewResult TestParse() {
        //    ViewData["Title"] = "Testing";
        //    return View("/Views/Shared/Components/Clipper/Results.cshtml");
        //}

        //AJAX-friendly component refresh logic
        //public IActionResult GetParserComponent(bool parsed = false) {
        //    return ViewComponent("ParserController", new { parsed = parsed });
        //}
    }
}