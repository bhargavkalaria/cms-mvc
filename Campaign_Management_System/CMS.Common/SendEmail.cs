using AutoMapper;
using CMS.DL.Implementation;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;


namespace CMS.Common
{
    public class SendEmail
    {
        private EmailMasterRepository emailMasterRepository = new EmailMasterRepository();
        private CustomerRepository _icustomerManager = new CustomerRepository();
        private Customer_CampaignRepository _customer_CampaignRepository = new Customer_CampaignRepository();
        private ICustomer_CampaignRepository _icustomer_CampaignRepository;
        private ICustomer_QuickCampaignRepository _icustomer_QuickCampaignRepository;
        private Constant constant = new Constant();
        public SendEmail()
        {
        }
        public SendEmail(ICustomer_CampaignRepository customer_CampaignRepository,
            ICustomer_QuickCampaignRepository customer_QuickCampaignRepository)
        {
            _icustomer_CampaignRepository = customer_CampaignRepository;
            _icustomer_QuickCampaignRepository = customer_QuickCampaignRepository;
        }
        
        public async Task<bool> SendForgotMail(string email, string body)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress("CMS@GMAIL");
                message.Subject = constant.forgotPasswordSubject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = constant.emailUsername,
                        Password = constant.emailPassword
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool sendMail(string msg,string email,string title)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress(constant.emailUsername);
            message.Subject = title;
            message.Body = msg;
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = constant.emailUsername,
                    Password = constant.emailPassword
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);

            }
            return true;
        }
    }
}
