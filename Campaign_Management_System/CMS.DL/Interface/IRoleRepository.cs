using CMS.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Interface
{
    public interface IRoleRepository
    {
        IList<User> GetAllUsers();
        User GetUserById(int id);
        bool updateRole(User user);
        bool hasAddUserAccess(int uid);

        bool hasAddBrandAccess(int uid);
        bool hasEditBrandAccess(int uid);
        bool hasDeleteBrandAccess(int uid);
        bool hasViewBrandAccess(int uid);

        
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
        bool hasPrintAccess(int uid);

        bool DeleteUser(int id);
    }
}
