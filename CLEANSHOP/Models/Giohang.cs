using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CLEANSHOP.Models;

namespace CLEANSHOP.Models
{
    public class Giohang
    {

        MydataDataContext data = new MydataDataContext();
        public int ID { get; set; }
                      
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Ảnh bìa")]
        public string Image { get; set; }
        [Display(Name = "Price")]
        public Double Price { get; set; }            
        [Display(Name = "Số lượng")]
        public int iSoLuong { get; set; }
        [Display(Name = "Thành tiền")]
        public Double TotalPrice
        {
            get { return iSoLuong * Price; }
        }
        public Giohang(int  id)
        {
             ID = id;
             Product product = data.Products.Single(n => n.Id ==id);
            ProductName = product.ProductName;
            Image = product.Image;
            Price = double.Parse(product.Price.ToString());
            iSoLuong = 1;
        }

        
     
    }
}