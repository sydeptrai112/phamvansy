using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDoOnline.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace ShopDoOnline.Areas.Admin.Controllers
{
    
    public class QuanLySanPhamController : Controller
    {
        // GET: Admin/QuanLySanPham
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 10;
            return View(db.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SanPham sp, FormCollection f, HttpPostedFileBase fFileUpLoad)
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoaiSP", "TenSP");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");

            if (fFileUpLoad == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                ViewBag.TenSP = f["TenSP"];
                ViewBag.MoTa = f["MoTa"];
                ViewBag.SoLuong = int.Parse(f["SoLuong"]);
                ViewBag.DonGia = decimal.Parse(f["DonGia"]);

                ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoaiSP", "TenSP", int.Parse(f["MaLoaiSP"]));
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", int.Parse(f["MaNSX"]));
                return View();

            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpLoad.FileName);
                    var path = Path.Combine(Server.MapPath("~/HinhAnhDoAn"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpLoad.SaveAs(path);
                    }

                    sp.TenSP = f["TenSP"];
                    sp.MoTa = f["MoTa"].Replace("<p>", "").Replace("</p>", "\n");
                    sp.HinhAnh = sFileName;
                    sp.NgayCapNhat = Convert.ToDateTime(f["NgayCapNhat"]);
                    sp.SoLuongTon = int.Parse(f["SoLuong"]);
                    sp.DonGia = decimal.Parse(f["DonGia"]);
                    sp.MaLoaiSP = int.Parse(f["MaLoaiSP"]);
                    sp.MaNSX = int.Parse(f["MaNSX"]);
                    db.SanPhams.InsertOnSubmit(sp);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            var sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {

                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {

                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.ChiTietDonDatHangs.Where(ct => ct.MaSP == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sản phẩm này đang có trong bảng chi tiết đặt hàng <br> " +
                    " Nếu muốn xóa thì phải xóa hết mã sản phẩm này trong bảng Chi tiết đặt hàng";
                return View(sp);
            }

            db.SanPhams.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            int ma = int.Parse(f["MaSP"]);
            var sp = db.SanPhams.SingleOrDefault(n => n.MaSP == ma);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", sp.MaNSX);
            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/HinhAnhDoAn"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sp.HinhAnh = sFileName;
                }




                sp.TenSP = f["TenSP"];
                sp.MoTa = f["MoTa"].Replace("<p>", "").Replace("</p>", "\n");

                sp.NgayCapNhat = Convert.ToDateTime(f["NgayCapNhat"]);
                sp.SoLuongTon = int.Parse(f["SoLuong"]);
                sp.DonGia = decimal.Parse(f["DonGia"]);
                sp.MaLoaiSP = int.Parse(f["MaLoaiSP"]);
                sp.MaNSX = int.Parse(f["MaNSX"]);


                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(sp);
        }
    }
}