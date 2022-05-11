using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using Pizza_Hut.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pizza_Hut.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly UserManager<Customer> userManager;
        private readonly SignInManager<Customer> signInManager;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ICartRepository cartRepository;
        public IEnumerable<Claim> ClaimsAsmaa { get; set; }
        public AccountController(UserManager<Customer> userManager
            , SignInManager<Customer> signInManager,
            ICategoryRepository _categoryRepository,
            ICustomerRepository custRepo,
            ICartRepository _cartRepository
            )
        {
            this.signInManager = signInManager;
            categoryRepository = _categoryRepository;
            this.userManager = userManager;
            customerRepository = custRepo;
            cartRepository = _cartRepository;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerViewModel newUser)//Customize property - validation atrbiute
        {
            if (ModelState.IsValid)
            {
                Customer UserModel = new Customer();
                UserModel.UserName = newUser.UserName;
                UserModel.PasswordHash = newUser.Password;
                UserModel.Address = newUser.Address;
                UserModel.PhoneNumber = newUser.PhoneNumber;
                IdentityResult result =
                    await userManager.CreateAsync(UserModel, UserModel.PasswordHash);
                if (result.Succeeded)
                {
                    ClaimsIdentity claims = new ClaimsIdentity();
                    //List<Claim> claims1 = new List<Claim>();
                    claims.AddClaim(new Claim("ID", UserModel.Id));


                    // comment
                    // await signInManager.SignInWithClaimsAsync(UserModel, false, claims);

                    //claims1.Add(new Claim("ID", UserModel.Id));
                    //claims.AddClaims(claims1);
                    //create cookie
                    //var identity = User.Identity as ClaimsIdentity;
                    //await signInManager.SignInWithClaimsAsync(UserModel, false, claims1);
                    await signInManager.SignInAsync(UserModel, false);
                    string i = UserModel.Id;
                    Cart cart = new Cart();
                    cart.CustomerID = i;
                    cart.TotalPrice = 0;
                    cartRepository.Insert(cart);


                    //string id= HttpContext.User.Claims("ID");
                    return RedirectToAction("Index", "Home", categoryRepository.GetAll());
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                }
                //save 
            }
            return View(newUser);
        }
        //------------------Register for Admin------------------
        [HttpGet]
       // [Authorize(Roles = "Admin")]
        public IActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin(CustomerViewModel newUser)//Customize property - validation atrbiute
        {
            if (ModelState.IsValid)
            {
                Customer UserModel = new Customer();
                UserModel.UserName = newUser.UserName;
                UserModel.PasswordHash = newUser.Password;
                UserModel.Address = newUser.Address;
                UserModel.PhoneNumber = newUser.PhoneNumber;
                IdentityResult result =
                    await userManager.CreateAsync(UserModel, UserModel.PasswordHash);
                if (result.Succeeded)
                {
                    IdentityResult RoleResult = await userManager.AddToRoleAsync(UserModel, "Admin");
                    if (RoleResult.Succeeded)
                    {
                        await signInManager.SignInAsync(UserModel, false);
                        return RedirectToAction("Index", "Home", categoryRepository.GetAll());
                    }
                    else
                    {
                        foreach (var item in RoleResult.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                }

            }
            return View(newUser);
        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel LoginAccount)
        {
            if (ModelState.IsValid)
            {
                Customer UserModel =
                    await userManager.FindByNameAsync(LoginAccount.USerName);
                if (UserModel != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await signInManager.PasswordSignInAsync
                        (UserModel, LoginAccount.Password, LoginAccount.isPersistent, false);
                    string i = UserModel.Id;
                    if(await userManager.IsInRoleAsync(UserModel,"Admin") == true)
                    {
                        return RedirectToAction("Admin", "Home");
                    }
      
                        return RedirectToAction("index", "Home", categoryRepository.GetAll());
                    
                }
                else
                {
                    ModelState.AddModelError("", "Username & password Invalid");
                }
            }
            return View(LoginAccount);

        }
    }
}
