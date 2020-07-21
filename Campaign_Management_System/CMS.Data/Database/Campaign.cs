using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Database
{

    public class Campaign
    {
        [Key]
        public int CampaignId { get; set; }

        public string CampaignName { get; set; }
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public DateTime? Stop_Date { get; set; }
        public DateTime? Resume_Date { get; set; }
        public Decimal CampaignBudget { get; set; }
        public bool isDeleted { get; set; }
        public Decimal ExpectedRevenue { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        [ForeignKey("User")]
        public int CreatedBy { get; set; }
        public virtual User User { get; set; }
        public int TemplateId { get; set; } //Foreign Key
        [ForeignKey("TemplateId")]
        public virtual Template Template { get; set; }

        public int CampaignStatusId { get; set; }  //Foreign Key
        [ForeignKey("CampaignStatusId")]
        public virtual CampaignStatus CampaignStatus { get; set; }

        public int MarketingTypeId { get; set; } //Foreign Key
        [ForeignKey("MarketingTypeId")]
        public virtual MarketingType MarketingType { get; set; }

        public int MarketingStrategyId { get; set; } //Foreign Key
        [ForeignKey("MarketingStrategyId")]
        public virtual MarketingStrategy MarketingStrategy { get; set; }

        public virtual Response Response { get; set; }
        public virtual Customer_Campaign Customer_Campaign { get; set; }

        
    }
}
