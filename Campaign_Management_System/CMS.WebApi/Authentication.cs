using CMS.BL.Interface;
using CMS.BL.Manager;
using CMS.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace CMS.WebApi
{
    public class Authentication : AuthorizeAttribute
    {
        private ILoginManager _iLoginManager;
        private IRoleManager _iroleManager;
        public Authentication()
        {
            _iLoginManager = new LoginManager();
            _iroleManager = new RoleManager();
        }
        public Authentication(ILoginManager loginManager, IRoleManager roleManager)
        {
            _iLoginManager = loginManager;
            _iroleManager = roleManager;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string act = actionContext.ControllerContext.RouteData.Values["action"].ToString();
            string cot = actionContext.ControllerContext.RouteData.Values["controller"].ToString();
            bool status = false;
            if (actionContext.Request.Headers.Authorization != null)
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string[] usernamePasswordArray = authenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];
                status = true;
                if (!status)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}