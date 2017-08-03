using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ds.zeus.entities.Modelos.Compartido;
using ds.zeus.services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class TiposSangresController : Controller
    {

        private readonly ITipoSangreServicio tipoSangreServicio;

        public TiposSangresController(ITipoSangreServicio tipoSangreServicio)
        {
            this.tipoSangreServicio = tipoSangreServicio;
        }

        // GET: TipoSangre
        public IActionResult Index()
        {
            return View(tipoSangreServicio.ObtenerTiposSangres());
        }

        // GET: TipoSangre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoSangre/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoSangre tipoSangre)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoSangreServicio.Crear(tipoSangre);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    tipoSangre.Nombre = "";
                    return View(tipoSangre);
                }


            }
            return View(tipoSangre);
        }

        // GET: TipoSangre/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSangre = tipoSangreServicio.ObtenerTipoSangre(Convert.ToInt32(id));
            if (tipoSangre == null)
            {
                return NotFound();
            }
            return View(tipoSangre);
        }

        // POST: TipoSangre/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TipoSangre tipoSangre)
        {
            if (id != tipoSangre.IdTipoSangre)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoSangreServicio.Editar(tipoSangre);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(tipoSangre);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(tipoSangre);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoSangreServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoSangreServicio.ObtenerTiposSangres());

        }
    }
}
