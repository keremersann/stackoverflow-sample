using StackOverFlowClone.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackOverFlowClone.Repositories
{
    public interface ICategoriesRepository
    {
        void InsertCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int categoryId);
        List<Category> GetAllCategories();
        List<Category> GetCategoryByCategoryId(int categoryId);
    }
    public class CategoriesRepository : ICategoriesRepository
    {
        StackOverflowDatabaseDBContext db;
        public CategoriesRepository()
        {
            db = new StackOverflowDatabaseDBContext();
        }
        public void InsertCategory(Category category) 
        {
            db.Categories.Add(category);
            db.SaveChanges();
        }
        public void UpdateCategory(Category category) 
        {
            Category categoryToBeUpdated = db.Categories.Where(c => c.CategoryID == category.CategoryID).FirstOrDefault();
            if(categoryToBeUpdated != null) 
            {
                categoryToBeUpdated.CategoryName = category.CategoryName;
                db.SaveChanges();
            }
        }
        public void DeleteCategory(int categoryId)
        {
            Category categoryToBeDeleted = db.Categories.Where(c => c.CategoryID == categoryId).FirstOrDefault();
            if (categoryToBeDeleted != null)
            {
                db.Categories.Remove(categoryToBeDeleted);
                db.SaveChanges();
            }
        }
        public List<Category> GetAllCategories() 
        {
            return db.Categories.ToList();
        }
        public List<Category> GetCategoryByCategoryId(int categoryId)
        {
            return db.Categories.Where(c => c.CategoryID == categoryId).ToList();
        }
    }
}
