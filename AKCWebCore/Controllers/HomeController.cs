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
    }
}