using System.ComponentModel.DataAnnotations;


namespace CMS.Data.Database
{
    public class CampaignStatus
    {
        [Key]
        public int CampaignStatusId { get; set; }
        public string Status { get; set; }
    }
}
