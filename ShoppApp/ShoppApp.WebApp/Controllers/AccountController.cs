using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppApp.WebApp.EmailServices;
using ShopApp.Business.Abstract;
using ShoppApp.WebApp.Extensions;
using ShoppApp.WebApp.Identity;
using ShoppApp.WebApp.Models;
using System.Threading.Tasks;

namespace ShoppApp.WebApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private ICartService _cartService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,ICartService cartService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public IActionResult Login(string ReturnUrl = null)
        //{
        //    return View(new LoginModel()
        //    {
        //        ReturnUrl = ReturnUrl
        //    });
        //}
        public IActionResult Login()
        {
            return View();
 
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Bu mail bilgisi ile kullanıcı bulunmamaktadır.");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "hesabızını onaylayın");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password,true,false);
            if(result.Succeeded)
            {
                //return Redirect(model.ReturnUrl ?? "~/");
                return RedirectToAction("Index","Home");

            }
            ModelState.AddModelError("", "Girilen bilgiler yanlış.");
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };


            var result = await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
            {
                _cartService.InitializeCart(user.Id);
                //generate token// emaille gönderilecek ileride
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var url = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = code });
                return RedirectToAction("Login","Account");
            }

            ModelState.AddModelError("", "Bilinmeyen hata.");
            return View(model);
            
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Put("message", new AlertMessage()
            {
                Title = "Oturum Kapatıldı.",
                Message = "Hesabınız güvenli bir şekilde kapatıldı.",
                AlertType = "warning"
            });
            return Redirect("~/");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    if (userId == null || token == null)
        //    {
        //        TempData.Put("message", new AlertMessage()
        //        {
        //            Title = "Geçersiz token.",
        //            Message = "Geçersiz Token",
        //            AlertType = "danger"
        //        });
        //        return View();
        //    }
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user != null)
        //    {
        //        var result = await _userManager.ConfirmEmailAsync(user, token);
        //        if (result.Succeeded)
        //        {
        //            // cart objesini oluştur.
        //            _cartService.InitializeCart(user.Id);

        //            TempData.Put("message", new AlertMessage()
        //            {
        //                Title = "Hesabınız onaylandı.",
        //                Message = "Hesabınız onaylandı.",
        //                AlertType = "success"
        //            });
        //            return View();
        //        }
        //    }
        //    TempData.Put("message", new AlertMessage()
        //    {
        //        Title = "Hesabınızı onaylanmadı.",
        //        Message = "Hesabınızı onaylanmadı.",
        //        AlertType = "warning"
        //    });
        //    return View();
        //}
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> ForgotPassword(string Email)
        //{
        //    if (string.IsNullOrEmpty(Email))
        //    {
        //        return View();
        //    }

        //    var user = await _userManager.FindByEmailAsync(Email);

        //    if (user == null)
        //    {
        //        return View();
        //    }

        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        //    var url = Url.Action("ResetPassword", "Account", new
        //    {
        //        userId = user.Id,
        //        token = code
        //    });

        //    // email
        //    await _emailSender.SendEmailAsync(Email, "Reset Password", $"Parolanızı yenilemek için linke <a href='https://localhost:5001{url}'>tıklayınız.</a>");

        //    return View();
        //}
        //public IActionResult ResetPassword(string userId, string token)
        //{
        //    if (userId == null || token == null)
        //    {
        //        return RedirectToAction("Home", "Index");
        //    }

        //    var model = new ResetPasswordModel { Token = token };

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        return RedirectToAction("Home", "Index");
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    return View(model);
        //}

    }
}
