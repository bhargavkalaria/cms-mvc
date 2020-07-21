using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using CMS.Data.Database;
using CMS.DL.Implementation;
using CMS.DL.Interface;
using System.Collections.Generic;
using System.Linq;

namespace CMS.BL.Manager
{
    public class CampaignManager : ICampaignManager
    {
        private ICampaignRepository _icampaignRepository;
        private ICustomerRepository _icustomerRepository;
        private ICustomer_CampaignRepository _icustomer_CampaignRepository;
        private IEmailMasterRepository _iemailMasterRepository;
        private Constant constant = new Constant();
        SendEmail se = new SendEmail();

        public CampaignManager(ICampaignRepository campaignRepository,ICustomerRepository customerRepository, 
            ICustomer_CampaignRepository customer_CampaignRepository, IEmailMasterRepository emailMasterRepository)
        {
            _icampaignRepository = campaignRepository;
            _icustomerRepository = customerRepository;
            _icustomer_CampaignRepository = customer_CampaignRepository;
            _iemailMasterRepository = emailMasterRepository;
        }

        public bool AddCampaign(CampaignViewModel campaignViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CampaignViewModel, Campaign>();
            });

            IMapper mapper = config.CreateMapper();
            var source = campaignViewModel;
            var dest = mapper.Map<CampaignViewModel, Campaign>(source);
            return _icampaignRepository.AddCampaign(dest);
        }
        public bool CheckSimilar(CampaignViewModel campaignViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CampaignViewModel, Campaign>();
            });

            IMapper mapper = config.CreateMapper();
            var source = campaignViewModel;
            var dest = mapper.Map<CampaignViewModel, Campaign>(source);
            return _icampaignRepository.CheckSimilar(dest);
        }
        public bool ChangeCampaignStatus(int id, int statusId)
        {
            return _icampaignRepository.ChangeCampaignStatus(id, statusId);
        }

        public bool DeleteCampaign(int id)
        {

            return _icampaignRepository.DeleteCampaign(id);
        }

        public bool EditCampaign(CampaignViewModel campaignViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CampaignViewModel, Campaign>();
            });

            IMapper mapper = config.CreateMapper();
            var source = campaignViewModel;
            var dest = mapper.Map<CampaignViewModel, Campaign>(source);
            return _icampaignRepository.EditCampaign(dest);
        }

        public string EmailPreview(CampaignViewModel campaignViewModel, string templateData, List<int> customerIds)
        {
            string template = templateData;
            int customerId = customerIds.First();
            var customer = _icustomerRepository.GetCustomerById(customerId);
            template = template.Replace("[User]", customer.CustomerName);
            template = template.Replace("[Date]", campaignViewModel.Start_Date.Date.ToString("dd/MM/yyyy"));
            template = template.Replace("[Day]", campaignViewModel.Start_Date.DayOfWeek.ToString());
            template = template.Replace("[year]", campaignViewModel.Start_Date.Year.ToString());
            template = template.Replace("[CampaignName]", campaignViewModel.CampaignName);
            return template;
        }
        public List<CampaignViewModel> GetAllCampaignsForList()
        {
            List<CampaignViewModel> campaignViewModel = new List<CampaignViewModel>();
            var campaigns = _icampaignRepository.GetAllCampaigns();
            foreach (var user in campaigns)
            {
                if (!user.isDeleted)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Campaign, CampaignViewModel>().
                        ForMember(d => d.Brand, p => p.MapFrom(s => s.Brand)).
                         ForMember(d => d.Template, p => p.MapFrom(s => s.Template)).
                          ForMember(d => d.MarketingType, p => p.MapFrom(s => s.MarketingType)).
                           ForMember(d => d.MarketingStrategy, p => p.MapFrom(s => s.MarketingStrategy)).
                            ForMember(d => d.CampaignStatus, p => p.MapFrom(s => s.CampaignStatus)).
                            ForMember(d => d.User, p => p.MapFrom(s => s.User));
                        cfg.CreateMap<Brand, BrandViewModel>();
                        cfg.CreateMap<Template, TemplateViewModel>();
                        cfg.CreateMap<MarketingStrategy, MarketingStrategyViewModel>();
                        cfg.CreateMap<MarketingType, MarketingTypeViewModel>();
                        cfg.CreateMap<CampaignStatus, CampaignStatusViewModel>();
                        cfg.CreateMap<User, UserViewModel>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var source = new Campaign();
                    source = user;
                    var dest = mapper.Map<Campaign, CampaignViewModel>(source);
                    campaignViewModel.Add(dest);
                }
            }
            return campaignViewModel;
        }

        public CampaignManager()
        {
            _icampaignRepository = new CampaignRepository();
            _icustomerRepository = new CustomerRepository();
            _icustomer_CampaignRepository = new Customer_CampaignRepository();
            _iemailMasterRepository = new EmailMasterRepository();
        }
        public List<CampaignViewModel> GetAllCampaigns()
        {
            List<CampaignViewModel> campaignViewModel = new List<CampaignViewModel>();
            var campaigns = _icampaignRepository.GetAllCampaigns();
            foreach (var user in campaigns)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Campaign, CampaignViewModel>().
                    ForMember(d => d.Brand, p => p.MapFrom(s => s.Brand)).
                     ForMember(d => d.Template, p => p.MapFrom(s => s.Template)).
                      ForMember(d => d.MarketingType, p => p.MapFrom(s => s.MarketingType)).
                       ForMember(d => d.MarketingStrategy, p => p.MapFrom(s => s.MarketingStrategy)).
                        ForMember(d => d.CampaignStatus, p => p.MapFrom(s => s.CampaignStatus)).
                        ForMember(d => d.User, p => p.MapFrom(s => s.User));
                    cfg.CreateMap<Brand, BrandViewModel>();
                    cfg.CreateMap<Template, TemplateViewModel>();
                    cfg.CreateMap<MarketingStrategy, MarketingStrategyViewModel>();
                    cfg.CreateMap<MarketingType, MarketingTypeViewModel>();
                    cfg.CreateMap<CampaignStatus, CampaignStatusViewModel>();
                    cfg.CreateMap<User, UserViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = new Campaign();
                source = user;
                var dest = mapper.Map<Campaign, CampaignViewModel>(source);
                campaignViewModel.Add(dest);
            }
            return campaignViewModel;
        }

        public List<CampaignStatusViewModel> GetAllCampaignStatus(int id)
        {
            List<CampaignStatusViewModel> campaignStatusViewModel = new List<CampaignStatusViewModel>();
            var statuses = _icampaignRepository.GetAllCampaignStatus();
            if (id != 0)
            {
                var campaign = _icampaignRepository.GetCampaignByid(id);
                if (campaign.CampaignStatusId == 1)
                {
                    statuses.RemoveAt(2);
                    statuses.RemoveAt(2);
                }
                if (campaign.CampaignStatusId == 2)
                {
                    statuses.RemoveAt(0);
                    statuses.RemoveAt(2);
                }
                else if (campaign.CampaignStatusId == 3 || campaign.CampaignStatusId == 4)
                {
                    statuses.RemoveAt(0);
                    statuses.RemoveAt(0);
                }
                else if (campaign.CampaignStatusId == 5)
                {
                    statuses.RemoveAt(0);
                    statuses.RemoveAt(0);
                    statuses.RemoveAt(0);
                    statuses.RemoveAt(0);
                }
            }
            //else 
            //{
            //    statuses.RemoveAt(1);
            //    statuses.RemoveAt(1);
            //    statuses.RemoveAt(1);
            //    statuses.RemoveAt(1);
            //}
            foreach (var user in statuses)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CampaignStatus, CampaignStatusViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = new CampaignStatus();
                source = user;
                var dest = mapper.Map<CampaignStatus, CampaignStatusViewModel>(source);
                campaignStatusViewModel.Add(dest);
            }
            return campaignStatusViewModel;

        }

        public List<MarketingStrategyViewModel> GetAllMarketingStrategy()
        {
            List<MarketingStrategyViewModel> marketingStrategyViewModel = new List<MarketingStrategyViewModel>();
            var marketStrategy = _icampaignRepository.GetAllMarketingStrategy();
            foreach (var user in marketStrategy)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<MarketingStrategy, MarketingStrategyViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = new MarketingStrategy();
                source = user;
                var dest = mapper.Map<MarketingStrategy, MarketingStrategyViewModel>(source);
                marketingStrategyViewModel.Add(dest);
            }
            return marketingStrategyViewModel;
        }

        public List<MarketingTypeViewModel> GetAllMarketingType()
        {
            List<MarketingTypeViewModel> marketingTypeViewModel = new List<MarketingTypeViewModel>();
            var marketsTypes = _icampaignRepository.GetAllMarketingTypes();
            foreach (var user in marketsTypes)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<MarketingType, MarketingTypeViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = new MarketingType();
                source = user;
                var dest = mapper.Map<MarketingType, MarketingTypeViewModel>(source);
                marketingTypeViewModel.Add(dest);
            }
            return marketingTypeViewModel;
        }

        public CampaignViewModel GetCampaignById(int Id)
        {
            Campaign cmn = _icampaignRepository.GetCampaignByid(Id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Campaign, CampaignViewModel>().
                ForMember(d => d.Brand, p => p.MapFrom(s => s.Brand)).
                 ForMember(d => d.Template, p => p.MapFrom(s => s.Template)).
                  ForMember(d => d.MarketingType, p => p.MapFrom(s => s.MarketingType)).
                   ForMember(d => d.MarketingStrategy, p => p.MapFrom(s => s.MarketingStrategy)).
                    ForMember(d => d.CampaignStatus, p => p.MapFrom(s => s.CampaignStatus)).
                     ForMember(d => d.User, p => p.MapFrom(s => s.User));
                cfg.CreateMap<Brand, BrandViewModel>();
                cfg.CreateMap<Template, TemplateViewModel>();
                cfg.CreateMap<MarketingStrategy, MarketingStrategyViewModel>();
                cfg.CreateMap<MarketingType, MarketingTypeViewModel>();
                cfg.CreateMap<CampaignStatus, CampaignStatusViewModel>();
                cfg.CreateMap<User, UserViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            var source = cmn;
            var dest = mapper.Map<Campaign, CampaignViewModel>(source);
            dest.TotalUser = _icustomer_CampaignRepository.GetCustomerIdsListByCampaignId(Id).Count();
            return dest;
        }

        public string GetCampaignStatusById(int id)
        {
            return _icampaignRepository.GetCampaignStatusById(id);
        }

        public int GetLatestCampaignId()
        {
            return _icampaignRepository.GetLatestCampaignId();
        }

        public string GetMarketingStrategyById(int id)
        {
            return _icampaignRepository.GetMarketingStrategyById(id);
        }

        public string GetMarketingTypeById(int id)
        {
            return _icampaignRepository.GetMarketingTypeById(id);
        }

        public bool SendCampaignMailAsync(int campaignId, int templateId)
        {
            bool status = false;
            List<CustomerViewModel> customerListForResponse = new List<CustomerViewModel>();
            List<CustomerCampaignViewModel> ccVm = new List<CustomerCampaignViewModel>();
            var cclist = _icustomer_CampaignRepository.GetCustomerListByCampaignId(campaignId);

            foreach (var user in cclist)
            {
                var config1 = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Customer_Campaign, CustomerCampaignViewModel>();
                });

                IMapper mapper1 = config1.CreateMapper();
                var source1 = new Customer_Campaign();
                source1 = user;
                var dest1 = mapper1.Map<Customer_Campaign, CustomerCampaignViewModel>(source1);
                ccVm.Add(dest1);
            }
            var customerListByCampaign = ccVm;
            Campaign cmn = _icampaignRepository.GetCampaignByid(campaignId);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Campaign, CampaignViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var source = cmn;
            var dest = mapper.Map<Campaign, CampaignViewModel>(source);

            var CampaignDetails = dest;

            foreach (var customer in customerListByCampaign)
            {
                Customer customerEntity = _icustomerRepository.GetCustomerById(customer.CustomerID);
                var configCustomer = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Customer, CustomerViewModel>();
                });
                IMapper mapperCustomer = configCustomer.CreateMapper();
                var sourceCustomer = customerEntity;
                var destCustomer = mapperCustomer.Map<Customer, CustomerViewModel>(sourceCustomer);

                var cust = destCustomer;
                customerListForResponse.Add(cust);
            }
            var link = "/api/ResponseApi/Feedback?guid=[GUID]";
            var temp = _iemailMasterRepository.GetTemplateById(templateId);
            string Template;
            Template = temp.TemplateData;

            foreach (var customer in customerListForResponse)
            {
                string msg;
                Customer_Campaign cc = new Customer_Campaign();
                cc = _icustomer_CampaignRepository.getCustomerByCampaignIdAndByCustomerId(campaignId, customer.CustomerID);
                if (cc.isEmailSent)
                {
                    continue;
                }
                if (cc.CustomTemplate != null)
                {
                    msg = cc.CustomTemplate;
                }
                else
                {
                    msg = Template;
                }
                string title = constant.responseTitle;
                title = title.Replace("[User]", customer.CustomerName);
                msg = msg.Replace("[User]", customer.CustomerName);
                msg = msg.Replace("[Date]", CampaignDetails.Start_Date.Date.ToString("dd/MM/yyyy"));
                msg = msg.Replace("[Day]", CampaignDetails.Start_Date.Day.ToString());
                msg = msg.Replace("[year]", CampaignDetails.Start_Date.Year.ToString());
                msg = msg.Replace("[CampaignName]", CampaignDetails.CampaignName);
                msg = msg.Replace("[CampaignOwner]", CampaignDetails.Brand.BrandName);

                string guidpositive = "CampaignId=" + CampaignDetails.CampaignId + "CustomerId=" +
                     customer.CustomerID + "CustomerEmail=" + customer.Email + "Response=Positive" + "END";
                string responseLinkpositive = link.Replace("[GUID]", Encrypt.EncryptString(guidpositive));
                var positive = constant.apiAddress + responseLinkpositive;
                msg = msg.Replace("[Positive]", positive);

                string guidNegative = "CampaignId=" + CampaignDetails.CampaignId + "CustomerId=" +
                    customer.CustomerID + "CustomerEmail=" + customer.Email + "Response=Negative" + "END";
                string responseLinknegative = link.Replace("[GUID]", Encrypt.EncryptString(guidNegative));
                var negative = constant.apiAddress + responseLinknegative;
                msg = msg.Replace("[Negative]", negative);

                string guidNeutral = "CampaignId=" + CampaignDetails.CampaignId + "CustomerId=" +
                    customer.CustomerID + "CustomerEmail=" + customer.Email + "Response=Neutral" + "END";
                string responseLinkNeutral = link.Replace("[GUID]", Encrypt.EncryptString(guidNeutral));
                var Neutral = constant.apiAddress + responseLinkNeutral;
                msg = msg.Replace("[Neutral]", Neutral);

                if (se.sendMail(msg,customer.Email,title))
                {
                    _icustomer_CampaignRepository.ChangeEmailStatus(cc.Id);
                    status = true;
                }
            }
            return status;
        }
    }
}
