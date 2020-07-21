using CMS.Data.Database;
using CMS.DL.Interface;
using System.Collections.Generic;
using System.Linq;
namespace CMS.DL.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        private CMSContext cmsContext;
        bool status = false;
        public CustomerRepository()
        {
            cmsContext = new CMSContext();
        }

        public List<Customer> GetAllCustomers()
        {
            return cmsContext.Customers.GroupBy(x => x.CustomerName).Select(x => x.FirstOrDefault()).ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return cmsContext.Customers.Find(id);
        }

        public bool ReadAndSaveExcel(Customer customer)
        {
            cmsContext.Customers.Add(customer);
            cmsContext.SaveChanges();
            if (cmsContext.SaveChanges() > 0)
            {
                status = true;
            }
            return status;
        }
    }
}
