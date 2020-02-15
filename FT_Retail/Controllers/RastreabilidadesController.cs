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

            return View(context.ObterListaLinhaRastreabilidades(ID));
        }

        [HttpPost]
        public ActionResult Edit(int id, Rastreabilidade rastreabilidade)
        {
            try
            {
                FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;
                
                context.atualizarRastreabilidade(rastreabilidade.LinhasRastreabilidade, id);
                //return RedirectToAction(nameof(Index));

                var artigo = context.ObterListaLinhaRastreabilidades(id);
                return View(artigo);
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}