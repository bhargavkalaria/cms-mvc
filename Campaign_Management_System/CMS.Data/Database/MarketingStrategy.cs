using System.ComponentModel.DataAnnotations;


namespace CMS.Data.Database
{
    public class MarketingStrategy
    {
        [Key]
        public int MarketingStrategyId { get; set; }
        public string StrategyName { get; set; }

    }
}
