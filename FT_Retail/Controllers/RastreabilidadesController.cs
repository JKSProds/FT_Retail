using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FT_Retail.Models;
using Microsoft.AspNetCore.Authorization;

namespace FT_Retail
{
    [Authorize(Roles = "Licensed")]
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
                
                context.AtualizarRastreabilidade(Linhas, ID);

                ViewBag.Message = "Rastreabilidade atualizada e enviada com sucesso!";
                ViewBag.Color = "SUCESS";

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