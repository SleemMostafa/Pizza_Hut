using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Pizza_Hut.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        public RoleManager<IdentityRole> IdentityRole;
        public RoleController(RoleManager<IdentityRole> _identityRole)
        {
            IdentityRole = _identityRole;
        }
        public IActionResult Index()
        {
            var roles = IdentityRole.Roles;
            return View(roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string role)
        {
            if(role != null)
            {
                IdentityRole identityRole = new IdentityRole();
                identityRole.Name = role;
                IdentityResult result  = await IdentityRole.CreateAsync(identityRole);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = result.Errors;
                }
            }
            ViewData["RoleName"] = role;
            return View();
        }
    }
}
