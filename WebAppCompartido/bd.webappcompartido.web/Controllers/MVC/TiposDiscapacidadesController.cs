using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ds.zeus.entities.Modelos.Compartido;
using ds.zeus.services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class TiposDiscapacidadesController : Controller
    {

        private readonly ITipoDiscapacidadServicio tipoDiscapacidadServicio;

        public TiposDiscapacidadesController(ITipoDiscapacidadServicio tipoDiscapacidadServicio)
        {
            this.tipoDiscapacidadServicio = tipoDiscapacidadServicio;
        }

        // GET: TipoDiscapacidad
        public IActionResult Index()
        {
            return View(tipoDiscapacidadServicio.ObtenerTiposDiscapacidades());
        }

        // GET: TipoDiscapacidad/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDiscapacidad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoDiscapacidad TipoDiscapacidad)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoDiscapacidadServicio.Crear(TipoDiscapacidad);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    TipoDiscapacidad.Nombre = "";
                    return View(TipoDiscapacidad);
                }


            }
            return View(TipoDiscapacidad);
        }

        // GET: TipoDiscapacidad/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var TipoDiscapacidad = tipoDiscapacidadServicio.ObtenerTipoDiscapacidad(Convert.ToInt32(id));
            if (TipoDiscapacidad == null)
            {
                return NotFound();
            }
            return View(TipoDiscapacidad);
        }

        // POST: TipoDiscapacidad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TipoDiscapacidad TipoDiscapacidad)
        {
            if (id != TipoDiscapacidad.IdTipoDiscapacidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoDiscapacidadServicio.Editar(TipoDiscapacidad);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(TipoDiscapacidad);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(TipoDiscapacidad);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoDiscapacidadServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoDiscapacidadServicio.ObtenerTiposDiscapacidades());

        }
    }

}
