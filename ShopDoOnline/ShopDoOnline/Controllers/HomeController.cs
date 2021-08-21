using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDoOnline.Models;

namespace DoAn.Controllers
{
   /*[Authorize(Roles ="Admin,Mod")]*/
    public class HomeController : Controller
    {

        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        public ActionResult Index()
        {
            var lstNew = db.SanPhams.Where(n => n.MaLoaiSP == 1 );
            ViewBag.ListNew = lstNew;
            //jacket
            var lstNew1 = db.SanPhams.Where(n => n.MaLoaiSP == 2 );
            ViewBag.ListNew1 = lstNew1;
            //Hoodie
            var lstNew2 = db.SanPhams.Where(n => n.MaLoaiSP == 3 );
            ViewBag.ListNew2 = lstNew2;
            return View();
        }
        
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }
        public ActionResult MenuPartial()
        {

            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }

        public ActionResult SliderPartial()
        {
            return PartialView();
        }

        /*[HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            ViewBag.ThongBao = "Thêm Thành Công!!!";
            db.ThanhViens.Add(tv);
            db.SaveChanges();

            return View();
        }

       
        // load câu hỏi bí mật
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Cầu thủ mà bạn yêu thích?");
            lstCauHoi.Add("Tên cha bạn là gì?");
            return lstCauHoi;

        }*/

        /*  public ActionResult DangNhap()
          {
              return RedirectToAction("Index");
          }*/

       
    }
}