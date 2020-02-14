﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FT_Retail.Models;
using X.PagedList;

namespace FT_Retail.Controllers
{
    public class ArtigosController : Controller
    {

        // GET: Artigos
        public ActionResult Index(int? page, string PLU, string Nome)
        {
            ViewData["CurrentFilter"] = PLU;
            ViewData["CurrentFilter2"] = Nome;

            FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;
            int pageSize = 100;
            var pageNumber = page ?? 1;

            if (PLU == null) { PLU = "";}
            if (Nome == null) { Nome = "";}

            var listArtigos = context.ObterArtigosWhere(PLU, Nome).ToPagedList(pageNumber, pageSize);

            //if (!String.IsNullOrEmpty(PLU) || !String.IsNullOrEmpty(Nome))
            //{
            //     listArtigos = context.ObterArtigosWhere(PLU, Nome).ToPagedList(pageNumber, pageSize);
            //}
            

            //switch (sortOrder)
            //{
            //    case "nome_desc":
            //        listArtigos = context.ObterArtigosOrderBy("nome", false).ToPagedList(pageNumber, pageSize);
            //        break;
            //    case "nome":
            //        listArtigos = context.ObterArtigosOrderBy("nome", true).ToPagedList(pageNumber, pageSize);
            //        break;
            //    case "plu_desc":
            //        listArtigos = context.ObterArtigosOrderBy("plu", false).ToPagedList(pageNumber, pageSize);
            //        break;
            //    default:
            //        listArtigos = context.ObterArtigosOrderBy("plu", true).ToPagedList(pageNumber, pageSize);
            //        break;
            //}

            return View(listArtigos);
        }

        // GET: Artigos/Edit/5
        public ActionResult Edit(int id)
        {
            FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;

            var artigo = context.ObterArtigo(id);
            //artigo.Promocao = context.ObterPromocao(id);
            return View(artigo);
        }

        // POST: Artigos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Artigo artigoNovo)
        {
            try
            {
                FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;
                context.atualizarArtigo(artigoNovo);
                //return RedirectToAction(nameof(Index));

                var artigo = context.ObterArtigo(id);
                return View(artigo);
            }
            catch (Exception)
            {
                return View();
        }
    }
    
}
}