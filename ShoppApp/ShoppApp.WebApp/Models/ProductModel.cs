using ShopApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace ShoppApp.WebApp.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        //[Required]
        //[StringLength(100),MinLength(5)]
        
        public string Name { get; set; }
        public string Url { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}
