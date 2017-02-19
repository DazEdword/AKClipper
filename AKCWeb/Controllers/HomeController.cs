using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ViewResult Index() {
            return View("Index");
        }
    }
}