using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ds.zeus.entities.Modelos.Compartido;
using ds.zeus.services.Interfaces;

namespace ZeusWebSite.Controllers
{
    public class TipoActivoFijoController : Controller
    {
        private readonly ITipoActivoFijoServicio tipoActivoFijoServicio;

        public TipoActivoFijoController(ITipoActivoFijoServicio tipoActivoFijoServicio)
        {
            this.tipoActivoFijoServicio = tipoActivoFijoServicio;
        }

        // GET: TipoActivoFijo
        public IActionResult Index()
        {
            return View(tipoActivoFijoServicio.ObtenerTiposActivosFijos());
        }

        // GET: TipoActivoFijo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoActivoFijo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoActivoFijo tipoActivoFijo)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoActivoFijoServicio.Crear(tipoActivoFijo);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    tipoActivoFijo.Nombre = "";
                    return View(tipoActivoFijo);
                }


            }
            return View(tipoActivoFijo);
        }

        // GET: TipoActivoFijo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoActivoFijo = tipoActivoFijoServicio.ObtenerTipoActivoFijo(Convert.ToInt32(id));
            if (tipoActivoFijo == null)
            {
                return NotFound();
            }
            return View(tipoActivoFijo);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoActivoFijo tipoActivoFijo)
        {
            if (id != tipoActivoFijo.IdTipoActivoFijo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoActivoFijoServicio.Editar(tipoActivoFijo);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(tipoActivoFijo);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(tipoActivoFijo);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoActivoFijoServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoActivoFijoServicio.ObtenerTiposActivosFijos());

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
