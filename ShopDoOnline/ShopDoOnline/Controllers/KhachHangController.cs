using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDoOnline.Models;

namespace DoAn.Controllers
{
    
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        public ActionResult Index()
        {
            var lstKH = from kh in db.KhachHangs select kh;
            return View(lstKH);
        }
        public ActionResult Index1()
        {
            var lstKH = from kh in db.KhachHangs select kh ;
            return View(lstKH);
        }

        public ActionResult TruyVan1DoiTuong()
        {
            // b1 : lấy danh sách khách hàng
            var lstKH = from kh in db.KhachHangs where kh.MaKH == 1 select kh;
            //b2 :
            // KhachHang khachhang = lstKH.FirstOrDefault();

            // Lấy đối tượng dựa trên phương thức hỗ trợ
            KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKH == 1);
            return View(khachhang);
        }


        public ActionResult GroupDuLieu()
        {
            List<ThanhVien> lstTV = db.ThanhViens.OrderByDescending(n => n.MaLoaiTV).ToList();
            return View(lstTV);
        }
        
    }
}