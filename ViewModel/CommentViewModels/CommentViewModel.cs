using System.ComponentModel.DataAnnotations;

namespace Pizza_Hut.ViewModel
{
    public class CommentViewModel
    {
        [Required(ErrorMessage = "Please enter Product Name")]
        [Display(Name = "User Name")]
        [MinLength(5, ErrorMessage = "Name Must Be More than 5")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter Your Comment")]
        [Display(Name = "Comment")]
        public string comment { get; set; }
        [Required]
        public int ProductID { get; set; }

    }
}
