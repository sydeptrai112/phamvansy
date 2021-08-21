using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDoOnline.Models;
using System.Net;
using PagedList;
using PagedList.Mvc;


namespace DoAn.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        /*public ActionResult Index()
        {
            return View();
        }
        public ActionResult SanPham1()
        {
            var lstSanPham = db.SanPhams.Where(n => n.Moi == 1 && n.MaLoaiSP == 1);
            ViewBag.ListSP = lstSanPham;

            var lstHd = db.SanPhams.Where(n => n.MaLoaiSP == 4);
            ViewBag.ListHd = lstHd;

            return View();

        }
        

        // tạo partialview
        public ActionResult SanPhamPartial()
        {
            //var lstSanPham = db.SanPhams.Where(n => n.Moi == 1);
            //return PartialView(lstSanPham);
            return PartialView();
        }*/
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }

        public ActionResult SanPhamStyle2Partial()
        {
            // Tshirt

            return PartialView();
        }

        public ActionResult BestSellerPartial()
        {
            return PartialView();
        }

        //chi tiết sản phẩm
        public ActionResult XemChiTiet(int ?id,string tensp)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);

        }

        public ActionResult SanPham(int ? MaLoaiSP,int ?MaNSX,int ?page)
        {
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Load sản phẩm dựa theo 2 tiêu chí là mã loại sản phẩm và mã nhà sản xuất
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            
            if(lstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            //thực hiện chức năng phân trang
            int PageSize = 6;

            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            
            return View(lstSP.OrderBy(n => n.DonGia).ToPagedList(PageNumber,PageSize));
        }
    }


}