using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CachingPreview.Web.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]        
        public double UnitPrice { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}