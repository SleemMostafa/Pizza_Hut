using Microsoft.AspNetCore.Http;
using Pizza_Hut.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pizza_Hut.ViewModel
{
    public class CategoryVewModel
    {
        public int ID { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Error Name Category Must Letter Only")]
        [MinLength(5)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public virtual ICollection<Product> products { get; set; }
    }
}
