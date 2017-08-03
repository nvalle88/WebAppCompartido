using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ds.zeus.entities.Modelos.Compartido;
using ds.zeus.services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class GenerosController : Controller
    {

        private readonly IGeneroServicio genroServicio;

        public GenerosController(IGeneroServicio genroServicio)
        {
            this.genroServicio = genroServicio;
        }

        // GET: Genero
        public IActionResult Index()
        {
            return View(genroServicio.ObtenerGeneros());
        }

        // GET: Genero/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genero/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genero Genero)
        {
            if (ModelState.IsValid)
            {
                var respuesta = genroServicio.Crear(Genero);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    Genero.Nombre = "";
                    return View(Genero);
                }


            }
            return View(Genero);
        }

        // GET: Genero/Edit/5
        public   IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Genero = genroServicio.ObtenerGenero(Convert.ToInt32(id));
            if (Genero == null)
            {
                return NotFound();
            }
            return View(Genero);
        }

        // POST: Genero/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public   IActionResult Edit(int id, Genero Genero)
        {
            if (id != Genero.IdGenero)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = genroServicio.Editar(Genero);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(Genero);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(Genero);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = genroServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", genroServicio.ObtenerGeneros());

        }
    }

}
