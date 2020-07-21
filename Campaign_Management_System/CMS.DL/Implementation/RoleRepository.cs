using CMS.Data.Database;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private CMSContext cmsContext=new CMSContext();
        
        public IList<User> GetAllUsers()
        {
            return cmsContext.User.ToList();
        }

        public User GetUserById(int id)
        {
            return cmsContext.User.
              Where(x => x.UId == id).FirstOrDefault();
        }

        public bool hasAddUserAccess(int uid)
        {
            return cmsContext.User.Find(uid).addUserAccess;
        }
        public bool hasAddBrandAccess(int uid)
        {
            return cmsContext.User.Find(uid).addBrandAccess;
        }
        public bool updateRole(User user)
        {
            bool status = false;
            User usr = new User();
            usr = GetUserById(user.UId);
            user.Email = usr.Email;
            user.FName = usr.FName;
            user.LName = usr.LName;
            user.Password = usr.Password;
            user.Role = usr.Role;
            var local = cmsContext.Set<User>()
                       .Local
                       .FirstOrDefault(f => f.UId == user.UId);
            if (local != null)
            {
                cmsContext.Entry(local).State = EntityState.Detached;
            }
            cmsContext.Entry(user).State = EntityState.Modified;

            if (cmsContext.SaveChanges() > 0)
            {
                status = true;
            }
            return status;
        }

        public bool hasEditBrandAccess(int uid)
        {
            return cmsContext.User.Find(uid).editBrandAccess;
        }

        public bool hasDeleteBrandAccess(int uid)
        {
            return cmsContext.User.Find(uid).deleteBrandAccess;
        }

        public bool hasViewBrandAccess(int uid)
        {
            return cmsContext.User.Find(uid).viewBrandAccess;
        }

        public bool hasAddCampaignAccess(int uid)
        {
            return cmsContext.User.Find(uid).addCampaignAccess;
        }

        public bool hasEditCampaignAccess(int uid)
        {
            return cmsContext.User.Find(uid).editCampaignAccess;
        }

        public bool hasDeleteCampaignAccess(int uid)
        {
            return cmsContext.User.Find(uid).deleteCampainAccess;
        }

        public bool hasViewCampaignAccess(int uid)
        {
            return cmsContext.User.Find(uid).viewCampaignAccess; 
        }

        public bool hasUploadCustomerAccess(int uid)
        {
            return cmsContext.User.Find(uid).uploadCustomerAccess;
        }

        public bool hasAddQuickCampaignAccess(int uid)
        {
            return cmsContext.User.Find(uid).addQuickCampaignAccess;
        }

        public bool hasViewQuickCampaignAccess(int uid)
        {
            return cmsContext.User.Find(uid).viewQuickCampaignAccess;
        }

        public bool hasAddTemplateAccess(int uid)
        {
            return cmsContext.User.Find(uid).addTemplateAccess;
        }

        public bool hasViewTemplateAccess(int uid)
        {
            return cmsContext.User.Find(uid).viewTemplateAccess;
        }

        public bool hasEditTemplateAccess(int uid)
        {
            return cmsContext.User.Find(uid).editTemplateAccess;
        }

        public bool hasDeleteTemplateAccess(int uid)
        {
            return cmsContext.User.Find(uid).deleteTemplateAccess;
        }

        public bool hasReportAccess(int uid)
        {
            return cmsContext.User.Find(uid).hasReportAccess;
        }

        public bool DeleteUser(int id)
        {
            int c = 0;
            bool status = false;
            User user = new User();

            user = GetUserById(id);
            user.isDeleted = true;
            var local = cmsContext.Set<User>()
                       .Local
                       .FirstOrDefault(f => f.UId == id);
            if (local != null)
            {
                cmsContext.Entry(local).State = EntityState.Detached;
            }
            cmsContext.Entry(user).State = EntityState.Modified;

            c = cmsContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
        }

        public bool hasPrintAccess(int uid)
        {
            return cmsContext.User.Find(uid).hasPrintAccess;
        }
    }
}
