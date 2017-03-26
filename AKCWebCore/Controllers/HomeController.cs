using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ParserWebHelper Helper;

        public HomeController(ParserWebHelper helper) {
            Helper = helper;
        }

        [Route("parser/{parsed:bool?}")]
        [Route("/{parsed:bool?}")]
        [Route("")]
        
        //http://localhost:60362/?parsed=true
        //this already works when called manually from browser
        public ViewResult Index(bool parsed = false) {
            ViewData["Title"] = "Home";

            if (parsed == true) {
                Helper.parsed = true;
            } else {
                Helper.parsed = false;
            }

            return View("Index", Helper);
        }

        //AJAX-friendly component refresh logic
        //public IActionResult GetParserComponent(bool parsed = false) {
        //    return ViewComponent("ParserController", new { parsed = parsed });
        //}
    }
}