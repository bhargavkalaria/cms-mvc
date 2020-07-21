using CMS.BE.ViewModels;
using System.Collections.Generic;

namespace CMS.BL.Interface
{
    public interface IBrandManager
    {
        List<BrandViewModel> GetAllBrands();
        bool AddBrand(BrandViewModel brandViewModel);
        bool EditBrand(BrandViewModel activityViewModel);
        bool deleteBrand(int id);
        BrandViewModel getBrandById(int id);
        bool CheckSimilar(BrandViewModel brandViewModel);
        List<BrandViewModel> GetAllBrandsForList();
    }
}
