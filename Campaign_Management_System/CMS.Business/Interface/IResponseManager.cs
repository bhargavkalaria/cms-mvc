using CMS.BE.ViewModels;
using System;
using System.Collections.Generic;

namespace CMS.BL.Interface
{
    public interface IResponseManager
    {
        ResponseVIewModel GetResponseById(int id);
        bool UpdateResponseInDb(ResponseVIewModel reponse);
        bool UpdateGivenResponse(string returnedResponse, int id);
        IList<ResponseCampaignViewModel> GetReportByDate(DateTime startDate, DateTime endDate);
        ResponseCampaignViewModel GetReportById(int id);
        IList<ResponseCampaignViewModel> GetReportByType();
    }
}
