using CMS.Data.Database;
using System.Collections.Generic;

namespace CMS.DL.Interface
{
    public interface ILoginRepository
    {
        List<User> GetAllUsers();
        bool AddUser(User objUser);
        User GetUserByEmail(string email);
        User GetUserByEmailPassword(string email, string password);
        int UpdateUserPassword(User objUser);

    }
}
