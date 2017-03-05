using Microsoft.AspNetCore.Mvc;
using AKCCore;

namespace AKCWebCore.ViewComponents {

    public class ParserControllerViewComponent : ViewComponent {
        private ParserController _parserController;

        public ParserControllerViewComponent() {
            _parserController = new ParserController();
            System.Diagnostics.Debug.WriteLine("ParserController instanced");
        }

        public IViewComponentResult Invoke() {
            ParserController parserController = _parserController;
            return View("~/Views/Clipper/Main.cshtml");
        }
    }
}