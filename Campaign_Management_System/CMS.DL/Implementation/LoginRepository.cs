using CMS.Data.Database;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMS.DL.Implementation
{
    public class LoginRepository : ILoginRepository
    {
        private CMSContext cmsContext;
        bool status = false;
        public LoginRepository()
        {
            cmsContext = new CMSContext();
        }
        public bool AddUser(User objUser)
        {
            cmsContext.User.Add(objUser);
            int c = cmsContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
        }

        public List<User> GetAllUsers()
        {
            return cmsContext.User.ToList();
        }

        public User GetUserByEmail(string email)
        {
            return cmsContext.User.Where(a => a.Email.Equals(email)).FirstOrDefault();
        }

        public User GetUserByEmailPassword(string email, string password)
        {
            try
            {
                var user = cmsContext.User.Where(a => a.Email.Equals(email) && a.Password.Equals(password)).FirstOrDefault();
                return user;
            }
            catch (Exception e)
            {

                throw;
            }


        }


        public int UpdateUserPassword(User objUser)
        {
            var Lst = GetUserByEmail(objUser.Email);
            Lst.Password = objUser.Password;
            cmsContext.Entry(Lst).State = EntityState.Modified;
            return cmsContext.SaveChanges();
        }
    }
}
