using CMS.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Interface
{
    public interface ICustomer_QuickCampaignRepository
    {
        bool AddToList(int quickcampaignId, List<int> customerIds, string Temp);

        List<Customer_QuickCampaign> GetCustomerListByQuickCampaignId(int id);
        List<int> GetCustomerIdsListByQuickCampaignId(int id);
        int AddUserResponse(Customer_QuickCampaign customerResponse);
        bool ChangeEmailStatus(int qid, int customerId);
    }
}
