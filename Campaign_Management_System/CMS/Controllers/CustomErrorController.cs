using CMS.Filter;
using NLog;
using System;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    [HandleError(View = "Custom_Error")]
    [CMS_Exception]
    public class CustomErrorController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        // GET: Error2
        public ActionResult Index()
        {
            try
            {
                throw new HttpException(500, "Internal Server Error");
            }
            catch (Exception e)
            {
                logger.Error(e, "Error Occured In : " + e.Source);
                return View();
            }
        }
        public ActionResult NotFound()
        {
            try
            {
                throw new HttpException(404, "Requested Page Not Found");
            }
            catch (Exception e)
            {
                logger.Error(e, "Error Occured In : " + e.Source);
                return View();
            }
        }
    }
}