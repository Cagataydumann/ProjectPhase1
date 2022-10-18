using Data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Data.Abstract
{
    public interface ICartRepository:IRepository<Cart>
    {
        Cart GetByUserId(string userId);
        void DeleteFromCart(int cartId,int productId);
        void ClearCart(int cartId);

    }
}
