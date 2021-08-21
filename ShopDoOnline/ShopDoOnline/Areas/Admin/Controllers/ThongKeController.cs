using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDoOnline.Models;

namespace ShopDoOnline.Areas.Admin.Controllers
{
    
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        public ActionResult Index()
        {
            ViewBag.PageView = HttpContext.Application["PageView"];
            ViewBag.TongDoanhThu = ThongKeDoanhThu();
            ViewBag.TongDDH = ThongKeDonHang();
            ViewBag.TongTV = ThongKeThanhVien();
            ViewBag.Online = HttpContext.Application["Online"];
            return View();
        }
        public double ThongKeDonHang()
        {
            var DDH = db.DonDatHangs.Count();
            return DDH;
        }
        public int ThongKeThanhVien()
        {
            var tv = db.ThanhViens.Count();
            return tv;
        }
        public decimal ThongKeDoanhThu()
        {
            decimal TongDoanhThu = db.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value;
            return TongDoanhThu;
        }
        public decimal ThongKeDoanhThuThang(int Thang, int Nam)
        {
            var DDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang && n.NgayDat.Value.Year == Nam);
            decimal TongTien = 0;
            foreach (var item in DDH)
            {
                TongTien += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return TongTien;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null) db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}