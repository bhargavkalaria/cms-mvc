using CMS.BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BL.Interface
{
    public interface ICustomer_QuickCampaignManager
    {
        bool AddToList(int quickcampaignId, List<int> customerIds, string Temp);
        List<Customer_QuickCampaignViewModel> getCustomersListByQuickCampaignId(int id);
        List<int> getCustomersIdsListByQuickCampaignId(int id);
        int AddCustomerResponse(Customer_QuickCampaignViewModel customerResponse);
        bool ChangeEmailStatus(int qid, int customerId);
    }
}
