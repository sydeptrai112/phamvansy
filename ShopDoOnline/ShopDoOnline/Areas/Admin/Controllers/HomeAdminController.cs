using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDoOnline.Models;
namespace ShopDoOnline.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/Home
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["UserName"];
            var sMatKhau = f["PassWord"];

            Admin1 ad = db.Admin1s.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);

            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "QuanLySanPham");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}