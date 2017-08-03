using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ds.zeus.services.Interfaces;
using ds.zeus.entities.Modelos.Compartido;
using ds.core.services.Utils;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class ProvinciasController : Controller
    {
        IProvinciaServicio provinciaServicio;
        IPaisServicio paisServicio;

        public ProvinciasController(IProvinciaServicio provinciaServicio, IPaisServicio paisServicio)
        {
            this.provinciaServicio = provinciaServicio;
            this.paisServicio = paisServicio;
        }

        // GET: Provinces
        public IActionResult Index()
        {
            return View(provinciaServicio.ObtenerProvincias());
        }

        // GET: Provinces/Details/5
        public IActionResult Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
             var response = new Response();// provinciaServicio.CambiarSiActivo(true, Convert.ToInt32(id));
            if (response.IsSuccess)
            {
                return RedirectToAction("Index","Provinces");
            }
            return View();
        }

        // GET: Provinces/Create
        public IActionResult Create()
        {
            ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre");
            return View();
        }

        // POST: Provinces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Provincia Provincia)
        {

            if (ModelState.IsValid)
            {
                var response = provinciaServicio.Crear(Provincia);

                if (response.IsSuccess)
                {

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre");
                return View(Provincia);
            }
           // ViewData["IdPais"] = new SelectList(CiudadServicio.ObtenerCiudadades(true), "IdCiudad", "Name");

            ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre");
            return View(Provincia);
        }

        // GET: Provinces/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Provincia = provinciaServicio.ObtenerProvincia(Convert.ToInt32(id));
            ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre");
            if (Provincia == null)
            {
                return NotFound();
            }
            return View(Provincia);
        }

        // POST: Provinces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Provincia Provincia)
        {
            if (id != Provincia.IdProvincia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = provinciaServicio.Editar(Provincia);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre");
                        return View(Provincia);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre");
            return View(Provincia);
        }

        // GET: Provinces/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = provinciaServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            ViewData["Error"] = respuesta.Message;
            return View("Index", provinciaServicio.ObtenerProvincias());

        }
    }  
}
