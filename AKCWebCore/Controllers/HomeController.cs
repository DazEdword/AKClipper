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

        public ViewResult Index() {
            ViewData["Title"] = "Home";
            return View("Index", Helper);
        }

        //[Route("/results")]
        //public ViewResult Results(ActionExecutingContext context) {
        //    //var path = context.HttpContext.Request.Path.Value.Trim().ToLower();
        //    ViewData["Title"] = "Results";
        //    return View("Index", Helper);
        //}

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