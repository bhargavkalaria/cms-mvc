using CMS.BL.Interface;
using CMS.Common;
using CMS.Filter;
using NLog;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Controllers
{
    [HandleError(View = "Custom_Error")]
    [CMS_Exception]
    public class DataImportController : Controller
    {
        Constant constant = new Constant();
        // GET: DataImport
        private IDataImportManager _idataImportManager;
        private IRoleManager _iRoleManager;
        public DataImportController()
        {

        }
        public DataImportController(IDataImportManager dataImportManager,IRoleManager iRoleManager)
        {
            _idataImportManager = dataImportManager;
            _iRoleManager = iRoleManager;
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getUploadCustomerAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase upload)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getUploadCustomerAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {
                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:

                    if (upload.FileName.EndsWith(constant.xlsfile) || upload.FileName.EndsWith(constant.xlsxfile))
                    {
                        string status = _idataImportManager.ReadAndSaveExcel(upload);
                        if (status == "success")
                        {
                            ModelState.AddModelError(constant.fileUploadErrorKey, constant.successUploadCustomer);
                            return View();
                        }
                        else
                        {
                            ModelState.AddModelError(constant.fileUploadErrorKey, status);
                            return View();
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(constant.fileUploadErrorKey, constant.fileUploadErrorMessage);
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError(constant.fileUploadErrorKey, constant.fileUploadNullError);
                }
            }
            return View();
        }

        public ActionResult Customers()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getUploadCustomerAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            return View(_idataImportManager.GetAllCustomers());
        }

        public void DownloadFormat()
        {
            
            var gv = new GridView();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[8]
            {
                new DataColumn("Name"),
                new DataColumn("Email"),
                new DataColumn("City"),
                new DataColumn("Mobile"),
                new DataColumn("Age"),
                new DataColumn("State"),
                new DataColumn("Country"),
                new DataColumn("Address"),
            });
            dt.Rows.Add("abc", "abc@gmail.com", "abc", "1234567890", "22", "xyx", "xyz", "xyz");
            gv.DataSource = dt;
            gv.DataBind();
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                gv.Columns[i].ItemStyle.Width = 500;
            }
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Customer Format.xlsx");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
        }


        private bool getUploadCustomerAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetDeleteBrandAccess?id=" + UserId.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private int getUId()
        {
            var getCookie = Request.Cookies["UserId"];
            if (getCookie == null)
            {
                return 0;
            }
            string UId = getCookie.Value;
            string decUId = Encrypt.DecryptString(UId);

            int pFrom = decUId.IndexOf("UserId") + "UserId".Length;
            int pTo = decUId.LastIndexOf("END");

            int UserId = Convert.ToInt32(decUId.Substring(pFrom, pTo - pFrom));
            return UserId;
        }
    }
}