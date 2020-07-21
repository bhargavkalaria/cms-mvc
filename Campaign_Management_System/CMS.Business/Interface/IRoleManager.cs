using CMS.BE.ViewModels;
using System.Collections.Generic;
namespace CMS.BL.Interface
{
    public interface IRoleManager
    {
        IList<UserViewModel> getAllUsers();
        UserViewModel getUserById(int id);
        bool updateRole(UserViewModel userModel);

        bool hasAddUserAccess(int uid);

        bool hasAddBrandAccess(int uid);
        bool hasEditBrandAccess(int uid);
        bool hasDeleteBrandAccess(int uid);
        bool hasViewBrandAccess(int uid);
        bool DeleteUser(int uid);

        bool hasPrintAccess(int uid);
        bool hasAddCampaignAccess(int uid);
        bool hasEditCampaignAccess(int uid);
        bool hasDeleteCampaignAccess(int uid);
        bool hasViewCampaignAccess(int uid);

        bool hasUploadCustomerAccess(int uid);

        bool hasAddQuickCampaignAccess(int uid);
        bool hasViewQuickCampaignAccess(int uid);

        bool hasAddTemplateAccess(int uid);
        bool hasViewTemplateAccess(int uid);
        bool hasEditTemplateAccess(int uid);
        bool hasDeleteTemplateAccess(int uid);

        bool hasReportAccess(int uid);
    }
}
