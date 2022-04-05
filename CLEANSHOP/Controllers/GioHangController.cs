    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLEANSHOP.Models;
using CLEANSHOP.Controllers;

namespace CLEANSHOP.Controllers
{
    public class GioHangController : Controller
    {
        MydataDataContext data = new MydataDataContext();

        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;

            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.ID == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        private int TongSoluong()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.iSoLuong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.TotalPrice);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();

        }
        public ActionResult XoaGiohang(int id)
        {
            List<Giohang> lstGiohang = Laygiohang();

            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.ID == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.ID == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.ID == id);
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(collection["txtSoLg"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }



        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "DichVu");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            Cart dh = new Cart();
            Customer kh = (Customer)Session["Taikhoan"];
                Product s = new Product();
            List<Giohang> gh = Laygiohang();
           var DeliveryDate = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.Customer_Id = kh.IdCustomer;
            dh.BookingDate = DateTime.Now;
            dh.DeliveryDate = DateTime.Parse(DeliveryDate);
            dh.Delivery = false;
            dh.TotalPrice = false;
            data.Carts.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                CartDetail ctdh = new CartDetail();
                ctdh.IdCart = dh.IdCart;
                ctdh.IdProduct= item.ID;
                ctdh.Amount = item.iSoLuong;
                ctdh.Price = (decimal)item.DisPrice;
                s = data.Products.Single(n => n.Id  == item.ID);
                s.Amount -= ctdh.Amount;
                data.SubmitChanges();
                data.CartDetails.InsertOnSubmit(ctdh);  
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacnhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {

            return View();
        }
        public ActionResult XacNhanAdmin()
        {

            return View();
        }
        public ActionResult Ten()
        {

            return PartialView();


        }

    }
}
