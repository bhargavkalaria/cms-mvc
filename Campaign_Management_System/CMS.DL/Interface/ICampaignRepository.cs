using CMS.Data.Database;
using System;
using System.Collections.Generic;


namespace CMS.DL.Interface
{
    public interface ICampaignRepository
    {
        List<Campaign> GetAllCampaigns();
        bool AddCampaign(Campaign campaign);

        bool EditCampaign(Campaign campaign);

        bool DeleteCampaign(int id);

        Campaign GetCampaignByid(int id);

        List<CampaignStatus> GetAllCampaignStatus();

        List<MarketingStrategy> GetAllMarketingStrategy();

        List<MarketingType> GetAllMarketingTypes();
        string GetCampaignStatusById(int id);

        string GetMarketingStrategyById(int id);

        string GetMarketingTypeById(int id);

        int GetLatestCampaignId();

        IList<Campaign> GetCampaignByDate(DateTime startDate, DateTime endDate);

        bool ChangeCampaignStatus(int id, int statusId);
        TotalCampaign GetCampaignTotal();
        IList<CamapignByMonthYear> GetCampaignByMonthOrYear();
        IList<CampaignStatusForDashboard> GetCampaignStatusForDashboard();
        bool CheckSimilar(Campaign campaign);
    }
}
