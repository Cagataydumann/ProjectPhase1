using ShopApp.Entity;

namespace ShoppApp.WebApp.Models
{
    public class ProductDetailModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}
