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
    public class ItemsViaticosController : Controller
    {
        private readonly IItemViaticoServicio itemViaticoServicio;

        public ItemsViaticosController(IItemViaticoServicio itemViaticoServicio)
        {
            this.itemViaticoServicio = itemViaticoServicio;
        }

        // GET: ItemViatico
        public IActionResult Index()
        {
            return View(itemViaticoServicio.ObtenerItemsViaticos());
        }

        // GET: ItemViatico/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemViatico/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemViatico itemViatico)
        {
            if (ModelState.IsValid)
            {
                var respuesta = itemViaticoServicio.Crear(itemViatico);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    itemViatico.Descipcion = "";
                    return View(itemViatico);
                }


            }
            return View(itemViatico);
        }

        // GET: ItemViatico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemViatico = itemViaticoServicio.ObtenerItemViatico(Convert.ToInt32(id));
            if (itemViatico == null)
            {
                return NotFound();
            }
            return View(itemViatico);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemViatico itemViatico)
        {
            if (id != itemViatico.IdItemViatico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = itemViaticoServicio.Editar(itemViatico);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(itemViatico);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(itemViatico);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = itemViaticoServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", itemViaticoServicio.ObtenerItemsViaticos());

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
