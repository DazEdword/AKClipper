using AKCCore;
using AKCWebCore.Extensions;
using AKCWebCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AKCWebCore.ViewComponents {

    public class ParserControllerViewComponent : ViewComponent {
        private ParserController ParserController;
        private ParserWebHelper Helper;

        //Parser controller dependency injection.
        public ParserControllerViewComponent(ParserController parserController,
            ParserWebHelper helper) {
            ParserController = parserController;
            Helper = helper;
        }

        //ViewComponent Sync/Async methods- Only one active at any given time.

        //Sync
        public IViewComponentResult Invoke(dynamic parse_params) {
            //Parser setup
            string content = Helper.parserClientContent.content = parse_params?.content;
            string language = Helper.parserClientContent.language = parse_params?.language;

            bool parse = true ? (content != null && language != null) : false;

            //TODO in any case, this logic if very shaky. We should perhaps add a "reparse" flag to reset parse
            //and parse again only upon "parse", do some other logic on grid interaction. 

            //Active parse or reparse
            if (parse) {
                return InvokeResults();
            //There is parse data already stored, we might be receiving query strings to reorder grid.
            } else if (Helper.parserClientContent.clippingData.Count > 0) {
                return InvokeMain();
            //No parse action call or prior data, invoke main so that the user can pick file, etc. 
            } else {
                return InvokeMain();
            }
        }

        //Async
        //public async Task<IViewComponentResult> InvokeAsync(bool loadPreview) {
        //    if (loadPreview == true) {
        //        var webHelper = new AKCWebCore.Models.helper();
        //        var preview = await webHelper.GetPreview();
        //        return View("~/Views/Clipper/Main.cshtml", new {Preview = preview });
        //    } else {
        //        return View("~/Views/Clipper/Main.cshtml");
        //    }
        //}

        public IViewComponentResult InvokeMain() {
            return View("~/Views/Shared/Components/Clipper/Main.cshtml", new { model = Helper }.ToExpando());
        }

        public IViewComponentResult InvokeResults() {
            ResetParser();
            Parse();

            return View("~/Views/Shared/Components/Clipper/Results.cshtml", new { model = Helper }.ToExpando());
        }

        //[HttpGet]
        //public ActionResult ResultsGrid(String param) {
        //    // Only grid string query values will be visible here.
        //    return PartialView("~/Views/Shared/Components/Clipper/Results.cshtml", new { model = Helper }.ToExpando());
        //}

        //This would be better off if we returned the collection. For WPF too I guess.
        public void Parse() {
            //Simpler version compared to WPF, not so many "safety checks". Can add said checks, but simpler. 
            string content = Helper.parserClientContent.content;
            string preview = ParserController.GeneratePreviewFromContent(content);
            string language = ParserController.options.Language = Helper.parserClientContent.language;
            string textSample = preview.Replace("\r", "").Split('\n')[1];

            bool correctParser = ParserController.ConfirmParserCompatibility(textSample, preview, true);

            if (correctParser != true) {
                //What if language mismatching?
            }

            ParserController.RunParserDirect(content);
            Helper.parserClientContent.clippingData = ClippingStorage.finalClippingsList;
        }

        public void ResetParser() {
            //TODO changing results to false can cause problems manipulating the grid (goes to index again)
            //Either we solve this with proper parameters or make the grid AJAX to avoid reloads. 
            Helper.parserClientContent.showResults = false;
            ClippingStorage.ClearStorage();
        }
    }
}