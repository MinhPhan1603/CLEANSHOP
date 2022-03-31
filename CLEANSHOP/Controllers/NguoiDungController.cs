using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLEANSHOP.Models;

namespace CLEANSHOP.Controllers
{
    public class NguoiDungController : Controller
    {
        MydataDataContext data = new MydataDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, Customer kh)
        {
            var Name = collection["Name"];
            var LoginName = collection["LoginName"];
            var Password = collection["Password"];
            var ConfirmPassword = collection["ConfirmPassword"];
            var Email = collection["Email"];
            var Address = collection["Address"];
            var Phone = collection["Phone"];
            var limit = Convert.ToInt32(collection["limit"]);
            var DateofBirth = String.Format("{0:MM/dd/yyyy}", collection["DateofBirth"]);
            if (String.IsNullOrEmpty(ConfirmPassword))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
            }
            else
            {
                if (!Password.Equals(ConfirmPassword))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";

                }
                else
                {
                    kh.Name = Name;
                    kh.LoginName = LoginName;
                    kh.Password = Password;
                    kh.Email = Email;
                    kh.Address = Address;
                    kh.Phone = Phone;
                    kh.limit = limit;
                    kh.DateofBirth = DateTime.Parse(DateofBirth);
                    data.Customers.InsertOnSubmit(kh);
                    data.SubmitChanges();
                    return RedirectToAction("DangNhap");
                }
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var LoginName = collection["LoginName"];
            var Password = collection["Password"];
            var limit = Convert.ToInt32(collection["limit"]);
            Customer kh = data.Customers.SingleOrDefault(n => n.LoginName == LoginName && n.Password == Password && n.limit >= 0);
            if (kh != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["Taikhoan"] = kh;
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return RedirectToAction("DatHang", "Giohang");

        }

    }
}
