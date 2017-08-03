using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ds.zeus.services.Interfaces;
using ds.zeus.entities.Modelos.Compartido;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class InstitucionesFinancierasController : Controller
    {
        private readonly IInstitucionFinancieraServicio InstitucionFinancieraServicio;


        public InstitucionesFinancierasController(IInstitucionFinancieraServicio InstitucionFinancieraServicio)
        {
            this.InstitucionFinancieraServicio = InstitucionFinancieraServicio;
        }

        // GET: InstitucionesFinancieras
        public IActionResult Index()
        {
            return View(InstitucionFinancieraServicio.ObtenerInstitucionesFinancieras());
        }

        // GET: InstitucionesFinancieras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InstitucionesFinancieras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InstitucionFinanciera institucionFinanciera)
        {
            if (ModelState.IsValid)
            {
                var respuesta = InstitucionFinancieraServicio.Crear(institucionFinanciera);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;

                    return View(institucionFinanciera);
                }
            }
            return View(institucionFinanciera);
        }

        // GET: InstitucionesFinancieras/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instFinanciera = InstitucionFinancieraServicio.ObtenerInstitucionFinanciera(Convert.ToInt32(id));
            if (instFinanciera == null)
            {
                return NotFound();
            }
            return View(instFinanciera);
        }

        // POST: InstitucionesFinancieras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, InstitucionFinanciera institucionFinanciera)
        {
            if (id != institucionFinanciera.IdInstitucionFinanciera)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = InstitucionFinancieraServicio.Editar(id,institucionFinanciera);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(institucionFinanciera);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(institucionFinanciera);
        }

        // GET: InstitucionesFinancieras/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = InstitucionFinancieraServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", InstitucionFinancieraServicio.ObtenerInstitucionesFinancieras());
        }
    }
}
