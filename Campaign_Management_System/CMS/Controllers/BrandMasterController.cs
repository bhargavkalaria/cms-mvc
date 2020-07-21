using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using CMS.Filter;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http; 
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS.Controllers
{

    [CMS_Exception]
    [HandleError(View = "Custom_Error")]
   

    public class BrandMasterController : Controller
    {

        string baseUrl = "https://localhost:44308/";
        private IBrandManager _iBrandmanager;
        private IRoleManager _iRoleManager;
        private Constant constant = new Constant();
        public BrandMasterController()
        {

        }
        public BrandMasterController(IBrandManager brandManager,IRoleManager iRoleManager)
        {
            _iBrandmanager = brandManager;
            _iRoleManager = iRoleManager;
        }
        // GET: Brand
        [HttpGet]
        public ActionResult BrandList()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (!getViewBrandAccess())
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!(getViewBrandAccess() || getAddBrandAccess() || getDeleteBrandAccess() || getEditBrandAccess()))
            {
                return RedirectToAction("NotAuthorized","Response");
            }
            IEnumerable<BrandViewModel> BrandsList = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseUrl);
                string addressUri = "api/BrandApi/GetAllBrands";
                var responseTask = client.GetAsync(addressUri);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    BrandsList = JsonConvert.DeserializeObject<List<BrandViewModel>>(Response);
                    return View(BrandsList);
                }
                else
                {
                    //log response status here..
                    BrandsList = Enumerable.Empty<BrandViewModel>();

                   ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(BrandsList);
                }

            }

        }
        [HttpGet]
        public ActionResult AddBrand()
        {
           
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getAddBrandAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            ViewBag.Error = "";
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddBrand(BrandViewModel brandViewModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getAddBrandAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    var response = await client.PostAsJsonAsync("api/BrandApi/InsertBrand", brandViewModel);
                    int res =(int)response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Success!";
                        return RedirectToAction("BrandList");
                    }
                    else if (res.Equals(400))
                    {
                        ViewBag.Error = "Brand with same name already exist";
                        return View(brandViewModel);
                    }
                    else
                    {
                        ViewBag.Error =response.ReasonPhrase;
                        return View(brandViewModel);
                    }
                }

            }
            else
            {
                var msg = ModelState.Values.First().Errors[0].ErrorMessage;
                ViewBag.Error = msg;
                return View(brandViewModel);
            }

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getDeleteBrandAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }

            BrandViewModel data = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var responseTask = client.GetAsync("api/BrandApi/GetBrand?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<BrandViewModel>(Response);
                    return View(data);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View("BrandList");
                }

            }
        }
        [HttpPost]
        public ActionResult Delete(BrandViewModel brandViewModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getDeleteBrandAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValidField("BrandId"))
            {
                int id = brandViewModel.BrandId;
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(baseUrl);
                    var responseTask = client.DeleteAsync("api/BrandApi/DeleteBrand/" + id.ToString());
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return View(brandViewModel);
                    }

                }

            }
            else
            {
                ViewBag.Error = constant.Error_Message;
                return View(brandViewModel);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getEditBrandAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            BrandViewModel data = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseUrl);
                var responseTask = client.GetAsync("api/BrandApi/GetBrand?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<BrandViewModel>(Response);
                    ViewBag.Error = "";
                    return View(data);
                }
                else
                {
                    ViewBag.Error = result.ReasonPhrase;
                    return View("BrandList");
                }

            }

        }

        [HttpPost]
        public async Task<ActionResult> Edit(BrandViewModel brandViewModel) 
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getEditBrandAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    var response = await client.PutAsJsonAsync("api/BrandApi/UpdateBrand", brandViewModel);
                    int res = (int)response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "SuccessEdit!";
                        return RedirectToAction("BrandList");
                    }
                    else if (res.Equals(400))
                    {
                        ViewBag.Error = "Brand with same name already exist";
                        return View(brandViewModel);
                    }
                    else
                    {
                        ViewBag.Error = response.ReasonPhrase;
                        return View(brandViewModel);
                    }
                }
            }
            else
            {
                var msg = ModelState.Values.First().Errors[0].ErrorMessage;
                ViewBag.Error = msg;
                return View(brandViewModel);
            }
        }

        private bool getAddBrandAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetAddBrandAccess?id=" + UserId.ToString());
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
        private bool getViewBrandAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetViewBrandAccess?id=" + UserId.ToString());
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
        private bool getEditBrandAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetEditBrandAccess?id=" + UserId.ToString());
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
        private bool getDeleteBrandAccess()
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