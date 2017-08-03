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

namespace ZeusWebSite.Controllers
{
    public class TiposEnfermedadesController : Controller
    {
        private readonly ITipoEnfermedadServicio tipoEnfermedadServicio;

        public TiposEnfermedadesController(ITipoEnfermedadServicio tipoEnfermedadServicio)
        {
            this.tipoEnfermedadServicio = tipoEnfermedadServicio;
        }

        // GET: TipoEnfermedad
        public IActionResult Index()
        {
            return View(tipoEnfermedadServicio.ObtenerTiposEnfermedades());
        }

        // GET: TipoEnfermedad/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoEnfermedad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoEnfermedad tipoEnfermedad)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoEnfermedadServicio.Crear(tipoEnfermedad);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    tipoEnfermedad.Nombre = "";
                    return View(tipoEnfermedad);
                }


            }
            return View(tipoEnfermedad);
        }

        // GET: TipoEnfermedad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEnfermedad = tipoEnfermedadServicio.ObtenerTipoEnfermedad(Convert.ToInt32(id));
            if (tipoEnfermedad == null)
            {
                return NotFound();
            }
            return View(tipoEnfermedad);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoEnfermedad tipoEnfermedad)
        {
            if (id != tipoEnfermedad.IdTipoEnfermedad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoEnfermedadServicio.Editar(tipoEnfermedad);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(tipoEnfermedad);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(tipoEnfermedad);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoEnfermedadServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoEnfermedadServicio.ObtenerTiposEnfermedades());

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
