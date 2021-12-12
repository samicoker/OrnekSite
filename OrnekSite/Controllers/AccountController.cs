using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using OrnekSite.Entity;
using OrnekSite.Identity;
using OrnekSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrnekSite.Controllers
{
    public class AccountController : Controller
    {
        DataContext db = new DataContext();

        private UserManager<ApplicationUser> UserManager; // kullanıcı oluşturmamızı ve var olan kullanıcıları yönetmemize yarar
        private RoleManager<ApplicationRole> RoleManager;
        public AccountController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            UserManager = new UserManager<ApplicationUser>(userStore);

            var roleStore = new RoleStore<ApplicationRole>(new IdentityDataContext());
            RoleManager = new RoleManager<ApplicationRole>(roleStore);
        }
        // GET: Account

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid) // zorunlu olan tüm alanları doldurmuş mu
            {
                var result = UserManager.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword); // eski şifre ile yeni şifreyi değiştirir. "changepassword", UserManagerin şifre değiştirme özelliğidir
                return View("Update");
            }
            return View(model);
        }
        public PartialViewResult UserCount()
        {
            var u = UserManager.Users;
            return PartialView(u);
        }
        public ActionResult UserList()
        {
            var u = UserManager.Users;
            return View(u);
        }
        public ActionResult UserProfil()
        {
            var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId(); // şuan bağlı olan, yani giriş yapmış olan kullanıcının ID'sini verir
            var user = UserManager.FindById(id); // burda da kullanıcıyı bulduk
            var data = new UserProfile()
            {
                id = user.Id,
                Name = user.Name,
                Surname=user.Surname,
                Email=user.Email,
                Username=user.UserName
            };
            return View(data);
        }
        [HttpPost]
        public ActionResult UserProfil(UserProfile model)
        {
            var user = UserManager.FindById(model.id); // burda da kullanıcıyı bulduk
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.UserName = model.Username;
            user.Email = model.Email;
            UserManager.Update(user);
            return View("Update");
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LogOut()
        {

            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.Find(model.Username, model.Password);
                if (user!=null)
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var Identityclaims = UserManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties();
                    authProperties.IsPersistent = model.RememberMe;
                    authManager.SignIn(authProperties, Identityclaims);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginUserError", "Böyle bir kullanıcı yok...");
                }
            }
            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.UserName = model.Username;
                user.Email = model.Email;
                var result = UserManager.Create(user, model.Password); // UserManagerin 'Create' adında bir metodu var, bu metod kullanıldığında yeni bir kullanıcı oluşturur.
                if (result.Succeeded) //kayıt başarılı olduysa
                {
                    if (RoleManager.RoleExists("user")) // user rolünü yeni kayıt olan kullanıcıya veriyoruz
                    {
                        UserManager.AddToRole(user.Id, "user");
                    }
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("RegisterUserError", "Kullanıcı oluşturma hatası...");
                }

            }
            return View(model);
        }
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var orders = db.Orders.Where(i => i.UserName == username).Select(i => new UserOrder
            {
                Id = i.Id,
                OrderNumber = i.OrderNumber,
                OrderState = i.OrderState,
                OrderDate = i.OrderDate,
                Total = i.Total
            }).OrderByDescending(i => i.OrderDate).ToList();
            return View(orders);
        }

        public ActionResult Details(int Id)
        {
            var model = db.Orders.Where(i => i.Id == Id).Select(i => new OrderDetails()
            {
                OrderId = i.Id,
                OrderNumber = i.OrderNumber,
                Total = i.Total,
                OrderDate = i.OrderDate,
                OrderState = i.OrderState,
                Adres = i.Adres,
                Sehir = i.Sehir,
                Semt = i.Semt,
                Mahalle = i.Mahalle,
                PostaKodu = i.PostaKodu,
                OrderLines = i.OrderLines.Select(x => new OrderLineModel()
                {
                    ProductId = x.ProductId,
                    Image = x.Product.Image,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            }).FirstOrDefault();
            ////////////////////////// diğer yöntem ///////////////
            //var mm = db.Orders.FirstOrDefault(i => i.Id == Id);
            //OrderDetails nesne = new OrderDetails();
            //nesne.OrderId = mm.Id;
            //nesne.OrderNumber = mm.OrderNumber;
            //nesne.Total = mm.Total;
            //nesne.OrderDate = mm.OrderDate;
            //nesne.OrderState = mm.OrderState;
            //nesne.Adres = mm.Adres;
            //nesne.Sehir = mm.Sehir;
            //nesne.Semt = mm.Semt;
            //nesne.Mahalle = mm.Mahalle;
            //nesne.PostaKodu = mm.PostaKodu;
            //var orderline = new OrderLineModel();
            //foreach (var item in mm.OrderLines)
            //{
            //    orderline.ProductId = item.ProductId;
            //    orderline.Image = item.Product.Image;
            //    orderline.ProductName = item.Product.Name;
            //    orderline.Quantity = item.Quantity;
            //    orderline.Price = item.Price;
            //}
            //nesne.OrderLines.Add(orderline);

            return View(model);
        }
    }
}