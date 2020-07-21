using CMS.BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BL.Interface
{
    public interface IResponse_QuickCampaignManager
    {
        Response_QuickCampaignViewModel GetResponseById(int id);
        bool UpdateResponseInDb(Response_QuickCampaignViewModel reponse);
        bool UpdateGivenResponse(string returnedResponse, int id);
        ResponseCampaignViewModel GetReportById(int id);
        IList<ResponseCampaignViewModel> GetReportByDate(DateTime startDate, DateTime endDate);
        IList<ResponseCampaignViewModel> GetReportByType();
    }
}
