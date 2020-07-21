using CMS.BE.ViewModels;
using System.Collections.Generic;
using System.Web;

namespace CMS.BL.Interface
{
    public interface IDataImportManager
    {
        string ReadAndSaveExcel(HttpPostedFileBase httpPostedFile);
        List<CustomerViewModel> GetAllCustomers();
    }
}
