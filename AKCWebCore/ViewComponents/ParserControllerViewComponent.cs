using Microsoft.AspNetCore.Mvc;
using AKCCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace AKCWebCore.ViewComponents {

    public class ParserControllerViewComponent : ViewComponent {
        //Parser controller dependency injection

        private ParserController _parserController;

        public ParserControllerViewComponent(ParserController parserController) {
            _parserController = parserController;
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file) {
            //long size = file.Length;

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            if (file.Length > 0) {
                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    //await file.CopyToAsync(stream);
                }
            }
            return Ok();
            //return Content("Miau");
        }

        private IActionResult Ok() {
            throw new NotImplementedException();
        }

        //ViewComponent
        public IViewComponentResult Invoke() {
            return View("~/Views/Clipper/Main.cshtml");
        }
    }
}