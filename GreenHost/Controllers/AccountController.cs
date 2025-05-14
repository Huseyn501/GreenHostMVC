using GreenHost.DAL;
using GreenHost.Models;
using GreenHost.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GreenHost.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _context;

        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(AppDbContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = new AppUser()
            {
                UserName = registerVm.Username,
                Email = registerVm.Email,
                Name = registerVm.Name,
                Surname = registerVm.Surname,
            };
            var result = await userManager.CreateAsync(appUser, registerVm.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm,string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await userManager.FindByEmailAsync(loginVm.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View();
            }
            var result = await signInManager.PasswordSignInAsync(appUser, loginVm.Password, false, false);
            if (result.Succeeded) 
            {
                await signInManager.SignInAsync(appUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View();
            }


           
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Member"));
            return RedirectToAction("Index","Home");
        }

    }
}
