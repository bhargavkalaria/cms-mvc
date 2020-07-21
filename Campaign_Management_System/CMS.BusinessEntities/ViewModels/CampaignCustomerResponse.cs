namespace CMS.BE.ViewModels
{
    public class CampaignCustomerResponse
    {
        public int Id { get; set; }
        public int CustomerID { get; set; }

        public int CampaignId { get; set; }
        public string Response { get; set; }
    }
}
