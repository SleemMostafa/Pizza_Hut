using System.ComponentModel.DataAnnotations;

namespace Pizza_Hut.ViewModel
{
    public class CustomerOrder
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string Phone { get; set; }
    }
}
