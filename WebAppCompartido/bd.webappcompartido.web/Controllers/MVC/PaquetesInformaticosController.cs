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
    public class PaquetesInformaticosController : Controller
    {

        private readonly IPaqueteInformaticoServicio IpaqueteInformaticoServicio;

        public PaquetesInformaticosController(IPaqueteInformaticoServicio IpaqueteInformaticoServicio)
        {
            this.IpaqueteInformaticoServicio = IpaqueteInformaticoServicio;
        }

        // GET: PaquetesInformaticos
        public IActionResult Index()
        {
            return View(IpaqueteInformaticoServicio.ObtenerPaquetesInformaticos());
        }

        // GET: PaquetesInformaticos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaquetesInformaticos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PaquetesInformaticos PaquetesInformaticos)
        {
            if (ModelState.IsValid)
            {
                var respuesta = IpaqueteInformaticoServicio.Crear(PaquetesInformaticos);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;
                    PaquetesInformaticos.Nombre = "";
                    return View(PaquetesInformaticos);
                }


            }
            return View(PaquetesInformaticos);
        }

        // GET: PaquetesInformaticos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PaquetesInformaticos = IpaqueteInformaticoServicio.ObtenerPaqueteInformatico(Convert.ToInt32(id));
            if (PaquetesInformaticos == null)
            {
                return NotFound();
            }
            return View(PaquetesInformaticos);
        }

        // POST: PaquetesInformaticos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PaquetesInformaticos PaquetesInformaticos)
        {
            if (id != PaquetesInformaticos.IdPaquetesInformaticos)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = IpaqueteInformaticoServicio.Editar(PaquetesInformaticos);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        return View(PaquetesInformaticos);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            return View(PaquetesInformaticos);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = IpaqueteInformaticoServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = respuesta.Message;
            return View("Index", IpaqueteInformaticoServicio.ObtenerPaquetesInformaticos());

        }
    }
}
