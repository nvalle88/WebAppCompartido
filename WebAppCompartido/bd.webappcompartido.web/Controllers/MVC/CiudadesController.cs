using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ds.zeus.services.Interfaces;
using ds.zeus.entities.Modelos.Compartido;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class CiudadesController : Controller
    {
        private readonly ICiudadServicio ciudadServicio;
        private readonly IProvinciaServicio provinciaServicio;
        private readonly IPaisServicio paisServicio;

        public CiudadesController(ICiudadServicio ciudadServicio, IProvinciaServicio provinciaServicio, IPaisServicio paisServicio)
        {
            this.ciudadServicio = ciudadServicio;
            this.provinciaServicio = provinciaServicio;
            this.paisServicio = paisServicio;
        }

        // GET: ciudades
        public IActionResult Index()
        {
            return View(ciudadServicio.ObtenerCiudadades());
        }

        // GET: ciudades/Create
        public IActionResult Create()
        {
            ViewBag.ListaPaises = paisServicio.ObtenerPaises();
            //ViewData["IdProvincia"] = new SelectList(provinciaServicio.ObtenerProvincias(), "IdProvincia", "Nombre");
            //ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre");
            return View();
        }

        // POST: ciudades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ciudad Ciudad)
        {
            if (ModelState.IsValid)
            {
                var response = ciudadServicio.Crear(Ciudad);

                if (response.IsSuccess)
                {

                    return RedirectToAction("Index");
                }
                var provincia = provinciaServicio.ObtenerProvincia(Ciudad.IdProvincia);
                ViewData["Error"] = response.Message;
                ViewData["IdProvincia"] = new SelectList(provinciaServicio.ObtenerProvincias(), "IdProvincia", "Nombre");
                ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre", provincia.IdPais);
                return View(Ciudad);
            }
            var prov = provinciaServicio.ObtenerProvincia(Ciudad.IdProvincia);
            ViewData["IdProvincia"] = new SelectList(provinciaServicio.ObtenerProvincias(), "IdProvincia", "Nombre");
            ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre",prov.IdPais);
            return View(Ciudad);
        }

        // GET: ciudades/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Ciudad = ciudadServicio.ObtenerCiudad(Convert.ToInt32(id));
            var provincia = ciudadServicio.ObtenerProvincia(Convert.ToInt32(id));
            ViewData["IdProvincia"] = new SelectList(provinciaServicio.ObtenerProvincias(), "IdProvincia", "Nombre",provincia.IdProvincia);
            ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre", provincia.IdPais);
            if (Ciudad == null)
            {
                return NotFound();
            }
            return View(Ciudad);
        }

        // POST: ciudades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Ciudad ciudad)
        {
            if (id != ciudad.IdCiudad)
            {
                return NotFound();
            }

            Provincia provincia = new Provincia();
            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = ciudadServicio.Editar(ciudad);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        provincia = ciudadServicio.ObtenerProvincia(Convert.ToInt32(id));
                        ViewData["IdProvincia"] = new SelectList(provinciaServicio.ObtenerProvincias(), "IdProvincia", "Nombre", provincia.IdProvincia);
                        ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre", provincia.IdPais);
                        return View(ciudad);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
           
            ViewData["IdProvincia"] = new SelectList(provinciaServicio.ObtenerProvincias(), "IdProvincia", "Nombre", provincia.IdProvincia);
            ViewData["IdPais"] = new SelectList(paisServicio.ObtenerPaises(), "IdPais", "Nombre", provincia.IdPais);
            return View(ciudad);
        }

        // GET: ciudades/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = ciudadServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            ViewData["Error"] = respuesta.Message;
            return View("Index", ciudadServicio.ObtenerCiudadades());


        }

        public JsonResult GetProvincias(int idPais)
        {
            var listaPais = paisServicio.ObtenerProvincias(idPais);

            listaPais.Insert(0, new Provincia {IdProvincia=0,Nombre="Seleccionar Provincia" });

            return Json(new SelectList(listaPais, "IdProvincia", "Nombre"));
        }
    }
}
