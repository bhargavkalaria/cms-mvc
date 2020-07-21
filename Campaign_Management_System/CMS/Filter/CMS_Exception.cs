using NLog;
using System;
using System.Web.Mvc;

namespace CMS.Filter
{
    public class CMS_Exception : HandleErrorAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;
            logger.Error(e, "Error Occured In : "+e.Source);
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = "ExceptionPage_CMS"
            };
        }
    }
}