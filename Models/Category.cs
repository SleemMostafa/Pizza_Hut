using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pizza_Hut.Models
{
    public class Category
    {
        
        public int ID { get; set; }
        [Required,MaxLength(50)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Error Name Category Must Letter Only")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name ="Select Image")]
        public string Image { get; set; }
        public virtual ICollection<Product> products { get; set; }
    }
}
