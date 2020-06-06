using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FT_Retail.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FT_Retail.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private void licensed ()
        {
            var userClaims = new List<Claim>()
                {
                new Claim(ClaimTypes.Name, "Test"),
                new Claim(ClaimTypes.Role, "Licensed"),
                 };

            var identity = new ClaimsIdentity(
                  userClaims, CookieAuthenticationDefaults.
            AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties();
            props.RedirectUri = "Account/Test";

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.
                AuthenticationScheme,
                principal, props).Wait();


        }

        public IActionResult Index()
        {
            LicenseSoftwareController context = new FT_Retail.Controllers.LicenseSoftwareController();
            
            if (context.readLicense() != "")
            {
               licensed();
            }
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
