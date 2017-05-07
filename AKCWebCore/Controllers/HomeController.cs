using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ParserWebHelper Helper;

        public HomeController(ParserWebHelper helper) {
            Helper = helper;
        }

        //http://localhost:60362/parser?Grid-Page=3&Grid-Rows=20&results=true works
        // if showResults is not reset, same problem with grid.

        //If using AjaxGrid, make sure to use it properly:
        //https://github.com/NonFactors/MVC6.Grid/issues/39


        [Route("")]
        [Route("/parse")]
        //[HttpGet("{id}")]
        //?Grid-Sort=DateAdded&Grid-Order=Asc
        public ViewResult Index() {
            ViewData["Title"] = "Home";
            return View("Index", Helper);
        }

        [HttpPost]
        public IActionResult Parse(string content, string language) {
            if (content != null && content.Length > 0) {
                Helper.parserClientContent.content = content;
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