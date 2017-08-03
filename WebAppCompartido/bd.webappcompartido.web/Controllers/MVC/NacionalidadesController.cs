using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ds.zeus.entities.Modelos.Compartido;
using ds.zeus.services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class NacionalidadesController : Controller
    {

        private readonly INacionalidadServicio nacionalidadServicio;

        public NacionalidadesController(INacionalidadServicio nacionalidadServicio)
        {
            this.nacionalidadServicio = nacionalidadServicio;
        }

        // GET: Nacionalidad
        public IActionResult Index()
        {
            return View(nacionalidadServicio.ObtenerNacionalidades());
        }

        // GET: Nacionalidad/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nacionalidad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Nacionalidad nacionalidad)
        {
            if (ModelState.IsValid)
            {
                var respuesta = nacionalidadServicio.Crear(nacionalidad);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    nacionalidad.Nombre = "";
                    return View(nacionalidad);
                }


            }
            return View(nacionalidad);
        }

        // GET: Nacionalidad/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nacionalidad = nacionalidadServicio.ObtenerNacionalidad(Convert.ToInt32(id));
            if (nacionalidad == null)
            {
                return NotFound();
            }
            return View(nacionalidad);
        }

        // POST: Nacionalidad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Nacionalidad nacionalidad)
        {
            if (id != nacionalidad.IdNacionalidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = nacionalidadServicio.Editar(nacionalidad);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(nacionalidad);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(nacionalidad);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = nacionalidadServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", nacionalidadServicio.ObtenerNacionalidades());

        }
    }
}
