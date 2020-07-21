
using CMS.BE.ViewModels;
using System.Collections.Generic;

namespace CMS.BL.Interface
{
    public interface ICampaignManager
    {
        List<CampaignViewModel> GetAllCampaigns();
        bool AddCampaign(CampaignViewModel campaignViewModel);

        bool EditCampaign(CampaignViewModel campaignViewModel);

        bool DeleteCampaign(int id);

        CampaignViewModel GetCampaignById(int Id);
        List<CampaignViewModel> GetAllCampaignsForList();

        List<CampaignStatusViewModel> GetAllCampaignStatus(int id);
        List<MarketingStrategyViewModel> GetAllMarketingStrategy();

        List<MarketingTypeViewModel> GetAllMarketingType();
        string GetCampaignStatusById(int id);

        string GetMarketingStrategyById(int id);

        string GetMarketingTypeById(int id);
        int GetLatestCampaignId();

        string EmailPreview(CampaignViewModel campaignViewModel,string templateData, List<int> customerId);

        bool ChangeCampaignStatus(int id, int statusId);
        bool CheckSimilar(CampaignViewModel campaignViewModel);
        bool SendCampaignMailAsync(int campaignId, int templateId);
    }
}
