using System;
using System.Threading.Tasks;
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
    public class ParroquiasController : Controller
    {
        private readonly IParroquiaServicio parroquiaServicio;
        private readonly ICiudadServicio CiudadServicio;

        public ParroquiasController(IParroquiaServicio parroquiaServicio,ICiudadServicio CiudadServicio)
        {
            this.parroquiaServicio = parroquiaServicio;
            this.CiudadServicio = CiudadServicio;
        }

        // GET: Parroquias
        public IActionResult Index()
        {
            return View(parroquiaServicio.ObtenerParroquias());
        }

        // GET: Parroquias/Details/5
        public IActionResult Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var response = new Response() ; //parroquiaServicio.CambiarSiActivo(true, Convert.ToInt32(id));
            if (response.IsSuccess)
            {
                return RedirectToAction("Index", "Parroquias");
            }

            return NotFound();
        }

        // GET: Parroquias/Create
        public IActionResult Create()
        {
            ViewData["IdCiudad"] = new SelectList(CiudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre");
            return View();
        }

        // POST: Parroquias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Parroquia Parroquia)
        {
            if (ModelState.IsValid)
            {
               var response= parroquiaServicio.Crear(Parroquia);
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                return NotFound();
               
            }
            ViewData["IdCiudad"] = new SelectList(CiudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre", Parroquia.IdCiudad);
            return View(Parroquia);
        }

        // GET: Parroquias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Parroquia =  parroquiaServicio.ObtenerParroquia(Convert.ToInt32(id));
            if (Parroquia == null)
            {
                return NotFound();
            }
            ViewData["IdCiudad"] = new SelectList(CiudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre", Parroquia.IdCiudad);
            return View(Parroquia);
        }

        // POST: Parroquias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Parroquia Parroquia)
        {
            if (id != Parroquia.IdParroquia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    parroquiaServicio.Editar(Parroquia);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction("Index");
            }
            ViewData["IdCiudad"] = new SelectList(CiudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre", Parroquia.IdCiudad);
            return View(Parroquia);
        }

        // GET: Parroquias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =new Response() ;// parroquiaServicio.CambiarSiActivo(false, Convert.ToInt32(id));
            if (response.IsSuccess)
            {
                return RedirectToAction("Index", "Parroquias");
            }
            return NotFound();
            
        }
    }
}
