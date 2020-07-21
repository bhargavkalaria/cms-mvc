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
    public class Response_QuickCampaignManager : IResponse_QuickCampaignManager
    {
        IResponse_QuickCampaignRepository _iresponse_QuickCampaignRepository;
        IQuickCampaignRepository _iQuickCampaignRepository;
        public Response_QuickCampaignManager(IResponse_QuickCampaignRepository response_QuickCampaignRepository,IQuickCampaignRepository iQuickCampaignRepository)
        {
            _iresponse_QuickCampaignRepository = response_QuickCampaignRepository;
            _iQuickCampaignRepository = iQuickCampaignRepository;
        }
        public ResponseCampaignViewModel GetReportById(int id)
        {
            ResponseCampaignViewModel responseCampaign = new ResponseCampaignViewModel();
            
                QuickCampaign campaign = _iQuickCampaignRepository.GetQuickCampaignById(id);
                Response_QuickCampaign response = _iresponse_QuickCampaignRepository.GetResponseDetailsByQuickCampaignId(id);
                responseCampaign.CampaignId = campaign.QuickCampaignId;
                responseCampaign.CampaignName = campaign.QuickCampaignName;
                responseCampaign.CampaignBudget = campaign.CampaignBudget;
                responseCampaign.ResponseId = response.QuickCampaignId;
                responseCampaign.Negative = response.Negative;
                responseCampaign.Positive = response.Positive;
                responseCampaign.Neutral = response.Neutral;
                responseCampaign.NoResponse = response.NoResponse;
                responseCampaign.type = "QuickCampaign";
                string RemainingTime = "Problem In Finding Time";
                if (DateTime.Now > campaign.Start_Date)
                {
                    RemainingTime = "Days Happende After Starting Campaign Is :" + (DateTime.Now-campaign.Start_Date).Days;
                }
                else if (DateTime.Now < campaign.Start_Date)
                {
                    RemainingTime = "Days Remaining Is Starting Campaign Is :" + (campaign.Start_Date - DateTime.Now).Days;
                }
                

                double positiveResponses = response.Positive;
                double totalResponses = response.Positive + response.Negative + response.Neutral + response.NoResponse;
                double positivePercentage = 0;
                if (positiveResponses > 0)
                {
                    positivePercentage = (positiveResponses / totalResponses) * 100;
                    positivePercentage = Math.Round(positivePercentage, 2);
                }
                bool status = false;
                if (positivePercentage > 50)
                    status = true;

                responseCampaign.percentageFor = positivePercentage;
                responseCampaign.DaysRemaining = RemainingTime;
                responseCampaign.successOrNot = status;
            

            return responseCampaign;
        }
        public Response_QuickCampaignViewModel GetResponseById(int id)
        {
            var responses = _iresponse_QuickCampaignRepository.GetResponseDetailsByQuickCampaignId(id);
            return new Response_QuickCampaignViewModel
            {
                ResponseId = responses.ResponseId,
                QuickCampaignId = responses.QuickCampaignId,
                Positive = responses.Positive,
                Negative = responses.Negative,
                Neutral = responses.Neutral,
                NoResponse = responses.NoResponse
            };
        }

        public bool UpdateGivenResponse(string returnedResponse, int id)
        {
            var currentResponse = GetResponseById(id);
            bool updateStatus = false;
            if (returnedResponse == "Positive")
            {
                currentResponse.Positive = currentResponse.Positive + 1;
                currentResponse.NoResponse = currentResponse.NoResponse - 1;
                updateStatus = UpdateResponseInDb(currentResponse);
            }
            else if (returnedResponse == "Negative")
            {
                currentResponse.Negative = currentResponse.Negative + 1;
                currentResponse.NoResponse = currentResponse.NoResponse - 1;
                updateStatus = UpdateResponseInDb(currentResponse);
            }
            else if (returnedResponse == "Neutral")
            {
                currentResponse.Neutral = currentResponse.Neutral + 1;
                currentResponse.NoResponse = currentResponse.NoResponse - 1;
                updateStatus = UpdateResponseInDb(currentResponse);
            }
            return updateStatus;
        }

        public bool UpdateResponseInDb(Response_QuickCampaignViewModel response)
        {
            var res = new Response_QuickCampaign
            {
                ResponseId = response.ResponseId,
                QuickCampaignId = response.QuickCampaignId,
                Positive = response.Positive,
                Negative = response.Negative,
                Neutral = response.Neutral,
                NoResponse = response.NoResponse
            };
            if (_iresponse_QuickCampaignRepository.UpdateResponse(res))
                return true;
            else
                return false;
        }

        public IList<ResponseCampaignViewModel> GetReportByDate(DateTime startDate, DateTime endDate)
        {
            List<ResponseCampaignViewModel> responses = new List<ResponseCampaignViewModel>();
            IList<QuickCampaign> campaigns = _iQuickCampaignRepository.GetCampaignByDate(startDate, endDate);
            foreach (var campaign in campaigns)
            {
                Response_QuickCampaign response = _iresponse_QuickCampaignRepository.GetResponseDetailsByQuickCampaignId(campaign.QuickCampaignId);
                string RemainingTime = "Problem In Finding Time";
                if (DateTime.Now > campaign.Start_Date)
                {
                    RemainingTime = "Days Happende After Starting Campaign Is :" + (DateTime.Now - campaign.Start_Date).Days;
                }
                else if (DateTime.Now < campaign.Start_Date)
                {
                    RemainingTime = "Days Remaining Is Starting Campaign Is :" + (campaign.Start_Date - DateTime.Now).Days;
                }


                double positiveResponses = response.Positive;
                double totalResponses = response.Positive + response.Negative + response.Neutral + response.NoResponse;
                double positivePercentage = 0;
                if (positiveResponses > 0)
                {
                    positivePercentage = (positiveResponses / totalResponses) * 100;
                    positivePercentage = Math.Round(positivePercentage, 2);
                }
                bool status = false;
                if (positivePercentage > 50)
                    status = true;

                responses.Add(new ResponseCampaignViewModel
                {
                    ResponseId = response.ResponseId,
                    CampaignId = response.QuickCampaignId,
                    Positive = response.Positive,
                    Negative = response.Negative,
                    Neutral = response.Neutral,
                    NoResponse = response.NoResponse,
                    CampaignBudget = campaign.CampaignBudget,
                    CampaignName = campaign.QuickCampaignName,
                    DaysRemaining = RemainingTime,
                    percentageFor = positivePercentage,
                    successOrNot = status,
                    type = "QuickCampaign"
                });
            }
            return responses;
        }




        public IList<ResponseCampaignViewModel> GetReportByType()
        {
            List<ResponseCampaignViewModel> responses = new List<ResponseCampaignViewModel>();
            IList<QuickCampaign> campaigns = _iQuickCampaignRepository.GetAllQuickCampaigns();
            foreach (var campaign in campaigns)
            {
                Response_QuickCampaign response = _iresponse_QuickCampaignRepository.GetResponseDetailsByQuickCampaignId(campaign.QuickCampaignId);
                string RemainingTime = "Problem In Finding Time";
                if (DateTime.Now > campaign.Start_Date)
                {
                    RemainingTime = "Days Happende After Starting Campaign Is :" + (DateTime.Now - campaign.Start_Date).Days;
                }
                else if (DateTime.Now < campaign.Start_Date)
                {
                    RemainingTime = "Days Remaining Is Starting Campaign Is :" + (campaign.Start_Date - DateTime.Now).Days;
                }


                double positiveResponses = response.Positive;
                double totalResponses = response.Positive + response.Negative + response.Neutral + response.NoResponse;
                double positivePercentage = 0;
                if (positiveResponses > 0)
                {
                    positivePercentage = (positiveResponses / totalResponses) * 100;
                    positivePercentage = Math.Round(positivePercentage, 2);
                }
                bool status = false;
                if (positivePercentage > 50)
                    status = true;

                responses.Add(new ResponseCampaignViewModel
                {
                    ResponseId = response.ResponseId,
                    CampaignId = response.QuickCampaignId,
                    Positive = response.Positive,
                    Negative = response.Negative,
                    Neutral = response.Neutral,
                    NoResponse = response.NoResponse,
                    CampaignBudget = campaign.CampaignBudget,
                    CampaignName = campaign.QuickCampaignName,
                    DaysRemaining = RemainingTime,
                    percentageFor = positivePercentage,
                    successOrNot = status,
                    type = "QuickCampaign"
                });
            }
            return responses;
        }
    }
}
