using CMS.Data.Database;
using System.Collections.Generic;
namespace CMS.DL.Interface
{
    public interface ICustomerRepository
    {
        bool ReadAndSaveExcel(Customer customer);

        List<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
    }
}
