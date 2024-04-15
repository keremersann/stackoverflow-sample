using AutoMapper;
using StackOverFlowClone.DomainModels;
using StackOverFlowClone.Repositories;
using StackOverFlowClone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.ServiceLayer
{
    public interface ICategoriesService
    {
        void InsertCategory(CategoryViewModel cvm);
        void UpdateCategory(CategoryViewModel cvm);
        void DeleteCategory(int categoryID);
        List<CategoryViewModel> GetCategories();
        CategoryViewModel GetCategoriyByCategoryId(int categoryId);
    }
    public class CategoriesService : ICategoriesService
    {
        public ICategoriesRepository cr;
        public CategoriesService()
        {
            cr = new CategoriesRepository();
        }
        public void InsertCategory(CategoryViewModel cvm)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryViewModel, Category>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            Category category = mapper.Map<CategoryViewModel, Category>(cvm);
            cr.InsertCategory(category);
        }
        public void UpdateCategory(CategoryViewModel cvm)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryViewModel, Category>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            Category category = mapper.Map<CategoryViewModel, Category>(cvm);
            cr.UpdateCategory(category);
        }
        public void DeleteCategory(int categoryID)
        {
            cr.DeleteCategory(categoryID);
        }
        public List<CategoryViewModel> GetCategories()
        {
            List<Category> categories = cr.GetAllCategories();
            List<CategoryViewModel> cvm = null;
            if(categories != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                cvm = mapper.Map<List<Category>, List<CategoryViewModel>>(categories);
            }
            return cvm;
        }
        public CategoryViewModel GetCategoriyByCategoryId(int categoryId)
        {
            Category category = cr.GetCategoryByCategoryId(categoryId).FirstOrDefault();
            CategoryViewModel cvm = null;
            if (category != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                cvm = mapper.Map<Category, CategoryViewModel>(category);
            }
            return cvm;
        }
    }
}
