using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CachingPreview.Web.Caching;
using CachingPreview.Web.Models;

namespace CachingPreview.Web.Controllers
{   
    public class ProductController : Controller
    {
		private readonly ICacheProvider cache;
		private readonly IProductRepository productRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        //public ProductController() : this(new ProductRepository(), new AzureCacheProvider())
        //{
        //}

        public ProductController(IProductRepository productRepository,ICacheProvider cache)
        {		
			this.productRepository = productRepository;
             this.cache = cache;
        }

        //
        // GET: /Product/

        public ViewResult Index()
        {
            return View(productRepository.AllIncluding(product => product.Category));
        }

        //
        // GET: /Product/Details/5

        public ViewResult Details(int id)
        {
            return View(productRepository.Find(id));
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            ViewBag.PossibleCategories = GetCategories();
            return View();
        } 

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid) {
                productRepository.InsertOrUpdate(product);
                productRepository.Save();
                return RedirectToAction("Index");
            } else {
                ViewBag.PossibleCategories = GetCategories();
				return View();
			}
        }
        
        //
        // GET: /Product/Edit/5
 
        public ActionResult Edit(int id)
        {
            ViewBag.PossibleCategories = GetCategories();
             return View(productRepository.Find(id));
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid) {
                productRepository.InsertOrUpdate(product);
                productRepository.Save();
                return RedirectToAction("Index");
            } else {
                ViewBag.PossibleCategories = GetCategories();
				return View();
			}
        }

        //
        // GET: /Product/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(productRepository.Find(id));
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            productRepository.Delete(id);
            productRepository.Save();

            return RedirectToAction("Index");
        }
        //Taking Category data from Windows Azure Caching
        private IEnumerable<Category> GetCategories()
        {
            IEnumerable<Category> categories = null;
            var cachedCategories = cache.Get("categories");

            if (cachedCategories != null)
            {
                categories = cachedCategories as IEnumerable<Category>;
            }
            return categories;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {               
                productRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

