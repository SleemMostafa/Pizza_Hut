using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizza_Hut.Models
{
    
    public class Order
    {
        [Key]
        public int ID { set; get; }
        
        [Required, DataType(DataType.Date)]
        public DateTime Date { set; get; }
        [Required]
        public double TotalPrice { set; get; }
        [Required]
        public string Location { set; get; }

        [ForeignKey("Customer")]
        public string CustomerID { set; get; }
        public virtual Customer Customer { set; get; }
        public virtual ICollection<ProductSizeOrder> ProductSizeOrders { set; get; }
        public Order()
        {
            ProductSizeOrders = new HashSet<ProductSizeOrder>();
        }

    }
}
