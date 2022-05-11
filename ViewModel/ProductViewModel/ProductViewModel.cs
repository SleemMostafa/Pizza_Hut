using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pizza_Hut.Models;
using Pizza_Hut.Validations;
using Pizza_Hut.ViewModel.ProductViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pizza_Hut.Views.ViewModel.ProductViewModel
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please enter Product Name")]
        [Display(Name = "Product Name")]
        [MinLength(5,ErrorMessage ="Name Must Be More than 5")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter Product Description")]
        [Display(Name = "Product Description")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Please choose Product image")]
        [Display(Name = "Product Image")]
        public IFormFile ProductImage { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage = "Please choose Product Category"),Range(1,int.MaxValue,ErrorMessage ="You Must Select Category")]
        [Display(Name = "Product Category")]
        public int CategoryID { get; set; }
        public List<Category> CategoryList { get; set; }
        public bool Small { get; set; }
        public bool Medium { get; set; }
        public bool Large { get; set; }
        public float PriceSmall { get; set; }
        public float PriceMedium { get; set; }
        public float PriceLarge { get; set; }
        //[Remote(action: "ProductSize", controller: "Product",
        //    ErrorMessage = "MinDegree Must Be Less than Degree")]
        [CheckProductSize(Message = "Choose Size and put Price for this size")]
        public ProductSizeViewModel productSizeViewModel { get; set; }
        public ProductViewModel()
        {
            productSizeViewModel=new ProductSizeViewModel();
        }

    }
}
