using AKCCore;
using AKCWebCore.Extensions;
using AKCWebCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AKCWebCore.ViewComponents {

    public class ParserControllerViewComponent : ViewComponent {
        private ParserWebHelper Helper;
        private ParserController ParserController;

        //Parser controller dependency injection.
        public ParserControllerViewComponent(ParserWebHelper helper) {
            Helper = helper;
            ParserController = new ParserController();
        }

        //ViewComponent Sync/Async methods- Only one active at any given time.

        //Sync
        public IViewComponentResult Invoke(dynamic parse_params) {
            //Parser setup
            string content = Helper.content = parse_params?.content;
            string language = Helper.language = parse_params?.language;

            if (Helper.reset == true) {
                ResetParser();
            }

            bool parse = true ? (content != null && language != null) : false;

            //Active parse or reparse
            if (parse) {
                return InvokeNewResults();
            //There is parse data already stored, we might be receiving query strings to reorder grid.
            } else if (Helper.clippingData.Count > 0) {
                return InvokeResults();
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

        public IViewComponentResult InvokeNewResults() {
            ResetParser();
            Parse();

            return View("~/Views/Shared/Components/Clipper/Results.cshtml", new { model = Helper }.ToExpando());
        }

        public IViewComponentResult InvokeResults() {
            return View("~/Views/Shared/Components/Clipper/Results.cshtml", new { model = Helper }.ToExpando());
        }

        //This would be better off if we returned the collection. For WPF too I guess.
        public void Parse() {
            //Simpler version compared to WPF, not so many "safety checks". Can add said checks, but simpler. 
            string content = Helper.content;
            string preview = ParserController.GeneratePreviewFromContent(content);
            string language = ParserController.options.Language = Helper.language;
            string textSample = preview.Replace("\r", "").Split('\n')[1];

            bool correctParser = ParserController.ConfirmParserCompatibility(textSample, preview, true);

            if (correctParser != true) {
                //What if language mismatching?
            }

            ParserController.RunParserDirect(content);
            Helper.clippingData = ParserController.ClippingStorage.finalClippingsList;
            Helper.Save();
        }

        public void ResetParser() {
            ParserController.ClippingStorage.ClearStorage();
            Helper.Reset();
        }
    }
}