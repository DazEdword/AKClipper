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
       
        public ViewResult Index(bool parsed = false) {
            ViewData["Title"] = "Home";

            if (parsed == true) {
                Helper.parsed = true;
            } else {
                Helper.parsed = false;
            }

            return View("Index", Helper);
        }

        //public async ViewResult Parse() {
        //    var result = await ViewComponent("ParserController");
        //    return View("Index", Helper);
        //}
    }
}