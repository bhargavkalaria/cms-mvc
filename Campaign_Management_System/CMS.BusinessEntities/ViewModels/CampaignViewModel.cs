using CMS.Data.Database;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class CampaignViewModel
    {
        public CampaignViewModel()
        {

        }
        public int CampaignId { get; set; }

        [Display(Name = "Campaign Name")]
        [Required(ErrorMessage = "Name of Campaign is required")]
        public string CampaignName { get; set; }

        [Display(Name = "Campaign Owner")]
        [Required(ErrorMessage = "Brand is required")]
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime Start_Date { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "End Date is required")]
        public DateTime End_Date { get; set; }
        public DateTime? Stop_Date { get; set; }
        public DateTime? Resume_Date { get; set; }

        [Display(Name = "Budget")]
        [Required(ErrorMessage = "Campaign Budget is required")]
        public Decimal CampaignBudget { get; set; }

        [Display(Name = "Expected Revenue")]
        [Required(ErrorMessage = "Expected revenue is required")]
        public Decimal ExpectedRevenue { get; set; }

        [Required(ErrorMessage = "Created Date is required")]
        public DateTime CreatedOn { get; set; }
        [Required(ErrorMessage = "Modified Date is required")]
        public DateTime ModifiedOn { get; set; }
        
        public int? CreatedBy { get; set; }
        public virtual User User { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "CampaignStatus is required")]
        public int CampaignStatusId { get; set; }

        public virtual CampaignStatus CampaignStatus { get; set; }

        [Display(Name = "Marketing Type")]
        [Required(ErrorMessage = "Marketing type is required")]
        public int MarketingTypeId { get; set; } //Foreign Key

        public virtual MarketingType MarketingType { get; set; }

        [Display(Name = "Template")]
        [Required(ErrorMessage = "Template is required")]
        public int TemplateId { get; set; }
        public virtual Template Template { get; set; }


        [Display(Name = "Marketing Strategy")]
        [Required(ErrorMessage = "Marketing Strategy is required")]
        public int MarketingStrategyId { get; set; } //Foreign Key

        public virtual MarketingStrategy MarketingStrategy { get; set; }

        public int? TotalUser { get; set; }
    }
}
