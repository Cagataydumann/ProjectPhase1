using Data.Abstract;
using Data.Concrete.EFCore;
using Data.Concrete.EFCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShoppApp.WebApp.EmailServices;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EFCore;
using ShoppApp.WebApp.Identity;

internal class Program
{
    //private IConfiguration _configuration;
    //public Program(IConfiguration configuraiton)
    //{
    //    _configuration = configuraiton;
    //}

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Server=.;database=ShopAppDB;Trusted_Connection=True;MultipleActiveResultSets=true;"));
        //builder.Services.AddDbContext<ShopContext>(options => options.UseSqlServer("Server=.;database=ShopAppDB;Trusted_Connection=True"));
        builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;

            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.AllowedForNewUsers = true;

            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false; //onay maili için
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/account/login";
            options.LogoutPath = "/account/logout";
            options.AccessDeniedPath = "/account/accessdenied";
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.Cookie = new CookieBuilder
            {
                HttpOnly = true,
                Name = ".ShopApp.Security.Cookie",
                SameSite = SameSiteMode.Strict
            };
        });

        builder.Services.AddScoped<ICategoryRepository, EFCoreCategoryRepository>();
        builder.Services.AddScoped<IProductRepository, EFCoreProductRepository>();
        builder.Services.AddScoped<ICartRepository, EFCoreCartRepository>();
        builder.Services.AddScoped<IOrderRepository, EFCoreOrderRepository>();

        builder.Services.AddScoped<IProductService, ProductManager>();
        builder.Services.AddScoped<ICategoryService, CategoryManager>();
        builder.Services.AddScoped<ICartService, CartManager>();
        builder.Services.AddScoped<IOrderService, OrderManager>();
        //builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
        //    new SmtpEmailSender(
        //        _configuration["EmailSender:Host"],
        //        _configuration.GetValue<int>("EmailSender:Port"),
        //        _configuration.GetValue<bool>("EmailSender:EnableSSL"),
        //        _configuration["EmailSender:UserName"],
        //        _configuration["EmailSender:Password"])
        //    );
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllersWithViews().AddFluentValidation(a => a.RegisterValidatorsFromAssemblyContaining<Program>());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        if (app.Environment.IsDevelopment())
        {
            SeedDateBase.Seed();

        }

        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {

            endpoints.MapControllerRoute(
              name: "orders",
              pattern: "orders",
              defaults: new { controller = "Cart", action = "GetOrders" }
          );


            endpoints.MapControllerRoute(
              name: "checkout",
              pattern: "checkout",
              defaults: new { controller = "Cart", action = "Checkout" }
          );

            endpoints.MapControllerRoute(
                name: "cart",
                pattern: "cart",
                defaults: new { controller = "Cart", action = "Index" }
            );

            endpoints.MapControllerRoute(
               name: "adminuseredit",
               pattern: "admin/user/{id?}",
               defaults: new { controller = "Admin", action = "UserEdit" }
           );

            endpoints.MapControllerRoute(
               name: "adminusers",
               pattern: "admin/user/list",
               defaults: new { controller = "Admin", action = "UserList" }
           );

            endpoints.MapControllerRoute(
                name: "adminroles",
                pattern: "admin/role/list",
                defaults: new { controller = "Admin", action = "RoleList" }
            );

            endpoints.MapControllerRoute(
                name: "adminrolecreate",
                pattern: "admin/role/create",
                defaults: new { controller = "Admin", action = "RoleCreate" }
            );


            endpoints.MapControllerRoute(
                name: "adminroleedit",
                pattern: "admin/role/{id?}",
                defaults: new { controller = "Admin", action = "RoleEdit" }
            );

            endpoints.MapControllerRoute(
                name: "adminproducts",
                pattern: "admin/products",
                defaults: new { controller = "Admin", action = "ProductList" }
            );

            endpoints.MapControllerRoute(
                name: "adminproductcreate",
                pattern: "admin/products/create",
                defaults: new { controller = "Admin", action = "ProductCreate" }
            );

            endpoints.MapControllerRoute(
                name: "adminproductedit",
                pattern: "admin/products/{id?}",
                defaults: new { controller = "Admin", action = "ProductEdit" }
            );

            endpoints.MapControllerRoute(
               name: "admincategories",
               pattern: "admin/categories",
               defaults: new { controller = "Admin", action = "CategoryList" }
           );

            endpoints.MapControllerRoute(
                name: "admincategorycreate",
                pattern: "admin/categories/create",
                defaults: new { controller = "Admin", action = "CategoryCreate" }
            );

            endpoints.MapControllerRoute(
                name: "admincategoryedit",
                pattern: "admin/categories/{id?}",
                defaults: new { controller = "Admin", action = "CategoryEdit" }
            );


            // localhost/search    
            endpoints.MapControllerRoute(
                name: "search",
                pattern: "search",
                defaults: new { controller = "Shop", action = "search" }
            );

            endpoints.MapControllerRoute(
                name: "productdetails",
                pattern: "{url}",
                defaults: new { controller = "Shop", action = "details" }
            );

            endpoints.MapControllerRoute(
                name: "products",
                pattern: "products/{category?}",
                defaults: new { controller = "Shop", action = "list" }
            );

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
        });


        //SeedIdentity.Seed(userManager,roleManager,configuration).Wait();
        app.Run();
    }
}

 