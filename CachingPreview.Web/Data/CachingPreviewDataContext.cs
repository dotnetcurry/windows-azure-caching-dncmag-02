using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using CachingPreview.Web.Models;

namespace CachingPreview.Web.Data
{
    public class CachingPreviewDataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }   
    }
}