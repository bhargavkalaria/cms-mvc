using CMS.BL.Interface;
using CMS.BL.Manager;
using CMS.BL.UnityResolverHelper;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace CMS.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.AddNewExtension<UnityResolverHelper>();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ICampaignManager, CampaignManager>();
            container.RegisterType<IQuickCampaignManager, QuickCampaignManager>();
            container.RegisterType<ILoginManager, LoginManager>();
            container.RegisterType<IDataImportManager, DataImportManager>();
            container.RegisterType<IResponseManager, ResponseManager>();
            container.RegisterType<IEmailMasterManager, EmailMasterManager>();
            container.RegisterType<ICustomer_CampaignManager, Customer_CampaignManager>();
            container.RegisterType<ICustomerManager, CustomerManager>();
            container.RegisterType<IBrandManager, BrandManager>();
            container.RegisterType<ICustomer_QuickCampaignManager, Customer_QuickCampaignManager>();
            container.RegisterType<IResponse_QuickCampaignManager, Response_QuickCampaignManager>();
            container.RegisterType<IRoleManager, RoleManager>();
            container.RegisterType<IDashBoardManager, DashBoardManager>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}