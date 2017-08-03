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
    public class TiposMovimientosInternosController : Controller
    {
        private readonly ITipoMovimientoInternoServicio tipoMovimientoInternoServicio;

        public TiposMovimientosInternosController(ITipoMovimientoInternoServicio tipoMovimientoInternoServicio)
        {
            this.tipoMovimientoInternoServicio = tipoMovimientoInternoServicio;
        }

        // GET: TipoMovimientoInterno
        public IActionResult Index()
        {
            return View(tipoMovimientoInternoServicio.ObtenerTiposMovimientosInternos());
        }

        // GET: TipoMovimientoInterno/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoMovimientoInterno/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoMovimientoInterno tipoMovimientoInterno)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoMovimientoInternoServicio.Crear(tipoMovimientoInterno);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    tipoMovimientoInterno.Nombre = "";
                    return View(tipoMovimientoInterno);
                }


            }
            return View(tipoMovimientoInterno);
        }

        // GET: TipoMovimientoInterno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMovimientoInterno = tipoMovimientoInternoServicio.ObtenerTipoMovimientoInterno(Convert.ToInt32(id));
            if (tipoMovimientoInterno == null)
            {
                return NotFound();
            }
            return View(tipoMovimientoInterno);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoMovimientoInterno tipoMovimientoInterno)
        {
            if (id != tipoMovimientoInterno.IdTipoMovimientoInterno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoMovimientoInternoServicio.Editar(tipoMovimientoInterno);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(tipoMovimientoInterno);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(tipoMovimientoInterno);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoMovimientoInternoServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoMovimientoInternoServicio.ObtenerTiposMovimientosInternos());

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
