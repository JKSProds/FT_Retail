using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FT_Retail.Models;

namespace FT_Retail.Controllers
{
    public class ArtigosController : Controller
    {
        // GET: Artigos
        public ActionResult Index()
        {
            FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;

            return View(context.ObterArtigos());
        }

        // GET: Artigos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Artigos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artigos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Artigos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Artigos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Artigos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Artigos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}