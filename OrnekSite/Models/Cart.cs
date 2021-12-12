using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrnekSite.Entity;

namespace OrnekSite.Models
{
    public class Cart // bu cart modelinde sepete ürün ekleme, ürün silme, toplam tutar gibi işlemler yapılır. 
    { // alışveriş sepetinin tamamını bu Cart temsil eder.
        private List<Cartline> _cartLines = new List<Cartline>(); // kullanıcıya özel Cart oluşturduk kullanıcı sepete kaç farklı ürün eklediyse sepette alt atla sıralanır
        public List<Cartline> CartLines 
        { 
            get { return _cartLines; } // kullanıcı sepete ürün eklemek iste bu kolleksiyonuna ekler, silmek istediğinde de bu koleksiyondan siler
        }
        public void AddProduct(Product product, int quantity)
        {
            var line = _cartLines.FirstOrDefault(i => i.Product.Id == product.Id); // sepete ekleme
            if (line==null) // sepette olmayan ürünü sepete eklemiş oluyoruz
            {
                _cartLines.Add(new Cartline() { Product = product, Quantity = quantity });

                //Cartline kk = new Cartline();
                //kk.Product = product;
                //kk.Quantity = quantity;
                //_cartLines.Add(kk);
            }
            else
            {
                line.Quantity += quantity; // sepette ürün varsa üzerine ekler
            }
        }

        public void DeleteProduct(Product product) // ürün silme
        {
            _cartLines.RemoveAll(i => i.Product.Id == product.Id);
        }

        public double Total() // toplam fiyat
        {
            return _cartLines.Sum(i => i.Product.Price * i.Quantity);
        }
        public void Clear() // tüm ürünleri temizleme
        {
            _cartLines.Clear();  // bu metotla tüm ürünleri siler
        }
    }
    public class Cartline // bu classı tek bir ürünü temsil etmek için ekledik. alışveriş sepetinde 1 satırı temsil eder
    {
        public Product Product { get; set; } // product ile ilgili olduğu için bu kolonu ekledik
        public int Quantity { get; set; } // adet
    }
}