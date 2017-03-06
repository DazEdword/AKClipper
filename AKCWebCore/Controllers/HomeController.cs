using AKCCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        private ParserController _parserController;

        public HomeController(ParserController parserController) {
            _parserController = parserController;
        }

        public ViewResult Index() {
            ViewData["Title"] = "Home";
            return View("Index");
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file){
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
    }
}