using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizza_Hut.Models
{
    public class Customer:IdentityUser
    {
        public string Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public int? CartID { get; set; }
        [ForeignKey("CartID")]
        public virtual Cart Cart { get; set; }

    }
}
