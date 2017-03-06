using Microsoft.AspNetCore.Mvc;

namespace AKCWebCore.ViewComponents {

    public class ParserControllerViewComponent : ViewComponent {
        //ViewComponent
        public IViewComponentResult Invoke() {
            return View("~/Views/Clipper/Main.cshtml");
        }
    }
}