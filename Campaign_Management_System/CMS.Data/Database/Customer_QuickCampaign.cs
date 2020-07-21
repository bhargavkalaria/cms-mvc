using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Database
{
    public class Customer_QuickCampaign
    {
        [Key]
        public int Id { get; set; }

        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        public bool isEmailSent { get; set; }
        public string Response { get; set; }
        public int QuickCampaignId { get; set; }
        [ForeignKey("QuickCampaignId")]
        public virtual QuickCampaign QuickCampaign { get; set; }


    }
}
