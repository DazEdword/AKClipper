using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AKCWeb.Controllers {
    public class ClipperController : Controller {
        public ViewResult Index() {
            return View("Main");
        }
    }
}