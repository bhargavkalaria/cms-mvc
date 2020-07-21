using CMS.BE.ViewModels;
using System.Collections.Generic;

namespace CMS.BL.Interface
{
    public interface ICustomer_CampaignManager
    {
        bool AddToList(int campaignId, List<int> customerIds, string Temp);
        List<CustomerCampaignViewModel> getCustomersListByCampaignId(int id);
        List<int> getCustomersIdsListByCampaignId(int id);
        int AddCustomerResponse(CampaignCustomerResponse customerResponse);
        bool ChangeEmailStatus(int id);
    }
}
