using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ds.zeus.entities.Modelos.Compartido;
using ds.zeus.services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class TiposIdentificacionesController : Controller
    {

        private readonly ITipoIdentificacionServicio tipoIdentificacionServicio;

        public TiposIdentificacionesController(ITipoIdentificacionServicio tipoIdentificacionServicio)
        {
            this.tipoIdentificacionServicio = tipoIdentificacionServicio;
        }

        // GET: TipoIdentificacion
        public IActionResult Index()
        {
            return View(tipoIdentificacionServicio.ObtenerTiposIdentificaciones());
        }

        // GET: TipoIdentificacion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoIdentificacion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoIdentificacion tipoIdentificacion)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoIdentificacionServicio.Crear(tipoIdentificacion);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    tipoIdentificacion.Nombre = "";
                    return View(tipoIdentificacion);
                }


            }
            return View(tipoIdentificacion);
        }

        // GET: TipoIdentificacion/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoIdentificacion = tipoIdentificacionServicio.ObtenerTipoIdentificacion(Convert.ToInt32(id));
            if (tipoIdentificacion == null)
            {
                return NotFound();
            }
            return View(tipoIdentificacion);
        }

        // POST: TipoIdentificacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TipoIdentificacion tipoIdentificacion)
        {
            if (id != tipoIdentificacion.IdTipoIdentificacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoIdentificacionServicio.Editar(tipoIdentificacion);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(tipoIdentificacion);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(tipoIdentificacion);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoIdentificacionServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoIdentificacionServicio.ObtenerTiposIdentificaciones());

        }
    }
}

