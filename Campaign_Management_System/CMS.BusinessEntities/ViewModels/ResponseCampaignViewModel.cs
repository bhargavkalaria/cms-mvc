using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class ResponseCampaignViewModel
    {
        public int ResponseId { get; set; }
        [Required(ErrorMessage = "Campaign Id is required")]
        public int CampaignId { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        public int Neutral { get; set; }
        public int NoResponse { get; set; }
        public string CampaignName { get; set; }
        public string DaysRemaining { get; set; }
        public Decimal CampaignBudget { get; set; }
        public double percentageFor { get; set; }
        public bool successOrNot { get; set; }
        public string type { get; set; }
    }
}
