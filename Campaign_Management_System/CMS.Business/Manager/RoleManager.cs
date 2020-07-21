using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Data.Database;
using CMS.DL.Implementation;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BL.Manager
{
    public class RoleManager : IRoleManager
    {
        private IRoleRepository _iRoleRepository;
        
        public RoleManager()
        {
            _iRoleRepository = new RoleRepository();
        }
        public IList<UserViewModel> getAllUsers()
        {
            List<UserViewModel> userViewModel = new List<UserViewModel>();
            var users = _iRoleRepository.GetAllUsers();
            foreach (var user in users)
            {
                if (!user.isDeleted)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<User, UserViewModel>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var source = new User();
                    source = user;
                    var dest = mapper.Map<User, UserViewModel>(source);
                    userViewModel.Add(dest);
                }
            }
            return userViewModel;
        }

        public UserViewModel getUserById(int id)
        {
            User usr = _iRoleRepository.GetUserById(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            var source = usr;
            var dest = mapper.Map<User, UserViewModel>(source);

            return dest;
        }

        public bool updateRole(UserViewModel userModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, User>();
            });

            IMapper mapper = config.CreateMapper();
            var source = userModel;
            var dest = mapper.Map<UserViewModel, User>(source);
            return _iRoleRepository.updateRole(dest);
        }
        public bool hasAddUserAccess(int uid)
        {
            return _iRoleRepository.hasAddUserAccess(uid);
        }
        public bool hasAddBrandAccess(int uid)
        {
            return _iRoleRepository.hasAddBrandAccess(uid);
        }

        public bool hasEditBrandAccess(int uid)
        {
            return _iRoleRepository.hasEditBrandAccess(uid);
        }

        public bool hasDeleteBrandAccess(int uid)
        {
            return _iRoleRepository.hasDeleteBrandAccess(uid);
        }

        public bool hasViewBrandAccess(int uid)
        {
            return _iRoleRepository.hasViewBrandAccess(uid);
        }

        public bool hasAddCampaignAccess(int uid)
        {
            return _iRoleRepository.hasAddCampaignAccess(uid);
        }

        public bool hasEditCampaignAccess(int uid)
        {
            return _iRoleRepository.hasEditCampaignAccess(uid);
        }

        public bool hasDeleteCampaignAccess(int uid)
        {
            return _iRoleRepository.hasDeleteCampaignAccess(uid);
        }

        public bool hasViewCampaignAccess(int uid)
        {
            return _iRoleRepository.hasViewCampaignAccess(uid);
        }

        public bool hasUploadCustomerAccess(int uid)
        {
            return _iRoleRepository.hasUploadCustomerAccess(uid);
        }

        public bool hasAddQuickCampaignAccess(int uid)
        {
            return _iRoleRepository.hasAddQuickCampaignAccess(uid);
        }

        public bool hasViewQuickCampaignAccess(int uid)
        {
            return _iRoleRepository.hasViewQuickCampaignAccess(uid);
        }

        public bool hasAddTemplateAccess(int uid)
        {
            return _iRoleRepository.hasAddTemplateAccess(uid);
        }

        public bool hasViewTemplateAccess(int uid)
        {
            return _iRoleRepository.hasViewTemplateAccess(uid);
        }

        public bool hasEditTemplateAccess(int uid)
        {
            return _iRoleRepository.hasEditTemplateAccess(uid);
        }

        public bool hasDeleteTemplateAccess(int uid)
        {
            return _iRoleRepository.hasDeleteTemplateAccess(uid);
        }

        public bool hasReportAccess(int uid)
        {
            return _iRoleRepository.hasReportAccess(uid);
        }

        public bool hasPrintAccess(int uid)
        {
            return _iRoleRepository.hasPrintAccess(uid);
        }

        public bool DeleteUser(int uid)
        {
            return _iRoleRepository.DeleteUser(uid);
        }
    }
}
