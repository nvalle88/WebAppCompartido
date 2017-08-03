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
    public class TiposConcursosController : Controller
    {
        private readonly ITipoConcursoServicio tipoCertificadoServicio;

        public TiposConcursosController(ITipoConcursoServicio tipoCertificadoServicio)
        {
            this.tipoCertificadoServicio = tipoCertificadoServicio;
        }

        // GET: TipoConcurso
        public IActionResult Index()
        {
            return View(tipoCertificadoServicio.ObtenerTiposConcursos());
        }

        // GET: TipoConcurso/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoConcurso/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoConcurso tipoCertificado)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoCertificadoServicio.Crear(tipoCertificado);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    tipoCertificado.Nombre = "";
                    return View(tipoCertificado);
                }


            }
            return View(tipoCertificado);
        }

        // GET: TipoConcurso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoCertificado = tipoCertificadoServicio.ObtenerTipoConcurso(Convert.ToInt32(id));
            if (tipoCertificado == null)
            {
                return NotFound();
            }
            return View(tipoCertificado);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoConcurso tipoCertificado)
        {
            if (id != tipoCertificado.IdTipoConcurso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoCertificadoServicio.Editar(tipoCertificado);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(tipoCertificado);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(tipoCertificado);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoCertificadoServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoCertificadoServicio.ObtenerTiposConcursos());

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
