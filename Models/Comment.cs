using System.ComponentModel.DataAnnotations.Schema;

namespace Pizza_Hut.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string comment { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

    }
}
