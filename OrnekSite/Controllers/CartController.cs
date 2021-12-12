using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrnekSite.Models;
using OrnekSite.Entity;

namespace OrnekSite.Controllers
{
    public class CartController : Controller
    {
        DataContext db = new DataContext(); // veritabanı ile bağlantı kurduk
        // GET: Cart

        public ActionResult Index()
        {
            return View(GetCart());
        }

        private void SaveOrder(Cart cart, ShippingDetails model)
        {
            var order = new Order();
            order.OrderNumber = "A" + (new Random()).Next(1111, 9999).ToString(); // A harfi ile başlayan ve 4 haneli rastgele bir sayı olan bir sipariş numarası
            order.Total = cart.Total();
            order.OrderDate = DateTime.Now;
            order.UserName = User.Identity.Name;
            order.OrderState = OrderState.Bekleniyor;
            order.Adres = model.Adres;
            order.Sehir = model.Sehir;
            order.Semt = model.Semt;
            order.Mahalle = model.Mahalle;
            order.PostaKodu = model.PostaKodu;
            order.OrderLines = new List<OrderLine>();
            foreach (var item in cart.CartLines)
            {
                var orderline = new OrderLine();
                orderline.Quantity = item.Quantity;
                orderline.Price = item.Quantity * item.Product.Price;
                orderline.ProductId = item.Product.Id;
                order.OrderLines.Add(orderline);
            }
            db.Orders.Add(order);
            db.SaveChanges();
        }
        public ActionResult Checkout()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ActionResult Checkout(ShippingDetails model)
        {
            var cart = GetCart();
            if (cart.CartLines.Count==0)
            {
                ModelState.AddModelError("UrunYok", "Sepetinizde ürün bulunmamaktadır..");
            }
            if (ModelState.IsValid)
            {
                SaveOrder(cart, model);
                cart.Clear();
                return View("SiparisTamamlandi");
            }
            else
            {
                return View(model);
            }

        }
        public PartialViewResult Summary()
        {
            return PartialView(GetCart());
        }

        public PartialViewResult Summary1()
        {
            return PartialView(GetCart());
        }
        public ActionResult RemoveFromCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if (product!=null)
            {
                GetCart().DeleteProduct(product);
            }
            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(int Id) // sepete ürün ekleme işlemi
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if (product != null) // ürün veritabanında var ise kontrolü
            {
                GetCart().AddProduct(product, 1);
            }
            return RedirectToAction("Index");
        }
        public Cart GetCart() // kullanıcının kartı varsa onu getir dedik
        {
            var cart = (Cart)Session["Cart"]; // tüm web programlama dillerinde oturum bilgilerini saklamak ve yönetmek için Session kullanılır. Kullanıcının bilgisayarında değil de sunucu bilgisayarında saklanır.
            if (cart==null) // kullanıcı ilk kez kart isteyecekse ona yeni kart veririz. tekrar isterse bunu veririz.
            {
                cart = new Cart(); // yeni bir kart oluşturduk
                Session["Cart"] = cart; // kartı Sessiona ekledik
            }
            return cart;
        }
    }
}