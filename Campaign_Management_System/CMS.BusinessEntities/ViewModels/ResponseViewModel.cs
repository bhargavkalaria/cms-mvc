using CMS.Data.Database;
using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class ResponseVIewModel
    {
        public int ResponseId { get; set; }
        [Required(ErrorMessage = "Campaign Id is required")]
        public int CampaignId { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        public int Neutral { get; set; }
        public int NoResponse { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}
