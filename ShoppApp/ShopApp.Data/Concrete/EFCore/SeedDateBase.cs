using Data.Concrete.EFCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Data.Concrete.EFCore
{
    public static class SeedDateBase
    {
        public static void Seed()
        {
            var context = new ShopContext();
            if(context.Database.GetPendingMigrations().Count()==0)
            {
                if(context.Categories.Count()==0)
                {
                    context.Categories.AddRange(Categories);
                }
                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
            }
            context.SaveChanges();
        }
        private static Category[] Categories =
        {
            new Category() { Name="Telefon",Url="telefon"},
            new Category() { Name="Bilgisayar",Url="bilgisayar"},
            new Category() { Name="Elektronik",Url="elektronik"}
        };

        private static Product[] Products =
{
            new Product() { Name="Iphone 11",Url="iphone-11",Price=15000,ImageUrl="1.jpg",Description="f/p telefon",IsApproved=true},
            new Product() { Name="Iphone 12",Url="iphone-12",Price=20000,ImageUrl="2.jpg",Description="f/p telefon değil",IsApproved=false},
            new Product() { Name="Iphone 13",Url="iphone-13",Price=25000,ImageUrl="3.jpg",Description="Güzel telefon",IsApproved=true},
            new Product() { Name="Iphone 14",Url="iphone-14",Price=30000,ImageUrl="4.jpg",Description="Süper telefon",IsApproved=true},
        };
        private static ProductCategory[] ProductCategories =
        {
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[2]},
            new ProductCategory(){Product=Products[2],Category=Categories[0]},
            new ProductCategory(){Product=Products[2],Category=Categories[2]},
            new ProductCategory(){Product=Products[3],Category=Categories[0]},
            new ProductCategory(){Product=Products[3],Category=Categories[2]},
        };
    }

}
