using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLEANSHOP.Models;
using PagedList;

namespace CLEANSHOP.Controllers
{
    public class DichVuController : Controller
    {
        // GET: DichVu
        MydataDataContext data = new MydataDataContext();

        public ActionResult Index(int ? page, string searchString)
        {
            if (page == null) page = 1;

            var all_product = (from tt in data.Products select tt).OrderBy(m=>m.Id);
            if (!string.IsNullOrEmpty(searchString)) all_product = (IOrderedQueryable<Product>)all_product.Where(a => a.ProductName.Contains(searchString));
            int pageSize = 6;
            int pageNum = page ?? 1;

            return View(all_product.ToPagedList(pageNum,pageSize) );
        }
        public ActionResult Sanpham(int? page)
        {
            if (page == null) page = 1;

            var all_product = (from tt in data.Products select tt).OrderBy(m => m.Id);
            int pageSize = 100;
            int pageNum = page ?? 1;    

            return View(all_product.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Detail(int id)
        {
            var D_Products = data.Products.Where(m => m.Id == id).First(); return View(D_Products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, Product s)
        {

        var E_ProductName = collection["ProductName"];
        var E_Image = collection["Image"];
        var E_Detail = collection["Detail"];
        var E_Time = collection["Time"];
        var E_Text = collection["Text"];
    
        var E_Price = Convert.ToDecimal(collection["Price"]);
           // var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
           // var E_soluongton Convert.ToInt32(collection["soluongton"]);
            if (string.IsNullOrEmpty(E_ProductName))
            
                {
                    ViewData["Error"] = "Don't empty!";
                }
            else
                {
                    s.ProductName = E_ProductName.ToString();
                    s.Image = E_Image.ToString();
                    s.Detail = E_Detail.ToString();
                    s.Time = E_Time.ToString();
                    s.Text = E_Text.ToString();

                    s.Price =E_Price;
                 //   s.ngaycapnhat = E_ngaycapnhat;
                  //  s.soluongton = E_soluongton;
                    data.Products.InsertOnSubmit(s);
                    data.SubmitChanges();
                    return RedirectToAction("Index");
                }
                    return this.Create();
        }


        public ActionResult Edit(int id)
        {
             var E_Products = data.Products.First(m => m.Id == id);
             return View(E_Products);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
             var E_Products = data.Products.First(m => m.Id == id);
             var E_ProductName = collection["ProductName"];
             var E_Image = collection["Image"];
             var E_Detail = collection["Detail"];
             var E_Time = collection["Time"];
             var E_Text = collection["Text"];
            var E_Amount = Convert.ToInt32(collection["Amount"]);
            var E_Price = Convert.ToDecimal(collection["Price"]);
             //var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycatnhat"]);
             //  var E_soluongton = Convert.ToInt32(collection["soluongton"]);
              E_Products.Id = id;
             if (string.IsNullOrEmpty(E_ProductName))
             {
               ViewData["Error"] = "Don't empty!";
             }
             else

             {
              E_Products.ProductName = E_ProductName;
              E_Products.Image = E_Image;
              E_Products.Detail = E_Detail;
              E_Products.Time = E_Time;
              E_Products.Text = E_Text;
              E_Products.Amount = E_Amount;
                E_Products.Price = E_Price;
                // E_Products.ngaycapnhat = E_ngaycapnhat;
              // E_Products.soluongton = E_soluongton;
                 UpdateModel(E_Products);
                data.SubmitChanges();
                return RedirectToAction("ListSach");
             }
                  return this.Edit(id);
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/Images/" + file.FileName));
            return "/Content/Images/" + file.FileName;
        }

        public ActionResult Delete(int id)
        {
            var D_sach = data.Products.First(m => m.Id == id);
            return View(D_sach);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_sach = data.Products.Where(m => m.Id == id).First();
            data.Products.DeleteOnSubmit(D_sach);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
 
        public ActionResult ListSach()
        {
            
           
            Customer kh = (Customer)Session["Taikhoan"];
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("XacNhanAdmin", "GioHang");

            }
            else
            {
                if (kh.limit == 1)
                {
                    var all_product = from ss in data.Products select ss;
                    return View(all_product);
                }
                else
                    return RedirectToAction("XacNhanAdmin", "GioHang");
            }   
                
            return View();

        }

        public ActionResult Nhom()
        {
            return PartialView();
        }
    }
   

}


    
