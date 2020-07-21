using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.DL.Interface;

namespace CMS.BL.Manager
{
    public class CustomerManager : ICustomerManager
    {
        private ICustomerRepository _iCustomerRepository;


        public CustomerManager(ICustomerRepository iCustomerRepository)
        {
            _iCustomerRepository = iCustomerRepository;
        }

        public CustomerViewModel GetCustomerById(int id)
        {
            var customerEntity = _iCustomerRepository.GetCustomerById(id);
            return new CustomerViewModel
            {
                CustomerID = customerEntity.CustomerID,
                City = customerEntity.City,
                CustomerName = customerEntity.CustomerName,
                Email = customerEntity.Email,
                Mobile = customerEntity.Mobile
            };
        }
    }
}
