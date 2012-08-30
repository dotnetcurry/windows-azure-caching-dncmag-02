using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CachingPreview.Web.Data;

namespace CachingPreview.Web.Models
{ 
    public class CategoryRepository : ICategoryRepository
    {
        CachingPreviewDataContext context = new CachingPreviewDataContext();

        public IQueryable<Category> GetAll()
        {
            return context.Categories; 
        }

        public IQueryable<Category> AllIncluding(params Expression<Func<Category, object>>[] includeProperties)
        {
            IQueryable<Category> query = context.Categories;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Category Find(int id)
        {
            return context.Categories.Find(id);
        }

        public void InsertOrUpdate(Category category)
        {
            if (category.CategoryId == default(int)) {
                // New entity
                context.Categories.Add(category);
            } else {
                // Existing entity
                context.Entry(category).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var category = context.Categories.Find(id);
            context.Categories.Remove(category);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface ICategoryRepository : IDisposable
    {
        IQueryable<Category> GetAll();
        IQueryable<Category> AllIncluding(params Expression<Func<Category, object>>[] includeProperties);
        Category Find(int id);
        void InsertOrUpdate(Category category);
        void Delete(int id);
        void Save();
    }
}