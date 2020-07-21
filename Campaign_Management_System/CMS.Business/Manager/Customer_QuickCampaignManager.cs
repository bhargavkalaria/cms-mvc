using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Data.Database;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BL.Manager
{
    public class Customer_QuickCampaignManager : ICustomer_QuickCampaignManager
    {
        private ICustomer_QuickCampaignRepository _icustomer_QuickCampaignRepository;
        public Customer_QuickCampaignManager(ICustomer_QuickCampaignRepository customer_QuickCampaignRepository)
        {
            _icustomer_QuickCampaignRepository = customer_QuickCampaignRepository;

        }
        public int AddCustomerResponse(Customer_QuickCampaignViewModel customerResponse)
        {
            Customer_QuickCampaign customerR = new Customer_QuickCampaign
            {
                Id = customerResponse.Id,
                QuickCampaignId = customerResponse.QuickCampaignId,
                CustomerID = customerResponse.CustomerID,
                Response = customerResponse.Response
            };
            return _icustomer_QuickCampaignRepository.AddUserResponse(customerR);
        }

        public bool ChangeEmailStatus(int qid, int customerId)
        {
            return _icustomer_QuickCampaignRepository.ChangeEmailStatus(qid,customerId);
        }

        public bool AddToList(int quickcampaignId, List<int> customerIds, string Temp)
        {
            return _icustomer_QuickCampaignRepository.AddToList(quickcampaignId, customerIds, Temp);
        }

        public List<int> getCustomersIdsListByQuickCampaignId(int id)
        {
            return _icustomer_QuickCampaignRepository.GetCustomerIdsListByQuickCampaignId(id);
        }

        public List<Customer_QuickCampaignViewModel> getCustomersListByQuickCampaignId(int id)
        {
            List<Customer_QuickCampaignViewModel> cqVm = new List<Customer_QuickCampaignViewModel>();
            var cclist = _icustomer_QuickCampaignRepository.GetCustomerListByQuickCampaignId(id);

            foreach (var user in cclist)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Customer_QuickCampaign, Customer_QuickCampaignViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = new Customer_QuickCampaign();
                source = user;
                var dest = mapper.Map<Customer_QuickCampaign, Customer_QuickCampaignViewModel>(source);
                cqVm.Add(dest);
            }
            return cqVm;
        }
    }
}
