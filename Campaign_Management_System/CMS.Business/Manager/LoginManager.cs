using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Data.Database;
using CMS.DL.Implementation;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace CMS.BL.Manager
{
    public class LoginManager : ILoginManager
    {
        private ILoginRepository _iLoginRepository;
        public LoginManager()
        {
            _iLoginRepository = new LoginRepository();
        }
        public bool AddUser(UserViewModel objUser)
        {
            if (objUser.Role.Equals("ADMIN"))
            {
                objUser.addBrandAccess = true;
                objUser.addCampaignAccess = true;
                objUser.addQuickCampaignAccess = true;
                objUser.addTemplateAccess = true;
                objUser.addUserAccess = true;
                objUser.deleteBrandAccess = true;
                objUser.deleteCampainAccess = true;
                objUser.deleteTemplateAccess = true;
                objUser.editBrandAccess = true;
                objUser.editCampaignAccess = true;
                objUser.editTemplateAccess = true;
                objUser.hasReportAccess = true;
                objUser.uploadCustomerAccess = true;
                objUser.viewBrandAccess = true;
                objUser.viewCampaignAccess = true;
                objUser.viewQuickCampaignAccess = true;
                objUser.viewTemplateAccess = true;
            }
            else if (objUser.Role.Equals("MANAGEMENT"))
            {
                objUser.addBrandAccess = true;
                objUser.addCampaignAccess = true;
                objUser.addQuickCampaignAccess = true;
                objUser.addUserAccess = true;
                objUser.deleteCampainAccess = true;
                objUser.editCampaignAccess = true;
                objUser.uploadCustomerAccess = true;
                objUser.viewCampaignAccess = true;
                objUser.viewQuickCampaignAccess = true;
            }
            else if (objUser.Role.Equals("MARKETING"))
            {
                objUser.hasReportAccess = true;
                objUser.uploadCustomerAccess = true;
            }
            else if (objUser.Role.Equals("DATA-ENTRY USER"))
            {

                objUser.addBrandAccess = true;
                objUser.addCampaignAccess = true;
                objUser.addQuickCampaignAccess = true;
                objUser.addTemplateAccess = true;
                objUser.addUserAccess = true;
                objUser.deleteBrandAccess = true;
                objUser.deleteCampainAccess = true;
                objUser.deleteTemplateAccess = true;
                objUser.editBrandAccess = true;
                objUser.editCampaignAccess = true;
                objUser.editTemplateAccess = true;
                objUser.uploadCustomerAccess = true;
                objUser.viewBrandAccess = true;
                objUser.viewCampaignAccess = true;
                objUser.viewQuickCampaignAccess = true;
                objUser.viewTemplateAccess = true;
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, User>();
            });

            IMapper mapper = config.CreateMapper();
            var source = objUser;
            var dest = mapper.Map<UserViewModel, User>(source);
            dest.Password = Hash(dest.Password);
            return _iLoginRepository.AddUser(dest);
        }

        public List<UserViewModel> GetAllUsers()
        {
            List<UserViewModel> userViewModel = new List<UserViewModel>();
            var users = _iLoginRepository.GetAllUsers();
            foreach (var user in users)
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
            return userViewModel;
        }

        public UserViewModel GetUserByEmail(string email)
        {
            var userList = _iLoginRepository.GetUserByEmail(email);
            return new UserViewModel
            {
                Email = userList.Email,
                Password = userList.Password
            };
        }

        public UserViewModel GetUserByEmailPassword(string email, string password)
        {
            string encPassword = Hash(password);
            var userList = _iLoginRepository.GetUserByEmailPassword(email, encPassword);
            UserViewModel objUser = new UserViewModel();
            if (userList != null)
            {
                objUser = new UserViewModel
                {
                    UId = userList.UId,
                    Email = userList.Email,
                    Password = encPassword,
                    FName = userList.FName,
                    LName = userList.LName,
                    Role = userList.Role,
                    viewCampaignAccess = userList.viewCampaignAccess,
                    addCampaignAccess = userList.addCampaignAccess,
                    editCampaignAccess = userList.editCampaignAccess,
                    deleteCampainAccess = userList.deleteCampainAccess,
                    uploadCustomerAccess = userList.uploadCustomerAccess,
                    addQuickCampaignAccess = userList.addQuickCampaignAccess,
                    viewQuickCampaignAccess = userList.viewQuickCampaignAccess,
                    addTemplateAccess = userList.addTemplateAccess,
                    viewTemplateAccess = userList.viewTemplateAccess,
                    editTemplateAccess = userList.editTemplateAccess,
                    deleteTemplateAccess = userList.deleteTemplateAccess,
                    addBrandAccess = userList.addBrandAccess,
                    editBrandAccess = userList.editBrandAccess,
                    viewBrandAccess = userList.viewBrandAccess,
                    deleteBrandAccess = userList.deleteBrandAccess,
                    addUserAccess = userList.addUserAccess,
                    hasReportAccess = userList.hasReportAccess,
                };
            }
            return objUser;
        }

        public string UpdateUserPassword(UserViewModel objUser)
        {
            string pwd = RandomPassword();
            string encPassword = Hash(pwd);
            User userList = new User
            {
                UId = objUser.UId,
                Email = objUser.Email,
                Password = encPassword,
                FName = objUser.FName,
                LName = objUser.LName
            };
            _iLoginRepository.UpdateUserPassword(userList);
            return pwd;
        }


        public int ResetPassword(UserViewModel objUser)
        {
            string encPassword = Hash(objUser.Password);
            User userList = new User
            {
                UId = objUser.UId,
                Email = objUser.Email,
                Password = encPassword,
                FName = objUser.FName,
                LName = objUser.LName
            };
            return _iLoginRepository.UpdateUserPassword(userList);
        }



        //Functions
        public string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(Random(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public int Random(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public bool CheckEmailExist(string email)
        {
            var user = _iLoginRepository.GetUserByEmail(email);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
          
        }
        public bool CheckEmail(string email)
        {
            string emailRegex = @"(^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$)";

            Regex re = new Regex(emailRegex);


            if (re.IsMatch(email))
                return (true);
            else
                return (false);
        }
        public bool CheckPassword(string pwd)
        {
            string pwdRegex = @"(^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$)";

            Regex re = new Regex(pwdRegex);


            if (re.IsMatch(pwd))
                return (true);
            else
                return (false);
        }
        public bool CheckName(string name)
        {
            string nameRegex = @"(^^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$)";

            Regex re = new Regex(nameRegex);
            if (re.IsMatch(name))
                return (true);
            else
                return (false);
        }

        public bool ServerSideEmptyValidation(UserViewModel userModel)
        {
            if (userModel.FName == null || userModel.LName == null || userModel.Email == null || userModel.Password == null || userModel.Role == null)
            {
                return true;
            }
            return false;
        }

    }
}
