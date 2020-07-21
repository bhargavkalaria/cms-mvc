using CMS.Data.Database;
using System;
using System.Collections.Generic;

namespace CMS.DL.Interface
{
    public interface IResponseRepository
    {
        Response GetResponseDetailsById(int id);
        bool UpdateResponse(Response response);
        Response GetResponseDetailsByCampaignId(int id);
        bool AddResponseDefault(int id);
        IList<Response> GetResponseByDate(DateTime startDate, DateTime endDate);
        ResponseRate GetTotalResponse();
        IList<Response> GetAllResponses();
    }
}
