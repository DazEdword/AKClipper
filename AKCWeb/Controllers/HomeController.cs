using Microsoft.AspNetCore.Mvc;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ViewResult Index() {
            return View("Index");
        }
        
        //Entry point. Action method or "action"
        /*
        public string Index() {
            return "Hello World";
        }
        */
    }
}

/*
//Entry point.
public IActionResult Index() {
    return View();
}

public string Index() {
    return "Hello world!";
}

public IActionResult About() {
    ViewData["Message"] = "Your application description page.";

    return View();
}

public IActionResult Contact() {
    ViewData["Message"] = "Your contact page.";

    return View();
}

public IActionResult Error() {
    return View();
} */