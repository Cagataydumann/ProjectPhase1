namespace ShoppApp.WebApp.Models
{
    public class OrderModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }

        public CartModel CartModel { get; set; }
    }
}
