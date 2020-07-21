using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Database
{
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }
        public int CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        public int Neutral { get; set; }
        public int NoResponse { get; set; }
        public virtual ICollection<Campaign> campaigns { get; set; }

    }
}
