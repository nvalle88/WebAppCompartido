using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ds.zeus.entities.Modelos.Compartido;
using ds.zeus.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ds.core.data;
using System.Linq;

namespace ZeusWebSite.Controllers.MVC
{

    public class EstadoCivilController : Controller
    {
        private readonly ISeguridadServicio seguridadServicio;
        private readonly IEstadoCivilServicio estadoCivilServicio;

        public EstadoCivilController(ISeguridadServicio seguridadServicio, IEstadoCivilServicio estadoCivilServicio)
        {

            this.seguridadServicio = seguridadServicio;

            this.estadoCivilServicio = estadoCivilServicio;
        }

        // GET: EstadoCivil
        public IActionResult Index(string url, string accion)
        {


            if (seguridadServicio.TienePermiso(url, accion, User).IsSuccess)
            {
                return View(estadoCivilServicio.ObtenerEstadosCiviles());
            }

            return NotFound();


        }

        // GET: EstadoCivil/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoCivil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EstadoCivil estadoCivil)
        {
            if (ModelState.IsValid)
            {
                var respuesta = estadoCivilServicio.Crear(estadoCivil);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    estadoCivil.Nombre = "";
                    return View(estadoCivil);
                }


            }
            return View(estadoCivil);
        }

        // GET: EstadoCivil/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoCivil = estadoCivilServicio.ObtenerEstadoCivil(Convert.ToInt32(id));
            if (estadoCivil == null)
            {
                return NotFound();
            }
            return View(estadoCivil);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EstadoCivil estadoCivil)
        {
            if (id != estadoCivil.IdEstadoCivil)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = estadoCivilServicio.Editar(estadoCivil);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(estadoCivil);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(estadoCivil);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = estadoCivilServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", estadoCivilServicio.ObtenerEstadosCiviles());

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
