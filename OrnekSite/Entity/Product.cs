using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrnekSite.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } // ürün adı
        public string Description { get; set; } // açıklama
        public string Image { get; set; } // ürün resmi
        public double Price { get; set; } // fiyatı
        public int Stock { get; set; } // stok miktarı
        public bool Slider { get; set; } // slaytta kullanacağımız ürünleri getirmek için kullanacağız
        public bool IsHome { get; set; }  // anasayfada olacak mı olmayacak mı kontrolü
        public bool IsFeatured { get; set; }  // öne çıkan ürünler
        public bool IsApproved { get; set; } // onaylı bir ürün mü kontrolü
        public int CategoryId { get; set; } // ürün hangi kategoriye aitse onun kontrolü 
        public Category Category { get; set; } // ürünün kategori bilgilerine gitmek için bu alanı kullanırız
    }
}