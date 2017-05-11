using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;
using System;
using AKCWebCore.Extensions;

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
                return ViewComponent("ParserController", new {
                    content = content,
                    language = language,
                });
            } else {
                //TODO Not found added just temporarily, return useful error message for user. 
                return NotFound();
            }  
        }
    }
}