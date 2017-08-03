using ds.core.entities.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ds.core.entities.Models.AccountViewModels;
using ds.core.entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ds.core.data;

namespace ZeusWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        public UsersAdminController(UserManager<ApplicationUser> userManager
            , RoleManager<ApplicationRole> roleManager,
            CoreDbContext context)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            _context = context;
        }

        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<ApplicationRole> RoleManager;
        private readonly CoreDbContext _context;

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user);

            return View(user);
        }

        //
        // GET: /Users/Create
        public IActionResult Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");
            
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    UserFirstName = userViewModel.Nombre,
                    UserLastName = userViewModel.Apellido
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First().Description);
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First().Description);
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            var userRoles = await UserManager.GetRolesAsync(user);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Nombre = user.UserFirstName,
                Apellido = user.UserLastName,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Email,Id,Nombre,Apellido")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);

                user.Email = editUser.Email;
                user.UserFirstName = editUser.Nombre;
                user.UserLastName = editUser.Apellido;

                var userRoles = await UserManager.GetRolesAsync(user);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().Description);
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().Description);
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(id);
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().Description);
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: /Users/Create
        public ActionResult Crear(int EmpresaId)
        {
            ViewBag.EmpresaId = EmpresaId;
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Crear(RegisterViewModel userViewModel, int EmpresaId)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    //Apellido = "ApellidoHard",
                    //Nombre = "NombreHard"
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    /*var result = await UserManager.AddToRoleAsync(user, "ROLNAMEHERE");
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        ViewBag.EmpresaId = EmpresaId;
                        return View();
                    }
                    */
                   
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First().Description);
                    ViewBag.EmpresaId = EmpresaId;
                    return View();

                }
                return RedirectToAction("Vendedores", new { EmpresaId = EmpresaId });
            }
            ViewBag.EmpresaId = EmpresaId;
            return View();
        }

        public async Task<ActionResult> Editar(string id, int EmpresaId)
        {
            ViewBag.EmpresaId = EmpresaId;

            if (id == null)
            {
                return null;
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return View(new RegisterViewModel()
            {
                Email = user.Email,
            });
        }

        [HttpPost]
        public ActionResult Editar(RegisterViewModel userViewModel, int EmpresaId)
        {
            return RedirectToAction("Vendedores", new { EmpresaId = EmpresaId });
        }
    }
}
