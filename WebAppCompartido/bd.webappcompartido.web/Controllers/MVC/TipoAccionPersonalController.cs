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
    public class TipoAccionPersonalController : Controller
    {
        private readonly ITipoAccionPersonalServicio tipoAccionPersonalServicio;

        public TipoAccionPersonalController(ITipoAccionPersonalServicio tipoAccionPersonalServicio)
        {
            this.tipoAccionPersonalServicio = tipoAccionPersonalServicio;
        }

        // GET: TipoAccionPersonal
        public IActionResult Index()
        {
            return View(tipoAccionPersonalServicio.ObtenerTiposAccionesPersonal());
        }

        // GET: TipoAccionPersonal/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoAccionPersonal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoAccionPersonal tipoAccionPersonal)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoAccionPersonalServicio.Crear(tipoAccionPersonal);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    tipoAccionPersonal.Descripcion = "";
                    return View(tipoAccionPersonal);
                }


            }
            return View(tipoAccionPersonal);
        }

        // GET: TipoAccionPersonal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoAccionPersonal = tipoAccionPersonalServicio.ObtenerTipoAccionPersonal(Convert.ToInt32(id));
            if (tipoAccionPersonal == null)
            {
                return NotFound();
            }
            return View(tipoAccionPersonal);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoAccionPersonal tipoAccionPersonal)
        {
            if (id != tipoAccionPersonal.IdTipoAccionPersonal)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoAccionPersonalServicio.Editar(tipoAccionPersonal);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(tipoAccionPersonal);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(tipoAccionPersonal);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoAccionPersonalServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoAccionPersonalServicio.ObtenerTiposAccionesPersonal());

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
