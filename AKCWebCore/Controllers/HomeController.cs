using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;
using System;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ParserWebHelper Helper;

        public HomeController(ParserWebHelper helper) {
            Helper = helper;
        }

        [Route("/")]
        [Route("parser")]
        [Route("/{results:bool?}")]

        //http://localhost:60362/parser?Grid-Page=3&Grid-Rows=20&results=true works
        // if showResults is not reset, same problem with grid.

        //If using AjaxGrid, make sure to use it properly:
        //https://github.com/NonFactors/MVC6.Grid/issues/39

        public ViewResult Index() {
            ViewData["Title"] = "Home";
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