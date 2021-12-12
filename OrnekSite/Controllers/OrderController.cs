using OrnekSite.Entity;
using OrnekSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrnekSite.Controllers
{
    public class OrderController : Controller
    {
        DataContext db = new DataContext();
        // GET: Order
        public ActionResult Index()
        {
            var orders = db.Orders.Select(i => new AdminOrder()
            {
                Id = i.Id,
                OrderNumber = i.OrderNumber,
                OrderDate = i.OrderDate,
                OrderState = i.OrderState,
                Total = i.Total,
                Count = i.OrderLines.Count
            }).OrderByDescending(x => x.OrderDate).ToList();
            return View(orders);
        }
        public ActionResult Details(int id)
        {
            var model = db.Orders.Where(i => i.Id == id).Select(i => new OrderDetails()
            {
                OrderId = i.Id,
                OrderNumber = i.OrderNumber,
                Total = i.Total,
                UserName = i.UserName,
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
            //////////////////////// DENEME ///////////////////
            var order = db.Orders.Where(i => i.Id == id).FirstOrDefault();
            OrderDetails orderDetails = new OrderDetails();
            orderDetails.OrderId = order.Id;
            orderDetails.OrderNumber = order.OrderNumber;
            orderDetails.Total = order.Total;
            orderDetails.UserName = order.UserName;
            orderDetails.OrderDate = order.OrderDate;
            orderDetails.OrderState = order.OrderState;
            orderDetails.Adres = order.Adres;
            orderDetails.Sehir = order.Sehir;
            orderDetails.Semt = order.Semt;
            orderDetails.Mahalle = order.Mahalle;
            orderDetails.PostaKodu = order.PostaKodu;
            List<OrderLineModel> orderLineModel = new List<OrderLineModel>();
            foreach (var item in order.OrderLines)
            {
                OrderLineModel orderLine = new OrderLineModel();
                orderLine.ProductId = item.ProductId;
                orderLine.Image = item.Product.Image;
                orderLine.ProductName = item.Product.Name;
                orderLine.Quantity = item.Quantity;
                orderLine.Price = item.Price;
                orderLineModel.Add(orderLine);
            }
            orderDetails.OrderLines = orderLineModel;

            return View(orderDetails);
        }

        public ActionResult UpdateOrderState(int OrderId,OrderState Orderstate)
        {
            var order = db.Orders.FirstOrDefault(i => i.Id == OrderId);
            if (order!=null)
            {
                order.OrderState = Orderstate;
                db.SaveChanges();
                TempData["mesaj"] = "Bilgiler Kaydedildi";
                return RedirectToAction("Details", new { id = OrderId });
            }
            return RedirectToAction("Index");
        }
        public ActionResult BekleyenSiparisler()
        {
            var model = db.Orders.Where(i => i.OrderState == OrderState.Bekleniyor).ToList();
            return View(model);
        }
        public ActionResult TamamlananSiparisler()
        {
            var model = db.Orders.Where(i => i.OrderState == OrderState.Tamamlandı).ToList();
            return View(model);
        }
        public ActionResult PaketlenenSiparisler()
        {
            var model = db.Orders.Where(i => i.OrderState == OrderState.Paketlendi).ToList();
            return View(model);
        }
        public ActionResult KargolananSiparisler()
        {
            var model = db.Orders.Where(i => i.OrderState == OrderState.Kargolandı).ToList();
            return View(model);
        }
    }
}