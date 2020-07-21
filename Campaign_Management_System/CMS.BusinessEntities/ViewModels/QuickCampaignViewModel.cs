using CMS.Data.Database;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class QuickCampaignViewModel
    {
        public int QuickCampaignId { get; set; }
        [Required(ErrorMessage = "QuickCampaign Name is required")]
        public string QuickCampaignName { get; set; }
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime Start_Date { get; set; }
        [Required(ErrorMessage = "Created Date is required")]
        public DateTime CreatedOn { get; set; }
        [Required(ErrorMessage = "Modified Date is required")]
        public DateTime ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public virtual User User { get; set; }

        [Display(Name = "Budget")]
        [Required(ErrorMessage = "CampaignBudget is required")]
        public Decimal CampaignBudget { get; set; }

        [Display(Name = "Expected Revenue")]
        [Required(ErrorMessage = "Expected revenue is required")]
        public Decimal ExpectedRevenue { get; set; }
        [Required(ErrorMessage = "Template Id is required")]
        public int TemplateId { get; set; }
        public virtual Template Template { get; set; }



    }
}
