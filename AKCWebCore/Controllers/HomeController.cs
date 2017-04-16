using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ParserWebHelper Helper;

        public HomeController(ParserWebHelper helper) {
            Helper = helper;
        }

        //http://localhost:60362/?results=true
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
        public IActionResult Parse(string content, string language) {
            if (content != null && content.Length > 0) {
                Helper.parserClientContent.content = content;
                //TODO this necessary at this point? I'd say it isn't
                Helper.parserClientContent.showResults = true;
                Helper.parserClientContent.language = language;
                return ViewComponent("ParserController");
            } else {
                Helper.parserClientContent.showResults = false;
                Helper.parserClientContent.content = "";
                //TODO Are we sure is a "not found" what we want to do?
                return NotFound();
            }  
        }
    }
}