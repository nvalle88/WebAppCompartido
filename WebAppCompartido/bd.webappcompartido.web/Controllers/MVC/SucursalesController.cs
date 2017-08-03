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
using ds.core.web.shared.Utils;

namespace ZeusWebSite.Controllers
{
    public class SucursalesController : Controller
    {
        ISucursalServicio sucursalServicio;
        ICiudadServicio ciudadServicio;

        public SucursalesController(ISucursalServicio sucursalServicio, ICiudadServicio ciudadServicio)
        {
            this.sucursalServicio = sucursalServicio;
            this.ciudadServicio = ciudadServicio;
        }

        // GET: Provinces
        public IActionResult Index()
        {
            return View(sucursalServicio.ObtenerSucursales());
        }

        // GET: Provinces/Details/5
        public IActionResult Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = new Response();// sucursalServicio.CambiarSiActivo(true, Convert.ToInt32(id));
            if (response.IsSuccess)
            {
                return RedirectToAction("Index", "Provinces");
            }
            return View();
        }

        // GET: Provinces/Create
        public IActionResult Create()
        {
            ViewData["IdCiudad"] = new SelectList(ciudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre");
            return View();
        }

        // POST: Provinces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sucursal Sucursal)
        {

            if (ModelState.IsValid)
            {
                var response = sucursalServicio.Crear(Sucursal);

                if (response.IsSuccess)
                {

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdCiudad"] = new SelectList(ciudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre");
                return View(Sucursal);
            }
            // ViewData["IdCiudad"] = new SelectList(CiudadServicio.ObtenerCiudadades(true), "IdCiudad", "Name");

            ViewData["IdCiudad"] = new SelectList(ciudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre");
            return View(Sucursal);
        }

        // GET: Provinces/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Sucursal = sucursalServicio.ObtenerSucursal(Convert.ToInt32(id));
            ViewData["IdCiudad"] = new SelectList(ciudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre");
            if (Sucursal == null)
            {
                return NotFound();
            }
            return View(Sucursal);
        }

        // POST: Provinces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Sucursal Sucursal)
        {
            if (id != Sucursal.IdSucursal)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = sucursalServicio.Editar(Sucursal);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;
                        ViewData["IdCiudad"] = new SelectList(ciudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre");
                        return View(Sucursal);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }

            }
            ViewData["IdCiudad"] = new SelectList(ciudadServicio.ObtenerCiudadades(), "IdCiudad", "Nombre");
            return View(Sucursal);
        }

        // GET: Provinces/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = sucursalServicio.Eliminar(Convert.ToInt32(id));
            if (respuesta.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            ViewData["Error"] = respuesta.Message;
            return View("Index", sucursalServicio.ObtenerSucursales());

        }
    }
}
