using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDoOnline.Models;
namespace ShopDoOnline.Areas.Admin.Controllers
{
    public class TimKiemAdminController : Controller
    {
        // GET: Admin/TimKiemAdmin
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        public ActionResult Search(string strSearch)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                //var kq = from s in db.SACHes select s;
                var kq = from s in db.SanPhams where s.TenSP.Contains(strSearch) select s;
                // var kq = from s in db.SACHes where s.MaCD == int.Parse(strSearch) select s;
                //var kq = from s in db.SACHes where s.SoLuongBan >= 5 && s.SoLuongBan <= 10 select s;

                // var kq = from s in db.SACHes
                //var kq = from s in db.SACHes where s.SoLuongBan >= 5 && s.SoLuongBan <= 10 orderby s.SoLuongBan ascending select s;
                //var kq = from s in db.SACHes where s.SoLuongBan >= 5 && s.SoLuongBan <= 10 orderby s.SoLuongBan descending select s;
                return View(kq);
            }
            return View();
        }
    }
}