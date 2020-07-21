using CMS.DL.Implementation;
using CMS.DL.Interface;
using Unity;
using Unity.Extension;

namespace CMS.BL.UnityResolverHelper
{
    public class UnityResolverHelper : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<ICampaignRepository, CampaignRepository>();
            Container.RegisterType<IQuickCampaignRepository, QuickCampaignRepository>();
            Container.RegisterType<ILoginRepository, LoginRepository>();
            Container.RegisterType<ICustomerRepository, CustomerRepository>();
            Container.RegisterType<IResponseRepository, ResponseRepository>();
            Container.RegisterType<IEmailMasterRepository, EmailMasterRepository>();
            Container.RegisterType<ICustomer_CampaignRepository, Customer_CampaignRepository>();
            Container.RegisterType<IBrandRepository, BrandRepository>();
            Container.RegisterType<ICustomer_QuickCampaignRepository, Customer_QuickCampaignRepository>();
            Container.RegisterType<IResponse_QuickCampaignRepository, Response_QuickCampaignRepository>();
            Container.RegisterType<IRoleRepository, RoleRepository>();
        }
    }   
}
