using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Pizza_Hut.Models
{
    public class ProductSizeCart
    {
        [Key]
        public int ID { get; set; }
        public int CartID { get; set; }
        public int ProductSizeID{ get; set; }
        public int Quantity{ get; set; }
        [ForeignKey("CartID ")]
        public virtual Cart Cart { set; get; }
        [ForeignKey("ProductSizeID")]
        public virtual ProductSize ProductSize { set; get; }
    }
}
