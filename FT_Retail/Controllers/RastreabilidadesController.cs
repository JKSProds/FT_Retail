using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FT_Retail.Models;

namespace FT_Retail
{
    public class RastreabilidadesController : Controller
    {
        public IActionResult Index()
        {
            FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;

            return View(context.ObterListaRastreabilidades());
        }
    }
}