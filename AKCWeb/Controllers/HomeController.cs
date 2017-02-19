using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ViewResult Index() {
            ViewData["Title"] = "Home";
            return View("Index");
        }
    }
}