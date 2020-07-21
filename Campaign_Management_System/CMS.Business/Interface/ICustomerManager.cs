using CMS.BE.ViewModels;

namespace CMS.BL.Interface
{
    public interface ICustomerManager
    {
        CustomerViewModel GetCustomerById(int id);
    }
}
