using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace AKCWeb.Controllers {
    public class HomeController : Controller {

        //Entry point.
        public IActionResult Index() {
            return View();
        }

        /*
        public string Index() {
            return "Hello world!";
        } */


        public IActionResult About() {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact() {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error() {
            return View();
        }
    }
}
