using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ParserWebHelper Helper;

        public HomeController(ParserWebHelper helper) {
            Helper = helper;
        }

        //http://localhost:60362/?showResults=true
        //this already works when called manually from browser

        [Route("parser")]
        [Route("/{results:bool?}")]
        [Route("")]

        public ViewResult Index(bool results = false) {
            ViewData["Title"] = "Home";

            Helper.parserClientContent.showResults = results;

            return View("Index", Helper);
        }

        [HttpPost]
        public IActionResult UpdateContent(string content, string language) {
            if (content != null && content.Length > 0) {
                Helper.parserClientContent.content = content;
                Helper.parserClientContent.showResults = true;
                Helper.parserClientContent.language = language;
                return Ok();
            } else {
                Helper.parserClientContent.showResults = false;
                Helper.parserClientContent.content = "";
                //TODO Are we sure is a "not found" what we want to do?
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Parse() {
            return ViewComponent("ParserController");
        }
        
        //[HttpPost]
        //public ViewResult Parse() {
        //    //TODO Carry on here
        //    Helper.parserClientContent.showResults = true;
        //    // return View("Index", Helper);
        //    return View("~/Views/Shared/Components/Clipper/Results.cshtml", Helper);
            

        //    //TestRoutes
        //    //Helper.routing.Controller = nameof(HomeController);
        //    //Helper.routing.Action = nameof(Parse);
        //    //return View("Routes", Helper);
        //}

        //public ViewResult TestRoutes() {
        //    Helper.routing.Controller = nameof(HomeController);
        //    Helper.routing.Action = nameof(TestRoutes);

        //    return View("Routes", Helper);
        //}
    }
}