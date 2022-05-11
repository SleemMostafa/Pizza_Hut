using System.ComponentModel.DataAnnotations;

namespace Pizza_Hut.ViewModel
{
    public class CustomerViewModel
    {
        [Required]
        public string UserName { set; get; }
        [Required] 
        public string Address { set; get; }
        [Required] 
        public string PhoneNumber { set; get; }
        [Required,DataType(DataType.Password)] 
        public string Password { set; get; }
        [Required, DataType(DataType.Password)]
        [Compare("Password")] 
        public string PasswordConfirme { set; get; }

    }
}
