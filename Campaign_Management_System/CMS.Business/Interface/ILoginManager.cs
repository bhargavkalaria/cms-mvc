using CMS.BE.ViewModels;
using System.Collections.Generic;

namespace CMS.BL.Interface
{
    public interface ILoginManager
    {
        List<UserViewModel> GetAllUsers();
        bool AddUser(UserViewModel objUser);
        UserViewModel GetUserByEmail(string email);
        UserViewModel GetUserByEmailPassword(string email, string password);
        string UpdateUserPassword(UserViewModel objUser);
        string Hash(string value);
        int ResetPassword(UserViewModel objUser);
        string RandomPassword();
        string RandomString(int size, bool lowerCase);
        int Random(int min, int max);
        bool CheckEmailExist(string email);
        bool CheckEmail(string email);
        bool CheckPassword(string pwd);
        bool CheckName(string name);
        bool ServerSideEmptyValidation(UserViewModel userModel);

    }
}
