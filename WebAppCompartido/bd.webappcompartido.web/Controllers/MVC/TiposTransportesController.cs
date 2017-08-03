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
    public class TiposTransportesController : Controller
    {
        private readonly ITipoTransporteServicio tipoTransporteServicio;

        public TiposTransportesController(ITipoTransporteServicio tipoTransporteServicio)
        {
            this.tipoTransporteServicio = tipoTransporteServicio;
        }

        // GET: TipoTransporte
        public IActionResult Index()
        {
            return View(tipoTransporteServicio.ObtenerTiposTransportes());
        }

        // GET: TipoTransporte/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoTransporte/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoTransporte tipoTransporte)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoTransporteServicio.Crear(tipoTransporte);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    tipoTransporte.Descripcion = "";
                    return View(tipoTransporte);
                }


            }
            return View(tipoTransporte);
        }

        // GET: TipoTransporte/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTransporte = tipoTransporteServicio.ObtenerTipoTransporte(Convert.ToInt32(id));
            if (tipoTransporte == null)
            {
                return NotFound();
            }
            return View(tipoTransporte);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoTransporte tipoTransporte)
        {
            if (id != tipoTransporte.IdTipoTransporte)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoTransporteServicio.Editar(tipoTransporte);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(tipoTransporte);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(tipoTransporte);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoTransporteServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoTransporteServicio.ObtenerTiposTransportes());

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
