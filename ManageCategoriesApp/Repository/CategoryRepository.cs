using Repository.Models;
using System.Collections.Generic;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Repository.Models.Category> GetCategories() => CategoryDAO.Instance.GetCategories();

        public Repository.Models.Category? GetCategoryById(int id) => CategoryDAO.Instance.GetCategoryById(id);

        public void AddCategory(Repository.Models.Category category) => CategoryDAO.Instance.AddCategory(category);

        public void UpdateCategory(Repository.Models.Category category) => CategoryDAO.Instance.UpdateCategory(category);

        public void DeleteCategory(int id) => CategoryDAO.Instance.DeleteCategory(id);
    }
}
