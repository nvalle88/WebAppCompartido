using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ds.core.data;
using ds.core.entities;

namespace ZeusWebSite.Controllers
{
    public class MenuOpcionesController : Controller
    {
        private readonly CoreDbContext _context;

        public MenuOpcionesController(CoreDbContext context)
        {
            _context = context;    
        }

        // GET: MenuOpciones
        public async Task<IActionResult> Index()
        {
            var coreDbContext = _context.MenuOpciones.OrderBy(p => p.ParentId).Include(o => o.ParentMenuOption).Include(o => o.Role);
            return View(await coreDbContext.ToListAsync());
        }

        // GET: MenuOpciones/Create
        public IActionResult Create(int? id)
        {
            ViewData["ParentId"] = new SelectList(_context.MenuOpciones.OrderBy(p => p.ParentId), "OpcionMenuId", "Texto", id);
            ViewData["RoleId"] = new SelectList(_context.Set<ApplicationRole>(), "Id", "Name");
            return View();
        }

        // POST: MenuOpciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OpcionMenuId,Texto,Glyph,Url,RoleId,ParentId")] OpcionMenu opcionMenu)
        {
            if (ModelState.IsValid)
            {
                opcionMenu.Url = opcionMenu.Url ?? "#";
                _context.Add(opcionMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ParentId"] = new SelectList(_context.MenuOpciones.OrderBy(p => p.ParentId), "OpcionMenuId", "Texto", opcionMenu.ParentId);
            ViewData["RoleId"] = new SelectList(_context.Set<ApplicationRole>(), "Id", "Name", opcionMenu.RoleId);
            return View(opcionMenu);
        }

        // GET: MenuOpciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opcionMenu = await _context.MenuOpciones.SingleOrDefaultAsync(m => m.OpcionMenuId == id);
            if (opcionMenu == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.MenuOpciones.OrderBy(p => p.ParentId), "OpcionMenuId", "Texto", opcionMenu.ParentId);
            ViewData["RoleId"] = new SelectList(_context.Set<ApplicationRole>(), "Id", "Name", opcionMenu.RoleId);
            return View(opcionMenu);
        }

        // POST: MenuOpciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OpcionMenuId,Texto,Glyph,Url,RoleId,ParentId")] OpcionMenu opcionMenu)
        {
            if (id != opcionMenu.OpcionMenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    opcionMenu.Url = opcionMenu.Url ?? "#";
                    _context.Update(opcionMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpcionMenuExists(opcionMenu.OpcionMenuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ParentId"] = new SelectList(_context.MenuOpciones.OrderBy(p => p.ParentId), "OpcionMenuId", "Texto", opcionMenu.ParentId);
            ViewData["RoleId"] = new SelectList(_context.Set<ApplicationRole>(), "Id", "Name", opcionMenu.RoleId);
            return View(opcionMenu);
        }

        // GET: MenuOpciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opcionMenu = await _context.MenuOpciones.OrderBy(p => p.ParentId)
                .Include(o => o.ParentMenuOption)
                .Include(o => o.Role)
                .SingleOrDefaultAsync(m => m.OpcionMenuId == id);
            if (opcionMenu == null)
            {
                return NotFound();
            }

            return View(opcionMenu);
        }

        // POST: MenuOpciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opcionMenu = await _context.MenuOpciones.SingleOrDefaultAsync(m => m.OpcionMenuId == id);
            _context.MenuOpciones.Remove(opcionMenu);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OpcionMenuExists(int id)
        {
            return _context.MenuOpciones.Any(e => e.OpcionMenuId == id);
        }

        private IOrderedQueryable<OpcionMenu> OpcionesMenuOrdenadas()
        {
            return _context.MenuOpciones.OrderBy(p => p.ParentId);
        }
    }
}
