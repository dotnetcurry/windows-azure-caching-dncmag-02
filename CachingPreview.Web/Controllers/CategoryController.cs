using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CachingPreview.Web.Caching;
using CachingPreview.Web.Models;

namespace CachingPreview.Web.Controllers
{   
    public class CategoryController : Controller
    {
		private readonly ICategoryRepository categoryRepository;
        private readonly ICacheProvider cache;
		// If you are using Dependency Injection, you can delete the following constructor
        //public CategoryController() : this(new CategoryRepository(),new AzureCacheProvider())
        //{
        //}

        public CategoryController(ICategoryRepository categoryRepository, ICacheProvider cache)
        {
			this.categoryRepository = categoryRepository;
            this.cache = cache;
        }

        //
        // GET: /Category/

        public ViewResult Index()
        {
            //Get cached data from Cache if the data exist, 
            IEnumerable<Category> categories=null;
            var cachedCategories = cache.Get("categories");

            if (cachedCategories != null)
            {
                categories = cachedCategories as IEnumerable<Category>;
            }
            else
            {
                //Data from Database
                categories = categoryRepository.GetAll().ToList();
                if(categories!=null)
                cache.Put("categories", categories);
            }
            return View(categories);
        }

        //
        // GET: /Category/Details/5

        public ViewResult Details(int id)
        {
            return View(categoryRepository.Find(id));
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid) {
                categoryRepository.InsertOrUpdate(category);
                categoryRepository.Save();
                var categories = categoryRepository.GetAll().ToList();
                cache.Put("categories", categories);
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Category/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(categoryRepository.Find(id));
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid) {
                categoryRepository.InsertOrUpdate(category);
                categoryRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Category/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(categoryRepository.Find(id));
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            categoryRepository.Delete(id);
            categoryRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                categoryRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

