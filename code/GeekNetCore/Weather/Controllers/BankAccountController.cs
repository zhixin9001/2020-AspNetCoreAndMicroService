using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Controllers
{
    [ApiController]
    [Route("web/[controller]/[action]")]
    public class BankAccountController: ControllerBase
    {
        [Authorize]
        public IActionResult Cookie()
        {
            return Content("bank account");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Jwt()
        {
            return Content(User.FindFirst("Name").Value);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult AnyOne()
        {
            return Content(User.FindFirst("Name").Value);
        }
    }
}
