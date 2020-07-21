using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Data.Database;
using CMS.DL.Interface;
using System.Collections.Generic;

namespace CMS.BL.Manager
{
    public class Customer_CampaignManager : ICustomer_CampaignManager
    {
        private ICustomer_CampaignRepository _icustomer_CampaignRepository;

        public Customer_CampaignManager(ICustomer_CampaignRepository customer_CampaignRepository)
        {
            _icustomer_CampaignRepository = customer_CampaignRepository;
        }

        public int AddCustomerResponse(CampaignCustomerResponse customerResponse)
        {
            Customer_Campaign customerR = new Customer_Campaign
            {
                Id = customerResponse.Id,
                CampaignId = customerResponse.CampaignId,
                CustomerID = customerResponse.CustomerID,
                Response = customerResponse.Response
            };
            return _icustomer_CampaignRepository.AddUserResponse(customerR);
        }

        public bool AddToList(int campaignId, List<int> customerIds, string Temp)
        {
            return _icustomer_CampaignRepository.AddToList(campaignId, customerIds, Temp);
        }

        public bool ChangeEmailStatus(int id)
        {
            return _icustomer_CampaignRepository.ChangeEmailStatus(id);
        }

        public List<int> getCustomersIdsListByCampaignId(int id)
        {
            return _icustomer_CampaignRepository.GetCustomerIdsListByCampaignId(id);
        }

        public List<CustomerCampaignViewModel> getCustomersListByCampaignId(int id)
        {
            List<CustomerCampaignViewModel> ccVm = new List<CustomerCampaignViewModel>();
            var cclist = _icustomer_CampaignRepository.GetCustomerListByCampaignId(id);

            foreach (var user in cclist)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Customer_Campaign, CustomerCampaignViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = new Customer_Campaign();
                source = user;
                var dest = mapper.Map<Customer_Campaign, CustomerCampaignViewModel>(source);
                ccVm.Add(dest);
            }
            return ccVm;
        }
    }
}
