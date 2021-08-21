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
    
    public class QuanLyKhachHangController : Controller
    {
       
        // GET: Admin/QuanLyKhachHang
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 10;
            return View(db.KhachHangs.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
        }
    }
}