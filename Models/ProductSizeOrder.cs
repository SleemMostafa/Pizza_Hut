using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Pizza_Hut.Models
{
    public class ProductSizeOrder
    {
        [Key]
        public int ID{ get; set; }
        public int ProductSizeID{ get; set; }
        public int OrderID{ get; set; }
        public int Quantity{ get; set; }
        [ForeignKey("ProductSizeID")]
        public virtual ProductSize ProductSize { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
    }
}
