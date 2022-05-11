using Pizza_Hut.Models;
using Pizza_Hut.ViewModel.ProductViewModel;
using System.ComponentModel.DataAnnotations;

namespace Pizza_Hut.Validations
{
    public class CheckProductSize: ValidationAttribute    
    {
        Context context;
        public CheckProductSize()
        {

        }
        public CheckProductSize(Context _context) 
        {
            context = _context;
        }
        public string Message { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProductSizeViewModel productSizeViewModel = value as ProductSizeViewModel;
            if (
                (productSizeViewModel.Small && productSizeViewModel.PriceSmall > 0) 
                || (productSizeViewModel.Medium && productSizeViewModel.PriceMedium > 0) 
                || (productSizeViewModel.Large && productSizeViewModel.PriceLarge > 0)
                )
            {
                if((productSizeViewModel.Small && productSizeViewModel.PriceSmall == null) || (!productSizeViewModel.Small && productSizeViewModel.PriceSmall > 0))
                {
                    return new ValidationResult(Message);
                }
                if ((productSizeViewModel.Medium && productSizeViewModel.PriceMedium == null) || (!productSizeViewModel.Medium && productSizeViewModel.PriceMedium > 0))
                {
                    return new ValidationResult(Message);
                }
                if ((productSizeViewModel.Large && productSizeViewModel.PriceLarge == null) || (!productSizeViewModel.Large && productSizeViewModel.PriceLarge > 0))
                {
                    return new ValidationResult(Message);
                }
                return ValidationResult.Success;
            }
            return new ValidationResult(Message);
        }
    }
}
