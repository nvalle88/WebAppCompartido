using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ds.zeus.entities.Modelos.Compartido;
using Microsoft.AspNetCore.Authorization;
using ds.zeus.services.Interfaces;
using ds.core.web.shared.Utils;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class ModelosController : Controller
    {
        IModeloServicio modeloServicio;
        IMarcaServicio marcaServicio;

        public ModelosController(IModeloServicio modeloServicio, IMarcaServicio marcaServicio)
        {
            this.modeloServicio = modeloServicio;
            this.marcaServicio = marcaServicio;
        }

        // GET: Modelos
        public IActionResult Index()
        {
            return View(modeloServicio.ObtenerModelos());
        }

        // GET: Modelos/Details/5
        public IActionResult Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = new Response();// modeloServicio.CambiarSiActivo(true, Convert.ToInt32(id));
            if (response.IsSuccess)
            {
                return RedirectToAction("Index", "Modelos");
            }
            return View();
        }

        // GET: Modelos/Create
        public IActionResult Create()
        {
            ViewData["IdMarca"] = new SelectList(marcaServicio.ObtenerMarcas(), "IdMarca", "Nombre");
            return View();
        }

        // POST: Modelos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Modelo Modelo)
        {

            if (ModelState.IsValid)
            {
                var response = modeloServicio.Crear(Modelo);

                if (response.IsSuccess)
                {

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdMarca"] = new SelectList(marcaServicio.ObtenerMarcas(), "IdMarca", "Nombre");
                return View(Modelo);
            }
            // ViewData["IdMarca"] = new SelectList(CiudadServicio.ObtenerCiudadades(true), "IdCiudad", "Name");

            ViewData["IdMarca"] = new SelectList(marcaServicio.ObtenerMarcas(), "IdMarca", "Nombre");
            return View(Modelo);
        }

        // GET: Modelos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Modelo = modeloServicio.ObtenerModelo(Convert.ToInt32(id));
            ViewData["IdMarca"] = new SelectList(marcaServicio.ObtenerMarcas(), "IdMarca", "Nombre");
            if (Modelo == null)
            {
                return NotFound();
            }
            return View(Modelo);
        }

        // POST: Modelos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Modelo Modelo)
        {
            if (id != Modelo.IdModelo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = modeloServicio.Editar(Modelo);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        ViewData["IdMarca"] = new SelectList(marcaServicio.ObtenerMarcas(), "IdMarca", "Nombre");
                        return View(Modelo);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            ViewData["IdMarca"] = new SelectList(marcaServicio.ObtenerMarcas(), "IdMarca", "Nombre");
            return View(Modelo);
        }

        // GET: Modelos/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = modeloServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            ViewData["Error"] = respuesta.Message;
            return View("Index", modeloServicio.ObtenerModelos());

        }
    }
}
