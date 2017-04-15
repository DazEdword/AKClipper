using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ParserWebHelper Helper;

        public HomeController(ParserWebHelper helper) {
            Helper = helper;
        }

        //http://localhost:60362/?parsed=true
        //this already works when called manually from browser

        [Route("parser")]
        [Route("/{parsed:bool?}")]
        [Route("")]

        public ViewResult Index(bool parsed) {
            ViewData["Title"] = "Home";

            if (parsed == true) {
                Helper.parserClientContent.parsed = true;
            } else {
                Helper.parserClientContent.parsed = false;
            }

            return View("Index", Helper);
        }

        //public ViewResult Index() {
        //    return TestRoutes();
        //}

        public ViewResult ParseIndex() {
            ViewData["Title"] = "Home";
            Helper.parserClientContent.parsed = true;
            return View("Index", Helper);
        }

        public ViewResult TestRoutes() {
            Helper.routing.Controller = nameof(HomeController);
            Helper.routing.Action = nameof(TestRoutes);
   
            return View("Routes", Helper);
        }

        //TODO: Sth wrong here, not receiving content on every occasion. 
        public IActionResult TestAjax(string content) {
            if (content != null && content.Length > 0) {
                Helper.parserClientContent.content = content;
                return Ok();
            } else {
                return NotFound();
            }
        }

        //public async ViewResult Parse() {
        //    var result = await ViewComponent("ParserController");
        //    return View("Index", Helper);
        //}
    }

    //public ViewResult TestRoutes() => View("Routes", new RoutingHelper {
    //    Controller = nameof(HomeController),
    //    Action = nameof(TestRoutes)
    //    });
    //}

 

}