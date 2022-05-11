using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pizza_Hut.Hubs;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using Pizza_Hut.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pizza_Hut.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryRepository CategoryRepository;
        IProductRepositor productRepositor;
        private readonly INotyfService notyf;
        private readonly IWebHostEnvironment  webHostEnvironment;
        public IHubContext<CategoryHub> CategoryHub { get; }
        public CategoryController(ICategoryRepository _CategoryRepository ,IProductRepositor _productRepositor, IWebHostEnvironment _webHostEnvironment, INotyfService _notyf, IHubContext<CategoryHub> CategoryHub)
        {
             CategoryRepository= _CategoryRepository;
             productRepositor = _productRepositor;
            webHostEnvironment = _webHostEnvironment;
            notyf = _notyf;
            this.CategoryHub = CategoryHub;

        }
        public IActionResult Index()
        {
            return View(CategoryRepository.GetAll());
        }
        public IActionResult Details(int id)
        {
            var query = CategoryRepository.GetById(id);
            if(query == null)
            {
                return BadRequest();
            }
            return View(query);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            CategoryVewModel categoryVM = new CategoryVewModel();
            categoryVM.products = productRepositor.GetAll();
            return View(categoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult SaveCategoryAsync(CategoryVewModel categoryVM)
        {
           
                if (ModelState.IsValid)
                {
                    Category category = new Category();
                    category.Name = categoryVM.Name;
                    category.Description = categoryVM.Description;
                    category.Name = categoryVM.Name;
                    category.Image = UploadedFile(categoryVM.Image);
                    
                    CategoryRepository.Insert(category);
                     
                    CategoryHub.Clients.All.SendAsync("AddNewCategory", category);
                   // notyf.Success("Created Successfully", 5);
                    return RedirectToAction("Index", "Home");
                }

            
            return View("Add", categoryVM);
        }
        public string UploadedFile(IFormFile model)
        {
            string uniqueFileName = null;

            if (model != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
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
            CategoryVewModel categoryVM = new CategoryVewModel();
            if (ModelState.IsValid)
            {
                var query = CategoryRepository.GetById(id);
                categoryVM.ID = query.ID;
                categoryVM.Name = query.Name;
                categoryVM.Description = query.Description;
                categoryVM.ImageName = query.Image;
                return View(categoryVM);
            }
            return View(categoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEdit([FromRoute] int id, CategoryVewModel categoryVM)
        {
            Category category = new Category();
            category.Name = categoryVM.Name;
            category.Description = categoryVM.Description;
            if (categoryVM.Image != null)
            {
                category.Image = UploadedFile(categoryVM.Image);
            }
            else
            {
                category.Image = categoryVM.ImageName;
            }
            if (category.Name != null)
            {
                CategoryRepository.Update(id, category);
              //  notyf.Success("Edit Successfully", 5);
                return RedirectToAction("Index","Home");
            }

            return View("Edit", category);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            CategoryRepository.Delete(id);
            return Ok();
        }
    }
}
