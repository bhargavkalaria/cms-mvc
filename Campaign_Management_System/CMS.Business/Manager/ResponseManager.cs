using System;
using System.Collections.Generic;
using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Data.Database;
using CMS.DL.Interface;

namespace CMS.BL.Manager
{
    public class ResponseManager : IResponseManager
    {
        IResponseRepository _iResponseRepository;
        ICampaignRepository _iCampaignRepository;
        public ResponseManager(IResponseRepository iResponseRepository,ICampaignRepository iCampaignRepository)
        {
            _iResponseRepository = iResponseRepository;
            _iCampaignRepository = iCampaignRepository;
        }

        public IList<ResponseCampaignViewModel> GetReportByDate(DateTime startDate, DateTime endDate)
        {
            List<ResponseCampaignViewModel> responses = new List<ResponseCampaignViewModel>();
            IList<Campaign> campaigns = _iCampaignRepository.GetCampaignByDate(startDate, endDate);
            foreach (var campaign in campaigns)
            {
                Response response = _iResponseRepository.GetResponseDetailsById(campaign.CampaignId);
                string RemainingTime = "Problem In Finding Time";
                if (DateTime.Now > campaign.Start_Date && DateTime.Now < campaign.End_Date)
                {
                    RemainingTime = "Days Remaining Before Ending Campaign Is :" + (campaign.End_Date - DateTime.Now).Days;
                }
                else if (DateTime.Now < campaign.Start_Date)
                {
                    RemainingTime = "Days Remaining Is Starting Campaign Is :" + (campaign.Start_Date - DateTime.Now).Days;
                }
                else if (DateTime.Now > campaign.End_Date)
                {
                    RemainingTime = "Days Happened After Campaign Is Ended Is :" + (System.DateTime.Now - campaign.End_Date).Days;
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

                responses.Add(new ResponseCampaignViewModel {
                    ResponseId = response.ResponseId,
                    CampaignId = response.CampaignId,
                    Positive = response.Positive,
                    Negative = response.Negative,
                    Neutral = response.Neutral,
                    NoResponse = response.NoResponse,
                    CampaignBudget = campaign.CampaignBudget,
                    CampaignName = campaign.CampaignName,
                    DaysRemaining = RemainingTime,
                    percentageFor = positivePercentage,
                    successOrNot = status,
                    type = "Campaign"
                });
            }
            return responses;
        }

        public ResponseCampaignViewModel GetReportById(int id)
        {
            ResponseCampaignViewModel responseCampaign = new ResponseCampaignViewModel();
             Campaign campaign = _iCampaignRepository.GetCampaignByid(id);
                Response response = _iResponseRepository.GetResponseDetailsById(id);
                responseCampaign.CampaignId = campaign.CampaignId;
                responseCampaign.CampaignName = campaign.CampaignName;
                responseCampaign.CampaignBudget = campaign.CampaignBudget;
                responseCampaign.ResponseId = response.CampaignId;
                responseCampaign.Negative = response.Negative;
                responseCampaign.Positive = response.Positive;
                responseCampaign.Neutral = response.Neutral;
                responseCampaign.NoResponse = response.NoResponse;
                responseCampaign.type = "Campaign";

                string RemainingTime = "Problem In Finding Time";
                if (DateTime.Now > campaign.Start_Date && DateTime.Now < campaign.End_Date)
                {
                    RemainingTime = "Days Remaining Before Ending Campaign Is :" + (campaign.End_Date - DateTime.Now).Days;
                }
                else if (DateTime.Now < campaign.Start_Date)
                {
                    RemainingTime = "Days Remaining Is Starting Campaign Is :" + (campaign.Start_Date - DateTime.Now).Days;
                }
                else if (DateTime.Now > campaign.End_Date)
                {
                    RemainingTime = "Days Happened After Campaign Is Ended Is :" + (System.DateTime.Now - campaign.End_Date).Days;
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

        public ResponseVIewModel GetResponseById(int id)
        {
            var responses = _iResponseRepository.GetResponseDetailsByCampaignId(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Response, ResponseVIewModel>().
                    ForMember(d => d.Campaign, p => p.MapFrom(s => s.Campaign));
                cfg.CreateMap<Campaign, CampaignViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var source = responses;
            var dest = mapper.Map<Response, ResponseVIewModel>(source);
            return dest;
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

        public bool UpdateResponseInDb(ResponseVIewModel response)
        {
            var res = new Response
            {
                ResponseId = response.ResponseId,
                CampaignId = response.CampaignId,
                Positive = response.Positive,
                Negative = response.Negative,
                Neutral = response.Neutral,
                NoResponse = response.NoResponse
            };
            if (_iResponseRepository.UpdateResponse(res))
                return true;
            else
                return false;
        }

        public IList<ResponseCampaignViewModel> GetReportByType()
        {
            List<ResponseCampaignViewModel> responses = new List<ResponseCampaignViewModel>();
            IList<Campaign> campaigns = _iCampaignRepository.GetAllCampaigns();
            foreach (var campaign in campaigns)
            {
                Response response = _iResponseRepository.GetResponseDetailsById(campaign.CampaignId);
                string RemainingTime = "Problem In Finding Time";
                if (DateTime.Now > campaign.Start_Date && DateTime.Now < campaign.End_Date)
                {
                    RemainingTime = "Days Remaining Before Ending Campaign Is :" + (campaign.End_Date - DateTime.Now).Days;
                }
                else if (DateTime.Now < campaign.Start_Date)
                {
                    RemainingTime = "Days Remaining Is Starting Campaign Is :" + (campaign.Start_Date - DateTime.Now).Days;
                }
                else if (DateTime.Now > campaign.End_Date)
                {
                    RemainingTime = "Days Happened After Campaign Is Ended Is :" + (System.DateTime.Now - campaign.End_Date).Days;
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
                    CampaignId = response.CampaignId,
                    Positive = response.Positive,
                    Negative = response.Negative,
                    Neutral = response.Neutral,
                    NoResponse = response.NoResponse,
                    CampaignBudget = campaign.CampaignBudget,
                    CampaignName = campaign.CampaignName,
                    DaysRemaining = RemainingTime,
                    percentageFor = positivePercentage,
                    successOrNot = status,
                    type = "Campaign"
                });
            }
            return responses;
        }
    }
}
