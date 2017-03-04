using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using AKCCore;

namespace AKCWeb.Controllers {

    public class ClipperController : Controller {
        public ParserController parserController;

        public ViewResult Index() {
            parserController = this.ParserController();
            System.Diagnostics.Debug.WriteLine("ParserController instanced");
            return View("Main");
        }

        public ParserController ParserController() {
            return new ParserController();
        }

        //TODO Time to carry on with actual implementation, finally. 

    }
}