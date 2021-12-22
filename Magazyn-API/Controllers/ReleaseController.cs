using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Controllers
{
    public class ReleaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
