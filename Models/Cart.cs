using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizza_Hut.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public double TotalPrice { get; set; }
        public string CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer  Customer { get; set; }
        public virtual ICollection<ProductSizeCart> ProductSizeCarts { get; set; }

    }
}
