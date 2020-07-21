using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class UserViewModel
    {
        
        public int UId { get; set; }
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Role { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LName { get; set; }
        public bool RememberMe { get; set; }
        public bool viewCampaignAccess { get; set; }
        public bool addCampaignAccess { get; set; }
        public bool editCampaignAccess { get; set; }
        public bool deleteCampainAccess { get; set; }
        public bool uploadCustomerAccess { get; set; }
        public bool addQuickCampaignAccess { get; set; }
        public bool viewQuickCampaignAccess { get; set; }
        public bool addTemplateAccess { get; set; }
        public bool viewTemplateAccess { get; set; }
        public bool editTemplateAccess { get; set; }
        public bool deleteTemplateAccess { get; set; }
        public bool addBrandAccess { get; set; }
        public bool editBrandAccess { get; set; }
        public bool viewBrandAccess { get; set; }
        public bool deleteBrandAccess { get; set; }
        public bool addUserAccess { get; set; }
        public bool hasReportAccess { get; set; }
    }
}
