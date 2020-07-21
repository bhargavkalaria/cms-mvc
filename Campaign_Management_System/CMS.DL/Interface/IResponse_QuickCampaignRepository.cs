using CMS.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Interface
{
    public interface IResponse_QuickCampaignRepository
    {
        Response_QuickCampaign GetResponseQuickCampaignById(int id);
        bool UpdateResponse(Response_QuickCampaign response);
        Response_QuickCampaign GetResponseDetailsByQuickCampaignId(int id);
        bool AddResponseDefault(int id);
    }
}
