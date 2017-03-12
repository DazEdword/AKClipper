using AKCCore;
using AKCWebCore.Models;
using AKCWebCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AKCWebCore.ViewComponents {

    public class ParserControllerViewComponent : ViewComponent {

        //TODO Parser controller dependency injection instead of direct instantiation.
        private ParserController ParserController;
        private ParserWebHelper Helper;

        public ParserControllerViewComponent(ParserController parserController,
            ParserWebHelper helper) {

            ParserController = parserController;
            Helper = helper;
        }

        //ViewComponent

        //Sync
        //Only one active at any given time. 
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
    }
}