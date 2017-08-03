using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ds.zeus.entities.Modelos.Compartido;
using ds.zeus.services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EstudiosController : Controller
    {
        private readonly IEstudioServicio estudioServicio;

        public EstudiosController(IEstudioServicio estudioServicio)
        {
            this.estudioServicio = estudioServicio;
        }

        // GET: Estudio
        public IActionResult Index()
        {
            return View(estudioServicio.ObtenerEstudios());
        }

        // GET: Estudio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estudio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Estudio estudio)
        {
            if (ModelState.IsValid)
            {
                var respuesta = estudioServicio.Crear(estudio);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    estudio.Nombre = "";
                    return View(estudio);
                }


            }
            return View(estudio);
        }

        // GET: Estudio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudio = estudioServicio.ObtenerEstudio(Convert.ToInt32(id));
            if (estudio == null)
            {
                return NotFound();
            }
            return View(estudio);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Estudio estudio)
        {
            if (id != estudio.IdEstudio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = estudioServicio.Editar(estudio);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(estudio);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(estudio);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = estudioServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", estudioServicio.ObtenerEstudios());

        }

        // GET: Pais/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var pais = await _context.Pais
        //    //    .SingleOrDefaultAsync(m => m.IdPais == id);
        //    if (pais == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(pais);
        //}

        //// POST: Pais/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var pais = await _context.Pais.SingleOrDefaultAsync(m => m.IdPais == id);
        //    _context.Pais.Remove(pais);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //private bool PaisExists(int id)
        //{
        //    return _context.Pais.Any(e => e.IdPais == id);
        //}
    }
}
