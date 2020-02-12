using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FT_Retail.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FT_Retail.Controllers
{
    public class LojasController : Controller
    {
        // GET: Lojas
        public ActionResult Index()
        {
            FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;

            var loja = context.ObterLoja(1);
            return View(loja);

        }
    }
}