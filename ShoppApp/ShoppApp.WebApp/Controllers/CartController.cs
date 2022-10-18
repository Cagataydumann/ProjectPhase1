using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShoppApp.WebApp.Identity;
using ShoppApp.WebApp.Models;

namespace ShoppApp.WebApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private UserManager<User> _userManager;
        private IOrderService _orderService;
        public CartController(ICartService cartService,UserManager<User> userManager, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult GetOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrders(userId);
            var orderlistModel = new List<OrderListModel>();
            OrderListModel orderModel;
            foreach (var order in orders)
            {
                orderModel = new OrderListModel();

                orderModel.OrderId = order.Id;
                orderModel.OrderNumber= order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.Adress = order.Adress;
                orderModel.Email = order.Email;
                orderModel.OrderState = order.OrderState;

                orderModel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    Quantity=i.Quantity,
                    ImageUrl = i.Product.ImageUrl,
                }).ToList();
                orderlistModel.Add(orderModel);
            }


            return View("Orders",orderlistModel);
        }
        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
            return View(new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId,int quantity)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.AddToCart(userId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.DeleteFromCart(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            };

            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = (double)i.Product.Price,
                        ImageUrl = i.Product.ImageUrl,
                        Quantity = i.Quantity
                       
                    }).ToList()
                };

                SaveOrder(model, userId);
                ClearCart(model.CartModel.CartId);
                return View("Success");
                //if (payment.Status == "success")
                //{
                //    SaveOrder(model, userId);
                //    return View("Success");
                //}
                //else
                //{
                //    var msg = new AlertMessage()
                //    {
                //        Message = $"{payment.ErrorMessage}",
                //        AlertType = "danger"
                //    };

                //    TempData["message"] = JsonConvert.SerializeObject(msg);
                //}
            //ClearCart(model.CartModel.CartId);

            return View("Success");
        }

        private void SaveOrder(OrderModel model, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = EnumOrderState.completed;

            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UserId = userId;
            order.Adress = model.Adress;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;

            order.OrderItems = new List<OrderItem>();

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new ShopApp.Entity.OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderItem);
            }
            _orderService.Create(order);
        }

        //[HttpPost]
        //private  IActionResult ClearCart(int cartId)
        //{
        //    _cartService.ClearCart(cartId);
        //    return RedirectToAction("Success");

        //}
        //[HttpPost]
        public IActionResult ClearCart(int cartId)
        {
            _cartService.ClearCart(cartId);
            return RedirectToAction("Index");
        }


    }
}
