using CMS.Data.Database;
using System.Collections.Generic;

namespace CMS.DL.Interface
{
    public interface IBrandRepository
    {
        List<Brand> GetAllBrands();
        bool AddBrand(Brand brand);
        Brand GetBrandById(int id);

        bool DeleteBrand(int id);

        bool EditBrand(Brand brand);

        bool CheckSimilar(Brand brand);

        IList<BrandBudgetData> topFiveBrandBudget();
    }
}
