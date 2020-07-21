using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerID { get; set; }

        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Customer Name is required")]
        public string CustomerName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "Mobile is required")]
        public string Mobile { get; set; }

        [Display(Name = "Age")]
        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
    }
}
