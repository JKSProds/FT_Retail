﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FT_Retail.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FT_Retail.Controllers
{
    public class BalancasController : Controller
    {
        // GET: Balancas
        public ActionResult Index()
        {
            FT_RetailContext context = HttpContext.RequestServices.GetService(typeof(FT_Retail.Models.FT_RetailContext)) as FT_RetailContext;

            List<Balanca> LstBalancas = context.ObterBalancas();
            return View(LstBalancas);
        }

        // GET: Balancas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Balancas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Balancas/Create
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

        // GET: Balancas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Balancas/Edit/5
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

        // GET: Balancas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Balancas/Delete/5
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