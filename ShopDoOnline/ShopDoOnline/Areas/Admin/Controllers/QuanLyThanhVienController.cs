using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDoOnline.Models;
using PagedList;
using PagedList.Mvc;
namespace ShopDoOnline.Areas.Admin.Controllers
{
    
    public class QuanLyThanhVienController : Controller
    {
        // GET: Admin/QuanLyKhachHang
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 10;
            return View(db.ThanhViens.ToList().OrderBy(n => n.MaThanhVien).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                Response.StatusCode = 404;
                return null;
            }

           
            return View(tv);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == int.Parse(f["iMaThanhVien"]));




            tv.TaiKhoan = f["sTaiKhoan"];
            tv.MatKhau = f["sMatKhau"];


            tv.HoTen = f["sHoTen"];
            tv.DiaChi = f["sDiaChi"];
            tv.Email = f["sEmail"];
            tv.SoDienThoai = f["sSDT"];
            tv.LoaiThanhVien.UuDai = int.Parse(f["UuDai"]);
               

                db.SubmitChanges();
                
            
            return View(tv);
        }
    }
}