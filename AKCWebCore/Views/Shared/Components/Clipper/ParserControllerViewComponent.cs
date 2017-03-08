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
        //Parser controller dependency injection

        private ParserController parserController;
        private ParserWebHelper helper;

        public ParserControllerViewComponent() {
            parserController = new ParserController();
            helper = new ParserWebHelper();
        }

        //[HttpPost]
        //public IActionResult UploadFile(IFormFile file) {
        //    //long size = file.Length;

        //    // full path to file in temp location
        //    var filePath = Path.GetTempFileName();

        //    if (file.Length > 0) {
        //        using (var stream = new FileStream(filePath, FileMode.Create)) {
        //            //await file.CopyToAsync(stream);
        //        }
        //    }
        //    return Ok();
        //    //return Content("Miau");
        //}

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