using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizza_Hut.Models
{
   public enum Size
      {
        Small,
        medium,
        Large
      }
    public class ProductSize
    {
        public int ID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public Size size { get; set; }
        public float Price { get; set; }
        public virtual ICollection<ProductSizeCart> ProductSizeCarts { get; set; }
        public virtual ICollection<ProductSizeOrder> ProductSizeOrders { get; set; }
    }
}
