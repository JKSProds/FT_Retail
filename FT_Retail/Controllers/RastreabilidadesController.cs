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

        public ActionResult Edit(int ID)
        {
            FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;
            ViewData["Nome_Rast"] = context.ObterRastreabilidade(ID).NomeRastreabilidade;
            return View(context.ObterListaLinhaRastreabilidades(ID));
        }

        [HttpPost]
        public ActionResult Edit(int ID, List<LinhaRastreabilidade> Linhas)
        {
            try
            {
                FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;
                
                context.atualizarRastreabilidade(Linhas, ID);
                

                
                ViewData["Nome_Rast"] = context.ObterRastreabilidade(ID).NomeRastreabilidade;
                return View(context.ObterListaLinhaRastreabilidades(ID));
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}