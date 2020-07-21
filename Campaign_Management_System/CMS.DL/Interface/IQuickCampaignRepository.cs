using CMS.Data.Database;
using System;
using System.Collections.Generic;

namespace CMS.DL.Interface
{
    public interface IQuickCampaignRepository
    {
        List<QuickCampaign> GetAllQuickCampaigns();
        bool AddQuickCampaign(QuickCampaign quickModel);
        int GetLatestQuickCampaignId();
        bool AddToList(int quickcampaignId, List<int> customerIds);
        List<int> GetCustomerIdsListByQuickCampaignId(int id);
        QuickCampaign GetQuickCampaignById(int id);
        IList<QuickCampaign> GetCampaignByDate(DateTime startDate, DateTime endDate);
        bool CheckSimilar(QuickCampaign quickCampaign);

    }
}
