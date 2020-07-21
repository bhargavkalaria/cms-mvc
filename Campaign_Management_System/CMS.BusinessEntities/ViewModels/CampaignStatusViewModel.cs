using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class CampaignStatusViewModel
    {
        public int CampaignStatusId { get; set; }

        [Required(ErrorMessage = "Enter Camapaign Status")]
        public string Status { get; set; }
    }
}
