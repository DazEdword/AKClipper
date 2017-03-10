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
        private ParserController parserController;
        private ParserWebHelper helper;

        public ParserControllerViewComponent() {
            parserController = new ParserController();
            helper = new ParserWebHelper();
        }

        //ViewComponent

        //Sync
        //Only one active at any given time. 
        public IViewComponentResult Invoke() {
            return View("~/Views/Shared/Components/Clipper/Main.cshtml", new {model = helper}.ToExpando());
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
    }
}