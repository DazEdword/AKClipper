using AKCCore;
using AKCWebCore.Extensions;
using AKCWebCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        public IViewComponentResult Invoke(bool parsed = false) {
            if (parsed) {
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
            return View("~/Views/Shared/Components/Clipper/Main.cshtml", new { model = Helper }.ToExpando());
        }

        public IViewComponentResult InvokeResults() {
            return View("~/Views/Shared/Components/Clipper/Results.cshtml", new { model = Helper }.ToExpando());
        }

        public async Task<IActionResult> Parse() {
            const string sessionKey = "AKCContent";
            string content = Helper.content = HttpContext.Session.GetString(sessionKey);

            if (Helper.content != null && ParserController.options.Language != null) {
                bool correctParserConfirmed = ParserController.ConfirmParserCompatibility(Helper.textSample, Helper.preview);

                try {
                    if (correctParserConfirmed == false) {

                        //Potential errors
                        //MessageBoxResult parsingProblemMessageBox = MessageBox.Show("Potential language incompatibilities detected. Are you sure you want to continue? \r\n \r\n Click 'Cancel' to go back and select the correct language (RECOMMENDED) or 'OK' to continue (WARNING: program might became inestable or crash.)",
                        //    "Parsing problem?", System.Windows.MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
                        //if (parsingProblemMessageBox == MessageBoxResult.OK) {
                        //    correctParserConfirmed = true;
                        //}
                        return new BadRequestResult();

                    }
                } catch (Exception ex) {
                    //MessageBox.Show(ex.Message, "Parsing problem");
                    return new BadRequestResult();
                }

                if (correctParserConfirmed) {
                    //Async parsing

                    //TODO Loading gif here?
                    await Task.Run(() => ParserController.RunParserDirect(content));

                    //Result generation
                    dynamic result = ParserController.ReportParsingResult(false);

                    if (result != null) {
                        //MessageBox.Show(result.clippingCount + " clippings parsed.", "Parsing successful.");
                        //MessageBox.Show(result.databaseEntries.ToString() + " clippings added to database. " +
                        //    result.removedClippings.ToString() + " empty or null clippings removed.", "Database created.");
                    }

                    //If you want to update UI from this task a dispatcher has to be used, since it has to be in the UI thread.
                    //TODO Launch results component, update or whatever we are going to do.
                    //For now it'll be return success message.
                    return new OkObjectResult(new {
                        myContent = content
                    });
                }
            }

            if (ParserController.options.TextToParsePath == null) {
                //MessageBox.Show("No path to .txt found, please select your Kindle clipping file and try again.");
                return new BadRequestResult();
            }

            if (ParserController.options.Language == null) {
                //MessageBox.Show("Problems detecting language, please select your language and try again.");
                return new BadRequestResult();
            }

            return new BadRequestResult();
        }
    }
}