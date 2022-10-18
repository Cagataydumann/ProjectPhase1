﻿using Data.Concrete.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Data.Concrete.EFCore
{
    public class EFCoreCartRepository : EFCoreGenericRepository<Cart, ShopContext>, ICartRepository
    {
        public void ClearCart(int cartId)
        {
            using (var context = new ShopContext())
            {
                var cmd = @"delete from CartItems";
                context.Database.ExecuteSqlRaw(cmd, cartId);
            }
        }
        public void DeleteFromCart(int cartId, int productId)
        {
            using(var context = new ShopContext())
            {
                var cmd = @"delete from CartItems where CartId=@p0 and ProductId=@p1";
                context.Database.ExecuteSqlRaw(cmd, cartId, productId);
            }
        }


        public Cart GetByUserId(string userId)
        {
            using(var context = new ShopContext())
            {
                return context.Carts
                    .Include(i => i.CartItems)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault(i => i.UserId == userId);
            }
        }
        public override void Update(Cart entity)
        {
            using (var context = new ShopContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }

    }

}
