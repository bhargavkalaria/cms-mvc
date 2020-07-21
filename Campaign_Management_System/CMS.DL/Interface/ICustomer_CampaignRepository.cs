using CMS.Data.Database;
using System.Collections.Generic;

namespace CMS.DL.Interface
{
    public interface ICustomer_CampaignRepository
    {
        bool AddToList(int campaignId, List<int> customerIds, string Temp);

        List<Customer_Campaign> GetCustomerListByCampaignId(int id);
        List<int> GetCustomerIdsListByCampaignId(int id);
        int AddUserResponse(Customer_Campaign customerResponse);
        bool ChangeEmailStatus(int id);
        Customer_Campaign getCustomerByCampaignIdAndByCustomerId(int CampaignId, int CustomerId);
    }
}
