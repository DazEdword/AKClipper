using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AKCWebCore.Controllers {
    public class ClipperController : Controller {

        public IActionResult Index() {
            return ViewComponent("ParserController");
        }
    }
}
