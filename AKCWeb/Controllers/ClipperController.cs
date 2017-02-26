using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {

    public class ClipperController : Controller {
        public ViewResult Index() {
            return View("Main");
        }

        /*TODO is this even remotely correct? Assuming we are going to need parser controller 
         * many other times, we'd better instantiate it with the ClipperController, on construction.
        */

        //public ActionResult BrowseFile(string path, int lines = 39) {
        //    ParserController parserController = InitParserController();
        //    return Content(parserController.GeneratePreviewFromPath(path, lines));
        //}

        //public ParserController InitParserController() {
        //    return new ParserController();
        //}
    }
}