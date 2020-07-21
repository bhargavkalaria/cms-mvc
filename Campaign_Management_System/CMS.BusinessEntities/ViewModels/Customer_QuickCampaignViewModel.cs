using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class Customer_QuickCampaignViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "CustomerID is required")]
        public int CustomerID { get; set; }
        public string Response { get; set; }
        public bool isEmailSent { get; set; }
        [Required(ErrorMessage = "QuickCampaign Id is required")]
        public int QuickCampaignId { get; set; }
    }
}
