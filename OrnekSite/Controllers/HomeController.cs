using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrnekSite.Entity;

namespace OrnekSite.Controllers
{
    public class HomeController : Controller
    {
        DataContext db = new DataContext();
        // GET: Home

        public PartialViewResult _FeaturedProductList()
        {
            var data = db.Products.Where(x =>x.IsApproved && x.IsFeatured).Take(5).ToList(); /* popüler ürünlerden ilk 5 tanesini getirdik */
            return PartialView(data);
        }
        public ActionResult Adres()
        {
            return View();
        }
        public ActionResult Search(string q)
        {
            var p = db.Products.Where(x => x.IsApproved);
            if (!string.IsNullOrEmpty(q)) /* q nun içi null ya da boş değilse */
            {
                p = p.Where(x => x.Name.Contains(q) || x.Description.Contains(q));
            }
            return View(p.ToList());
        }
        public PartialViewResult Slider()
        {
            var data = db.Products.Where(x => x.IsApproved && x.Slider).Take(5).ToList(); 
            return PartialView(data);
        }
        public ActionResult Index()
        {
            var degerler = db.Products.Where(x => x.IsHome && x.IsApproved).ToList();
            return View(degerler);
        }
        public ActionResult ProductDetails(int id)
        {
            var data = db.Products.Where(x => x.Id == id).FirstOrDefault();
            return View(data);
        }
        public ActionResult Product()
        {
            var data = db.Products.ToList();
            return View(data);
        }
        public ActionResult ProductList(int id)
        {
            var data = db.Products.Where(x => x.CategoryId == id).ToList();
            return View(data);
        }
    }
}