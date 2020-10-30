using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartsUnlimitedWebsite.Controllers
{
    public class B2CPolicies : Controller
    {
        [EnableCors]
        [HttpGet("/Account/Login")]
        public IActionResult SignUpSignIn()
        {
            return View();
        }

        [EnableCors]
        [HttpGet("/Account/Profile")]
        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
