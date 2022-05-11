using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Pizza_Hut.Controllers
{
    public class CartController : Controller
    {
        public ICartRepository cartRepository;
        private readonly IProductSizeRepository productSizeRepository;
        private readonly IProductSizeCartRepository productSizeCartRepository;
        private readonly INotyfService notyf;
        private readonly UserManager<Customer> userManager;

        public CartController(ICartRepository cartRepo,
            IProductSizeRepository _productSizeRepository,
            IProductSizeCartRepository _productSizeCartRepository,
            UserManager<Customer> _userManager, INotyfService _notyf)
        {
            cartRepository=cartRepo;
            productSizeRepository = _productSizeRepository;
            productSizeCartRepository = _productSizeCartRepository;
            notyf = _notyf;
        }
        public IActionResult Index()
        {
            List<Cart> carts = cartRepository.GetAll();
            return View("Index", carts);
        }
        [Authorize]
        public IActionResult AddProduct(int id, string ProductSize)  
        {
            ProductSize proSize;
            string userId="";
            if (ProductSize == "Small")
            {
                proSize =  productSizeRepository.GetProductSize(id, Size.Small);
            }
            else if (ProductSize == "medium")
            {
                proSize = productSizeRepository.GetProductSize(id, Size.medium);
            }
            else
            {
                proSize =  productSizeRepository.GetProductSize(id, Size.Large);
            }
           
            if (proSize != null)
            {
                 userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
               
                Cart cart = cartRepository.GetCartByCustomerId(userId);
                if(!cartRepository.ProductSizeIsExistInCart(cart.ID, proSize.ID))
                {
                        ProductSizeCart productSizeCart = new ProductSizeCart();
                        cart.TotalPrice += proSize.Price;
                        productSizeCart.CartID = cart.ID;
                        productSizeCart.ProductSizeID = proSize.ID;
                        productSizeCart.Quantity = 1;
                        productSizeCartRepository.Insert(productSizeCart);
                        return Json(true);
                }
            }
                    return Json(false);
        }
        [Authorize]
        public IActionResult CartDetails()
        {
          var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart cart = cartRepository.GetCartByCustomerId(userId);

            return View("CartItem",cart);
        }
        public IActionResult MinusProductQuantity(int id)
        {
          ProductSizeCart productSizeCart=  productSizeCartRepository.GetById(id);
            if (productSizeCart.Quantity != 1)
            {
                productSizeCart.Quantity--;
                productSizeCart.Cart.TotalPrice -= productSizeCart.ProductSize.Price;
            }

        productSizeCartRepository.Update(id, productSizeCart);
            return Json(productSizeCart);
        }
        public IActionResult PlusProductQuantity(int id)
        {
            ProductSizeCart productSizeCart = productSizeCartRepository.GetById(id);
                productSizeCart.Quantity++;
                productSizeCart.Cart.TotalPrice += productSizeCart.ProductSize.Price;

                productSizeCartRepository.Update(id, productSizeCart);
            
            return Json(productSizeCart);
        }
        public IActionResult Delete(int id)
        {
            ProductSizeCart productSizeCart = productSizeCartRepository.GetById(id);
            productSizeCart.Cart.TotalPrice -= productSizeCart.ProductSize.Price*productSizeCart.Quantity;

            productSizeCartRepository.Delete(id);
            return Json(productSizeCart);

        }
    }
}
