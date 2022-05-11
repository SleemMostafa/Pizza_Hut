using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pizza_Hut.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description  { get; set; }
        public string Photo { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public Product()
        {
            Comments = new HashSet<Comment>();
        }
    }
}
