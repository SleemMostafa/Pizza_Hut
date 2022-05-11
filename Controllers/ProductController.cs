using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pizza_Hut.Hubs;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using Pizza_Hut.ViewModel.ProductViewModel;
using Pizza_Hut.Views.ViewModel.ProductViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pizza_Hut.Controllers
{
    public class ProductController : Controller
    {
        public IProductRepositor ProductRepositor;
        private ICategoryRepository categoryRepository;
        private readonly IProductSizeRepository productSizeRepository;
        private readonly IWebHostEnvironment hostEnvironment;

        public IHubContext<ProductHub> ProductHub { get; }

        public ProductController(
            IProductRepositor _productRepositor, 
            ICategoryRepository _categoryRepository,
            IProductSizeRepository _productSizeRepository,
            IWebHostEnvironment _hostEnvironment,
             IHubContext<ProductHub> ProductHub
            )
        {
            ProductRepositor = _productRepositor;
            categoryRepository = _categoryRepository;
            productSizeRepository = _productSizeRepository;
            hostEnvironment = _hostEnvironment;
            this.ProductHub = ProductHub;
        }
        public IActionResult Index()
        {
            return View(ProductRepositor.GetAll());
        }
        public IActionResult Details(int id)
        {
            try
            {
                return View("ProductDetails", ProductRepositor.GetById(id));
            }catch(Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }

        public IActionResult Add()
        {
            try
            {
                ProductViewModel product = new ProductViewModel();
                product.CategoryList = categoryRepository.GetAll();
                return View(product);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(ProductViewModel productViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Product product = new Product();
                    product.CategoryID = productViewModel.CategoryID;
                    product.Description = productViewModel.Description;
                    product.Name = productViewModel.Name;
                    product.Photo = UploadedFile(productViewModel.ProductImage);
                    await ProductHub.Clients.All.SendAsync("AddNewProduct", product);
                    ProductRepositor.Insert(product);

                    //ProductSize
                    if (productViewModel.productSizeViewModel.Small)
                    {
                        ProductSize productSize = new ProductSize()
                        {
                            Price = (float)productViewModel.productSizeViewModel.PriceSmall,
                            ProductID = product.ID,
                            size = Size.Small
                        };
                        productSizeRepository.Insert(productSize);
                    }
                    if (productViewModel.productSizeViewModel.Medium)
                    {
                        ProductSize productSize = new ProductSize()
                        {
                            Price = (float)productViewModel.productSizeViewModel.PriceMedium,
                            ProductID = product.ID,
                            size = Size.medium
                        };
                        productSizeRepository.Insert(productSize);
                    }
                    if (productViewModel.productSizeViewModel.Large)
                    {
                        ProductSize productSize = new ProductSize()
                        {
                            Price = (float)productViewModel.productSizeViewModel.PriceLarge,
                            ProductID = product.ID,
                            size = Size.Large
                        };
                        productSizeRepository.Insert(productSize);
                    }
                    return RedirectToAction("DisplayProduct",new { id = product.CategoryID });
                }
                productViewModel.CategoryList = categoryRepository.GetAll();
                return View("Add", productViewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }
        private string UploadedFile(IFormFile model)
        {
            string uniqueFileName = null;

            if (model != null)
            {
                string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            try
            {
                Product product = ProductRepositor.GetById(id);
                ProductViewModel productView = new ProductViewModel()
                {
                    CategoryID = product.CategoryID,
                    ID = product.ID,
                    Name = product.Name,
                    Description = product.Description,
                    Image = product.Photo,
                    CategoryList = categoryRepository.GetAll()
                };
                if (product.ProductSizes != null)
                {
                    foreach (var item in product.ProductSizes)
                    {
                        if (item.size == Size.Small)
                        {
                            productView.productSizeViewModel.Small = true;
                            productView.productSizeViewModel.PriceSmall = item.Price;
                        }
                        else if (item.size == Size.medium)
                        {
                            productView.productSizeViewModel.Medium = true;
                            productView.productSizeViewModel.PriceMedium = item.Price;
                        }
                        else
                        {
                            productView.productSizeViewModel.Large = true;
                            productView.productSizeViewModel.PriceLarge = item.Price;
                        }

                    }
                }
                return View(productView);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult SaveEdit([FromRoute] int id, ProductViewModel productViewModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    Product product = new Product()
                    {
                        CategoryID = productViewModel.CategoryID,
                        Name = productViewModel.Name,
                        Description = productViewModel.Description,
                        Photo = productViewModel.Image,
                    };
                    if (productViewModel.ProductImage != null)
                    {
                        product.Photo = UploadedFile(productViewModel.ProductImage);
                    }
                    ProductRepositor.Update(id, product);
                    

                    //Product Size

                    var productSizes =
                        productSizeRepository.productSizesForProduct(id);
                    //productViewModel.Small

                    if (productViewModel.productSizeViewModel.Small)
                    {
                        ProductSize productSize = productSizes.Find(p => p.size == Size.Small);
                        if (productSize != null)
                        {
                            productSize.Price = (float)productViewModel.productSizeViewModel.PriceSmall;
                            productSizeRepository.Update(productSize.ID, productSize);
                        }
                    }

                    if (productViewModel.productSizeViewModel.Small)
                    {
                        ProductSize productSize = productSizes.Find(p => p.size == Size.Small);
                        if (productSize == null)
                        {
                            ProductSize productSizeNew = new ProductSize()
                            {
                                size = Size.Small,
                                Price = (float)productViewModel.productSizeViewModel.PriceSmall,
                                ProductID = id
                            };
                            productSizeRepository.Insert(productSizeNew);
                        }
                    }

                    if (!productViewModel.productSizeViewModel.Small)
                    {
                        ProductSize productSize = productSizes.Find(p => p.size == Size.Small);
                        if (productSize != null)
                        {
                            productSizeRepository.Delete(productSize.ID);
                        }
                    }

                    //productViewModel.Medium
                    if (productViewModel.productSizeViewModel.Medium)
                    {
                        ProductSize productSize = productSizes.Find(p => p.size == Size.medium);
                        if (productSize != null)
                        {
                            productSize.Price = (float)productViewModel.productSizeViewModel.PriceMedium;
                            productSizeRepository.Update(productSize.ID, productSize);
                        }
                    }

                    if (productViewModel.productSizeViewModel.Medium)
                    {
                        ProductSize productSize = productSizes.Find(p => p.size == Size.medium);
                        if (productSize == null)
                        {
                            ProductSize productSizeNew = new ProductSize()
                            {
                                size = Size.medium,
                                Price = (float)productViewModel.productSizeViewModel.PriceMedium,
                                ProductID = id
                            };
                            productSizeRepository.Insert(productSizeNew);
                        }
                    }

                    if (!productViewModel.productSizeViewModel.Medium)
                    {
                        ProductSize productSize = productSizes.Find(p => p.size == Size.medium);
                        if (productSize != null)
                        {
                            productSizeRepository.Delete(productSize.ID);
                        }
                    }

                    //productViewModel.Large
                    if (productViewModel.productSizeViewModel.Large)
                    {
                        ProductSize productSize = productSizes.Find(p => p.size == Size.Large);
                        if (productSize != null)
                        {
                            productSize.Price = (float)productViewModel.productSizeViewModel.PriceLarge;
                            productSizeRepository.Update(productSize.ID, productSize);
                        }
                    }

                    if (productViewModel.productSizeViewModel.Large)
                    {
                        ProductSize productSize = productSizes.Find(p => p.size == Size.Large);
                        if (productSize == null)
                        {
                            ProductSize productSizeNew = new ProductSize()
                            {
                                size = Size.Large,
                                Price = (float)productViewModel.productSizeViewModel.PriceLarge,
                                ProductID = id
                            };
                            productSizeRepository.Insert(productSizeNew);
                        }
                    }

                    if (!productViewModel.productSizeViewModel.Large)
                    {
                        ProductSize productSize = productSizes.Find(p => p.size == Size.Large);
                        if (productSize != null)
                        {
                            productSizeRepository.Delete(productSize.ID);
                        }
                    }

                    return RedirectToAction("DisplayProduct", new { id = product.CategoryID });
                }
                productViewModel.CategoryList = categoryRepository.GetAll();
                return View("Edit", productViewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }
        private void CheckSmallSize(int id, ProductViewModel productViewModel, List<ProductSize> productSizes)
        {

        }
        private void CheckMediumSize(int id, ProductViewModel productViewModel, List<ProductSize> productSizes)
        {

        }
        private void CheckLargeSize(int id, ProductViewModel productViewModel, List<ProductSize> productSizes)
        {

        }

        [Authorize(Roles = "Admin")]
        public IActionResult PeoductDetailsAdmin(int id)
        {
            try
            {
                Product product = ProductRepositor.GetById(id);
                product.ProductSizes = productSizeRepository.productSizesForProduct(id);
                if (product == null)
                {
                    return View("Error", new ErrorViewModel() { RequestId = "Product Not Found" });
                }
                return View(product);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }

        // POST: Carts/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                int CategoryID = ProductRepositor.GetById(id).CategoryID;
                ProductRepositor.Delete(id);
                return RedirectToAction("DisplayProduct", CategoryID);
            }catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }

        public IActionResult GetCategoryByID(int id)
        {
            try
            {
                return Json(categoryRepository.GetById(id));
            }catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }

        }
        public IActionResult DisplayProduct(int id)
        {
            try
            {
                List<Product> products = ProductRepositor.GetByCategoryID(id);
                if (!User.IsInRole("Admin"))
                {
                    
                    return View("Details", products);
                }
                return View("GetAllByCategoryIdAdmin",products);

            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllByCategoryIdAdmin(int id)
        {
            try
            {
                List<Product> products = ProductRepositor.GetByCategoryID(id);
                return View(products);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }

        }
        public IActionResult ProductSize(ProductSizeViewModel Degree)
        {
            try
            {
                if (Degree.Small || Degree.Medium || Degree.Large)
                    return Json(true);
                return Json(false);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }
    }

}
