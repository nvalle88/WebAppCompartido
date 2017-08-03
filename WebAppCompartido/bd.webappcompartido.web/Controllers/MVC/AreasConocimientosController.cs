using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ds.zeus.data;
using ds.zeus.entities.Modelos.Compartido;
using ds.zeus.services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AreasConocimientosController : Controller
    {

        private readonly IAreaConocimientoServicio areaConocimientoServicio;

        public AreasConocimientosController(IAreaConocimientoServicio areaConocimientoServicio)
        {
            this.areaConocimientoServicio = areaConocimientoServicio;
        }

        // GET: AreaConocimiento
        public IActionResult Index()
        {
            return View(areaConocimientoServicio.ObtenerAreasConocimientos());
        }

        // GET: AreaConocimiento/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AreaConocimiento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AreaConocimiento AreaConocimiento)
        {
            if (ModelState.IsValid)
            {
                var respuesta = areaConocimientoServicio.Crear(AreaConocimiento);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    AreaConocimiento.Descripcion = "";
                    return View(AreaConocimiento);
                }


            }
            return View(AreaConocimiento);
        }

        // GET: AreaConocimiento/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var AreaConocimiento = areaConocimientoServicio.ObtenerAreaConocimiento(Convert.ToInt32(id));
            if (AreaConocimiento == null)
            {
                return NotFound();
            }
            return View(AreaConocimiento);
        }

        // POST: AreaConocimiento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AreaConocimiento AreaConocimiento)
        {
            if (id != AreaConocimiento.IdAreaConocimiento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = areaConocimientoServicio.Editar(AreaConocimiento);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(AreaConocimiento);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(AreaConocimiento);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = areaConocimientoServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", areaConocimientoServicio.ObtenerAreasConocimientos());

        }
    }
}
