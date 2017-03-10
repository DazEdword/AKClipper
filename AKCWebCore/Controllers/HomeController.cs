using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ViewResult Index() {
            ViewData["Title"] = "Home";
            return View("Index");
        }

        public ActionResult Parse() {
            return Content("Test");
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
    }
}