using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Database
{
    public class User
    {
        [Key]
        public int UId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
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
        public bool hasPrintAccess { get; set; }
        public bool isDeleted { get; set; }
    }
}
