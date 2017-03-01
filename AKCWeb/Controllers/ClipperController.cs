using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using AKCCore;
using System;

namespace AKCWeb.Controllers {
    public class ClipperController : Controller {

        private ParserController parserController;

        public void ConfigureServices(IServiceCollection services) {
            services.AddTransient<ParserController>();
        }

        //public ClipperController() {
        //    parserController =  new ParserController();
        //    System.Diagnostics.Debug.WriteLine("hello hello");
        //}

        public ViewResult Index() {
            return View("Main");
        }

        /*TODO is this even remotely correct? Assuming we are going to need parser controller 
         * many other times, we'd better instantiate it with the ClipperController, on construction.
        */

        public ActionResult BrowseFile(string path, int lines = 39) {
            
            return Content(parserController.GeneratePreviewFromPath(path, lines));
        }

        public ParserController InitParserController() {
            return new ParserController();
        }
    }
}