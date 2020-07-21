using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using CMS.Data.Database;
using CMS.DL.Interface;
using System.Collections.Generic;
using System.Linq;

namespace CMS.BL.Manager
{
    public class QuickCampaignManager : IQuickCampaignManager
    {
        private IQuickCampaignRepository _iQuickCampaignRepository;
        private IEmailMasterRepository _iemailMasterRepository;
        private ICustomerRepository _icustomerRepository;
        private ICustomer_QuickCampaignRepository _icustomerQuickCampaignRepository;
        SendEmail se = new SendEmail();
        public QuickCampaignManager(IQuickCampaignRepository iQuickCampaignRepository,
            IEmailMasterRepository emailMasterRepository, ICustomerRepository customerRepository,
            ICustomer_QuickCampaignRepository quickCampaignRepository)
        {
            _iQuickCampaignRepository = iQuickCampaignRepository;
            _iemailMasterRepository = emailMasterRepository;
            _icustomerRepository = customerRepository;
            _icustomerQuickCampaignRepository = quickCampaignRepository;
        }
        public bool AddQuickCampaign(QuickCampaignViewModel quickModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<QuickCampaignViewModel, QuickCampaign>();
            });

            IMapper mapper = config.CreateMapper();
            var source = quickModel;
            var dest = mapper.Map<QuickCampaignViewModel, QuickCampaign>(source);
            return _iQuickCampaignRepository.AddQuickCampaign(dest);
        }

        public bool CheckSimilar(QuickCampaignViewModel quickCampaignViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<QuickCampaignViewModel, QuickCampaign>();
            });

            IMapper mapper = config.CreateMapper();
            var source = quickCampaignViewModel;
            var dest = mapper.Map<QuickCampaignViewModel, QuickCampaign>(source);
            return _iQuickCampaignRepository.CheckSimilar(dest);
        }

        public bool AddToList(int quickcampaignId, List<int> customerIds)
        {
            return _iQuickCampaignRepository.AddToList(quickcampaignId, customerIds);
        }

        public List<QuickCampaignViewModel> GetAllQuickCampaigns()
        {
            var qc = _iQuickCampaignRepository.GetAllQuickCampaigns();
            List<QuickCampaignViewModel> QVM = new List<QuickCampaignViewModel>();
            foreach (var item in qc)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<QuickCampaign, QuickCampaignViewModel>().
                    ForMember(d => d.Template, p => p.MapFrom(s => s.Template));
                    cfg.CreateMap<Template, TemplateViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = item;
                var dest = mapper.Map<QuickCampaign, QuickCampaignViewModel>(source);
                QVM.Add(dest);
            }
            return QVM;
        }

        public List<int> GetCustomerIdsListByQuickCampaignId(int id)
        {
            return _iQuickCampaignRepository.GetCustomerIdsListByQuickCampaignId(id);
        }

        public int GetLatestQuickCampaignId()
        {
            return _iQuickCampaignRepository.GetLatestQuickCampaignId();
        }

        public bool sendMail(int quickcampaignId, List<int> customerIds, int templateId, string temp)
        {
            bool status = false;
            var QuickCampaign = _iQuickCampaignRepository.GetQuickCampaignById(quickcampaignId);
            string msg;
            if (temp == null)
            {
                var Temp = _iemailMasterRepository.GetTemplateById(templateId);
                msg = Temp.TemplateData;
            }
            else
            {
                msg = temp;
            }

            var link = "/api/ResponseApi/QuickFeedback?guid=[GUID]";
            foreach (var csId in customerIds)
            {
                var customer = _icustomerRepository.GetCustomerById(csId);
                string title = "Quick Test";
                title = title.Replace("[User]", customer.CustomerName);

                msg = msg.Replace("[User]", customer.CustomerName);
                msg = msg.Replace("[StartDate]",QuickCampaign.Start_Date.Date.ToString("dd/MM/yyyy"));
                msg = msg.Replace("[CreatedDate]", QuickCampaign.CreatedOn.Date.ToString("dd/MM/yyyy"));
                msg = msg.Replace("[ModifiedDate]", QuickCampaign.ModifiedOn.Date.ToString("dd/MM/yyyy"));
                //msg = msg.Replace("[Day]", CampaignDetails.Start_Date.Day.ToString());
                //msg = msg.Replace("[year]", CampaignDetails.Start_Date.Year.ToString());
                msg = msg.Replace("[QuickCampaignName]", QuickCampaign.QuickCampaignName);
                


                string guidpositive = "QuickCampaignId=" + quickcampaignId + "CustomerId=" +
                         customer.CustomerID + "CustomerEmail=" + customer.Email + "Response=Positive" + "END";
                string responseLinkpositive = link.Replace("[GUID]", Encrypt.EncryptString(guidpositive));
                var positive = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace
                    (System.Web.HttpContext.Current.Request.Url.PathAndQuery, responseLinkpositive);
                msg = msg.Replace("[Positive]", positive);


                string guidnegative = "QuickCampaignId=" + quickcampaignId + "CustomerId=" +
                         customer.CustomerID + "CustomerEmail=" + customer.Email + "Response=Negative" + "END";
                string responseLinknegative = link.Replace("[GUID]", Encrypt.EncryptString(guidpositive));
                var negative = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace
                    (System.Web.HttpContext.Current.Request.Url.PathAndQuery, responseLinkpositive);
                msg = msg.Replace("[Negative]", negative);


                string guidneutral = "QuickCampaignId=" + quickcampaignId + "CustomerId=" +
                         customer.CustomerID + "CustomerEmail=" + customer.Email + "Response=Neutral" + "END";
                string responseLinkneutral = link.Replace("[GUID]", Encrypt.EncryptString(guidpositive));
                var neutral = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace
                    (System.Web.HttpContext.Current.Request.Url.PathAndQuery, responseLinkpositive);
                msg = msg.Replace("[Neutral]", neutral);
                if (se.sendMail(msg, customer.Email, title))
                {
                    _icustomerQuickCampaignRepository.ChangeEmailStatus(quickcampaignId, customer.CustomerID);
                    status = true;
                }
            }
            return status;  
        }

        public string EmailPreview(QuickCampaignViewModel quickcampaignViewModel, string templateData, List<int> customerIds)
        {
            string template = templateData;
            int customerId = customerIds.First();
            var customer = _icustomerRepository.GetCustomerById(customerId);
            template = template.Replace("[User]", customer.CustomerName);
            template = template.Replace("[Date]", quickcampaignViewModel.Start_Date.Date.ToString("dd/MM/yyyy"));
            template = template.Replace("[Day]", quickcampaignViewModel.Start_Date.DayOfWeek.ToString());
            template = template.Replace("[year]", quickcampaignViewModel.Start_Date.Year.ToString());
            template = template.Replace("[CampaignName]", quickcampaignViewModel.QuickCampaignName);
            return template;
        }
    }
}
