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

        //ViewComponent
        //Sync/Async - Only one active at any given time.

        //Sync
        public IViewComponentResult Invoke() {
            //TODO prob we should pass results directly from AJAX to ViewComponent, in which case this results will disappear.
            bool results = Helper.parserClientContent.showResults;

            if (results) {
                return InvokeResults();
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
            //return View("~/Views/Shared/Routes.cshtml", new {model = Helper });
            return View("~/Views/Shared/Components/Clipper/Main.cshtml", new { model = Helper }.ToExpando());
        }

        public IViewComponentResult InvokeResults() {
            Parse();
            return View("~/Views/Shared/Components/Clipper/Results.cshtml", new { model = Helper }.ToExpando());
        }

        //This would be better off if we returned the collection. For WPF too I guess.
        public void Parse() {
            //Simpler version compared to WPF, not so many "safety checks". Can add said checks, but more simplified. 
            string content = Helper.parserClientContent.content;
            string preview = ParserController.GeneratePreviewFromContent(content);
            string language = ParserController.options.Language = Helper.parserClientContent.language;
            string textSample = preview.Replace("\r", "").Split('\n')[1];

            bool correctParser = ParserController.ConfirmParserCompatibility(textSample, preview, true);

            if (correctParser != true) {
                //What if language mismatching?
            }

            ParserController.RunParserDirect(content);
            Helper.parserClientContent.clippingData = ClippingDatabase.finalClippingsList;
        }
    }
}