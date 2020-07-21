using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class BrandViewModel
    {
        public int BrandId { get; set; }
        [Required]
        public string BrandName { get; set; }
    }
}
