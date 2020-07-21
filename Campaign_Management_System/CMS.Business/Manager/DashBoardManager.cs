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
    public class DashBoardManager : IDashBoardManager
    {
        private IResponseRepository _iResponseRepository;
        private ICampaignRepository _iCampaignRepository;
        private IBrandRepository _iBrandRepository;

        public DashBoardManager(IResponseRepository iResponseRepository, ICampaignRepository iCampaignRepository, IBrandRepository iBrandRepository)
        {
            _iResponseRepository = iResponseRepository;
            _iCampaignRepository = iCampaignRepository;
            _iBrandRepository = iBrandRepository;
        }
        public DashboardGraphViewModel GetBasicDashBoardDetails()
        {
            DashboardGraphViewModel dashboardGraphViewModel = new DashboardGraphViewModel();
            ResponseRate responseRate = _iResponseRepository.GetTotalResponse();
            dashboardGraphViewModel.SuccessRate = responseRate.SuccessRate;
            dashboardGraphViewModel.SuccessIncrease = responseRate.SuccessIncrease;

            TotalCampaign totalCampaign = _iCampaignRepository.GetCampaignTotal();
            dashboardGraphViewModel.CampaignCount = totalCampaign.CampaignCount;
            dashboardGraphViewModel.CampaignIncrease = totalCampaign.CampaignIncrease;
            dashboardGraphViewModel.ActiveCampaignCount = totalCampaign.activeCampaignCount;
            dashboardGraphViewModel.ActiveCampaignIncrease = totalCampaign.activeCampaignIncrease;

            return dashboardGraphViewModel;
        }

        public IList<CampaignMonthYearViewModel> GetCampaignByMonthOrYear()
        {
            IList<CamapignByMonthYear> camapignByMonthYear = _iCampaignRepository.GetCampaignByMonthOrYear();
            IList<CampaignMonthYearViewModel> camapignByMonthYearViewModels = new List<CampaignMonthYearViewModel>();

            foreach (var item in camapignByMonthYear)
            {
                camapignByMonthYearViewModels.Add(new CampaignMonthYearViewModel
                {
                    CampaignCount = item.CampaignCount,
                    NameOfTheYearOrMonth = item.DateOrMonth
                });
            }
            return camapignByMonthYearViewModels;
        }

        public IList<DashboardBrandViewModel> getTopFiveBrandByBudget()
        {
            IList<BrandBudgetData> brandBudgetData = _iBrandRepository.topFiveBrandBudget();
            IList<DashboardBrandViewModel> dashboardBrandViewModel = new List<DashboardBrandViewModel>();

            var topFive = new List<BrandBudgetData>();
            int count = 5;
            foreach (var item in brandBudgetData)
            {
                topFive.Add(item);
                count--;
                if (count <= 0)
                {
                    break;
                }
            }
            
            foreach (var item in topFive)
            {
                dashboardBrandViewModel.Add(new DashboardBrandViewModel {
                    BrandName=item.BrandName,
                    BrandBudget=item.Budget,
                    BrandCampaignCount = item.countBrand
                });
            }
            return dashboardBrandViewModel;
        }
        public IList<CampaignCountByStatusViewModel> GetCampaignStatusList()
        {
            IList<CampaignStatusForDashboard> campaignStatusList = _iCampaignRepository.GetCampaignStatusForDashboard();
            IList<CampaignCountByStatusViewModel> returnCampaignStatusList = new List<CampaignCountByStatusViewModel>();

            foreach (var item in campaignStatusList)
            {
                returnCampaignStatusList.Add(new CampaignCountByStatusViewModel
                {
                    ActiveCount = item.ActiveCount,
                    CompletedCount=item.CompletedCount,
                    CreatedCount=item.CreatedCount,
                    date=item.date
                });
            }
            return returnCampaignStatusList;
        }

        public IList<ResponseCampaignViewModel> GetTopThreeResponse()
        {
            IList<Response> responses = _iResponseRepository.GetAllResponses();
            IList<ResponseCampaignViewModel> responseCampaignViewModel = new List<ResponseCampaignViewModel>();

            var topThree = new List<Response>();
            int count = 3;
            foreach (var item in responses)
            {
                topThree.Add(item);
                count--;
                if (count <= 0)
                {
                    break;
                }
            }
            foreach (var item in topThree)
            {
                Campaign campaign = _iCampaignRepository.GetCampaignByid(item.CampaignId);
                responseCampaignViewModel.Add(new ResponseCampaignViewModel
                {
                    CampaignName=campaign.CampaignName,
                    Positive=item.Positive,
                    Negative=item.Negative,
                    Neutral=item.Neutral,
                    NoResponse=item.NoResponse
                });
            }
            return responseCampaignViewModel;
        }
        public IList<MarketingStrategyViewModel> getMarketingStatergies()
        {
            IList<MarketingStrategy> marketing = _iCampaignRepository.GetAllMarketingStrategy();
            IList<MarketingStrategyViewModel> marketingStrategies = new List<MarketingStrategyViewModel>();

            foreach (var item in marketing)
            {
                marketingStrategies.Add(new MarketingStrategyViewModel
                {
                    StrategyName = item.StrategyName
                });
            }
            return marketingStrategies;
        }

        
    }
}
