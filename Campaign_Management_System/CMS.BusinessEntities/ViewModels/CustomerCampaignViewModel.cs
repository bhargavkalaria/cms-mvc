using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class CustomerCampaignViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Customer ID is required")]
        public int CustomerID { get; set; }
        public string Response { get; set; }
        public string CustomTemplate { get; set; }
        public bool isEmailSent { get; set; }
        [Required(ErrorMessage = "Campaign Id is required")]
        public int CampaignId { get; set; }
    }
}
