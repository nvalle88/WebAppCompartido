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
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TipoPermisoController : Controller
    {
        private readonly ITipoPermisoServicio tipoPermisoServicio;

        public TipoPermisoController(ITipoPermisoServicio tipoPermisoServicio)
        {
            this.tipoPermisoServicio = tipoPermisoServicio;
        }

        // GET: TipoPermiso
        public IActionResult Index()
        {
            return View(tipoPermisoServicio.ObtenerTiposPermisos());
        }

        // GET: TipoPermiso/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoPermiso/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoPermiso tipoPermiso)
        {
            if (ModelState.IsValid)
            {
                var respuesta = tipoPermisoServicio.Crear(tipoPermiso);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    tipoPermiso.Nombre = "";
                    return View(tipoPermiso);
                }


            }
            return View(tipoPermiso);
        }

        // GET: TipoPermiso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoPermiso = tipoPermisoServicio.ObtenerTipoPermiso(Convert.ToInt32(id));
            if (tipoPermiso == null)
            {
                return NotFound();
            }
            return View(tipoPermiso);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoPermiso tipoPermiso)
        {
            if (id != tipoPermiso.IdTipoPermiso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = tipoPermisoServicio.Editar(tipoPermiso);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(tipoPermiso);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(tipoPermiso);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = tipoPermisoServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", tipoPermisoServicio.ObtenerTiposPermisos());

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
