using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using System;
using System.Collections.Generic;

namespace Pizza_Hut.Controllers
{
    public class ProductSizeController : Controller
    {
        IProductSizeRepository productSizeRepository;
        public ProductSizeController(IProductSizeRepository _productSizeRepository)
        {
            productSizeRepository = _productSizeRepository;
        }
        public IActionResult Index()
        {
            List<ProductSize> productSizes = productSizeRepository.GetAll();
            return View(productSizes);
        }
        public IActionResult Details(int id)
        {
            try
            {
                return View(productSizeRepository.productSizesForProduct(id));
            }
            catch(Exception ex)
            {
                return View(ex);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View(new ProductSize());
        }
        [HttpPost]
        public IActionResult Add(ProductSize productSize)
        {
            if (ModelState.IsValid == true)
            {
                if (productSize != null)
                {
                    productSizeRepository.Insert(productSize);
                    return RedirectToAction("Index");
                }
            }
            return View("Add", productSize);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            return View(productSizeRepository.GetById(id));
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, ProductSize productSize)
        {
            if (productSize.Price > 0)
            {
                productSizeRepository.Update(id, productSize);
                return RedirectToAction("Index");
            }
            return View("Edit", productSize);
        }
    }
}
