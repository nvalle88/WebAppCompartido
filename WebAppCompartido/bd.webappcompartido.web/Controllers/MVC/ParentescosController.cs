using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ds.zeus.services.Interfaces;
using ds.zeus.entities.Modelos.Compartido;
using Microsoft.AspNetCore.Authorization;

namespace ZeusWebSite.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class ParentescosController : Controller
    {
        private readonly IParentescosServicio ParentescosServicio;

        public ParentescosController(IParentescosServicio ParentescosServicio)
        {
            this.ParentescosServicio = ParentescosServicio;    
        }

        // GET: Parentescos
        public IActionResult Index()
        {
            return View(ParentescosServicio.ObtenerParentescos());
        }

        // GET: Parentescos/Details/5
        public IActionResult Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ParentescosServicio.CambiarSiActivo(true, Convert.ToInt32(id));
            if (response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // GET: Parentescos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parentescos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Parentesco Parentesco)
        {
            if (ModelState.IsValid)
            {
                var response = ParentescosServicio.Crear(Parentesco);
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(Parentesco);
        }

        // GET: Parentescos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var InstitucionFinanciera = ParentescosServicio.ObtenerParentesco(Convert.ToInt32(id));
            if (InstitucionFinanciera == null)
            {
                return NotFound();
            }
            return View(InstitucionFinanciera);
        }

        // POST: Parentescos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Parentesco Parentesco)
        {
            if (id != Parentesco.IdParentesco)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = ParentescosServicio.Editar(id, Parentesco);
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    return NotFound();
                }
                catch (Exception)
                {

                    return NotFound();

                }

            }
            return View(Parentesco);
        }

        // GET: Parentescos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                if (id == null)
                {
                    return NotFound();
                }

            var response = ParentescosServicio.CambiarSiActivo(false, Convert.ToInt32(id));
            if (response.IsSuccess)
            {
                return RedirectToAction("Index", "Parentescos");
            }
            return NotFound();
        }
    }
}
