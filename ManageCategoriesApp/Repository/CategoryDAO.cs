using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class CategoryDAO
    {
        private static CategoryDAO? _instance;
        private static readonly object _lock = new object();

        private CategoryDAO() { }

        public static CategoryDAO Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CategoryDAO();
                    }
                    return _instance;
                }
            }
        }

        public List<Repository.Models.Category> GetCategories()
        {
            try
            {
                using var context = new MyStoreContext();
                return context.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetCategories: " + ex.Message, ex);
            }
        }

        public Repository.Models.Category? GetCategoryById(int id)
        {
            try
            {
                using var context = new MyStoreContext();
                return context.Categories.FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetCategoryById: " + ex.Message, ex);
            }
        }

        public void AddCategory(Repository.Models.Category category)
        {
            try
            {
                using var context = new MyStoreContext();
                context.Categories.Add(category);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddCategory: " + ex.Message, ex);
            }
        }

        public void UpdateCategory(Repository.Models.Category category)
        {
            try
            {
                using var context = new MyStoreContext();
                context.Categories.Update(category);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateCategory: " + ex.Message, ex);
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                using var context = new MyStoreContext();
                var category = context.Categories.FirstOrDefault(c => c.Id == id);
                if (category != null)
                {
                    var hasProducts = context.Products.Any(p => p.CategoryId == id);
                    if (hasProducts)
                    {
                        throw new InvalidOperationException("Không thể xóa danh mục này vì đã có sản phẩm thuộc danh mục.");
                    }
                    
                    context.Categories.Remove(category);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
