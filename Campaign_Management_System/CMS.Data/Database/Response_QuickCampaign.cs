using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Database
{
    public class Response_QuickCampaign
    {
        [Key]
        public int ResponseId { get; set; }
        public int QuickCampaignId { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        public int Neutral { get; set; }
        public int NoResponse { get; set; }
    }
}
