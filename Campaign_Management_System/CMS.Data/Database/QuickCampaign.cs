using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Database
{
    public class QuickCampaign
    {
        [Key]
        public int QuickCampaignId { get; set; }
        public string QuickCampaignName { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        [ForeignKey("User")]
        public int CreatedBy { get; set; }
        public virtual User User { get; set; }

        public Decimal CampaignBudget { get; set; }

        public Decimal ExpectedRevenue { get; set; }


        public int TemplateId { get; set; }
        public virtual Template Template { get; set; }


    }
}
