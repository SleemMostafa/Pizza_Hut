using Microsoft.AspNetCore.Mvc;
using Pizza_Hut.Repository;

namespace Pizza_Hut.Controllers
{
    public class ProductSizeOrderController : Controller
    {
        IProductSizeOrderRepository productSizeOrderRepository;
        public ProductSizeOrderController(IProductSizeOrderRepository _productSizeOrderRepository)
        {
            productSizeOrderRepository = _productSizeOrderRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
