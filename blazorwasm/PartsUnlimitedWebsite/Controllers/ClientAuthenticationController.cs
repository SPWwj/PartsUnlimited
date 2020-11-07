using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartsUnlimitedWebsite.Controllers
{
    public class ClientAuthenticationController : Controller
    {
        [HttpGet("authentication/login-callback")]
        public IActionResult LoginCallback() => View();

        [HttpGet("authentication/login-failed")]
        public IActionResult LoginFailed() => View();
    }
}
