using Repository.Models;
using System.Collections.Generic;

namespace Repository
{
    public interface ICategoryRepository
    {
        List<Repository.Models.Category> GetCategories();
        Repository.Models.Category? GetCategoryById(int id);
        void AddCategory(Repository.Models.Category category);
        void UpdateCategory(Repository.Models.Category category);
        void DeleteCategory(int id);
    }
}
