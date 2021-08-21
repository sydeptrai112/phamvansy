using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDoOnline.Models;

namespace DoAn.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        //Lay gio hang
        public List<ItemGioHang> LayGioHang()
        {
            // Giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang == null)
            {
                // Nếu session giỏ hàng chưa tồn tại thì khởi tạo giỏ hàng 
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
                
            }
            return lstGioHang;
        }

        // Thêm giỏ hàng 
        public ActionResult ThemGioHang(int MaSP , string strURL)
        {
            // Kiểm tra sản phẩm có tồn tại trong csdl hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            // sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck!= null)
            {
                if(sp.SoLuongTon< spCheck.SoLuong)
                {
                    return PartialView("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return PartialView("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }
        // tính tổng số lượng
        public double TinhTongSoLuong()
        {
            // Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        // tính tổng tiền
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();

            return PartialView();
        }



        // GET: GioHang
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            /*if(lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }*/
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return View(lstGioHang);
        }
        public ActionResult XoaSPKhoiGioHang(int MaSP)
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang sp = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.MaSP == MaSP);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("XemGioHang");
        }
        /*public ActionResult CapNhatGioHang(int MaSP, FormCollection f)
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang sp = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp != null)
            {
                sp.SoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("XemGioHang");
        }*/

        public ActionResult XoaGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
       /* public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap?id=2");
            }

            if (Session["XemGioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<ItemGioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return View(lstGioHang);
        }*/
       /* [HttpPost]*/

       /* public ActionResult DatHang(FormCollection f)
        {
            DonDatHang ddh = new DonDatHang();
            ThanhVien tv = (ThanhVien)Session["TaiKhoan"];
            List<ItemGioHang> lstGioHang = LayGioHang();
            ddh.Ma = tv.MaThanhVien;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.TinhTrangGiaoHang = 1;
            ddh.DaThanhToan = false;
            db.DONDATHANGs.InsertOnSubmit(ddh);
            db.SubmitChanges();
            foreach (var item in lstGioHang)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CHITIETDATHANGs.InsertOnSubmit(ctdh);
            }

            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
*/

        // chỉnh sửa giỏ hàng
        public ActionResult SuaGioHang(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // Kiểm tra xem sản phẩm có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Lấy list giỏ hàng tạo giao diên
            ViewBag.GioHang = lstGioHang;
            // nếu tồn tại rồi
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH, FormCollection f)
        {
            // Kiểm tra số lượng tồn
            SanPham spCheck = db.SanPhams.SingleOrDefault(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return PartialView("ThongBao");
            }
            // cập nhật số lượng trong session Giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            itemGHUpdate.SoLuong = int.Parse(f["txtSoLuong"].ToString());
            /*itemGHUpdate.SoLuong = itemGH.SoLuong;*/
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang", new { @MaSP = itemGH.MaSP });
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TinhTongSoLuong();

            ViewBag.TongTien = TinhTongTien();
            return View(lstGioHang);
        }
        // xấy dựng chức năng đặt hàng

        [HttpPost]
        public ActionResult DatHang(KhachHang kh,FormCollection f)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                // thêm khách hàng vào bảng khách hàng đối với khách hàng chưa có tài khoản
                khang = new KhachHang();
                khang = kh;
                db.KhachHangs.InsertOnSubmit(khang);
                db.SubmitChanges();
            }

            else
            {
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.MaThanhVien = tv.MaThanhVien;
                db.KhachHangs.InsertOnSubmit(khang);
                db.SubmitChanges();
            }


            // Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
           // var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
           // ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.InsertOnSubmit(ddh);
            db.SubmitChanges();
            // Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;

                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.InsertOnSubmit(ctdh);

            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Index","Home");
        }



        /* [HttpGet]
         public ActionResult DatHang()
         {
             if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
             {
                 return Redirect("~/User/DangNhap?id=2");
             }

             if (Session["GioHang"] == null)
             {
                 return RedirectToAction("Index", "SachOnline");
             }

             List<ItemGioHang> lstGioHang = LayGioHang();
             ViewBag.TongSoLuong = TinhTongSoLuong();
             ViewBag.TongTien = TinhTongTien();
             return View(lstGioHang);
         }
         [HttpPost]

         public ActionResult DatHang(FormCollection f)
         {
             DonDatHang ddh = new DonDatHang();
             KhachHang kh = (KhachHang)Session["TaiKhoan"];
             List<ItemGioHang> lstGioHang = LayGioHang();
             ddh.MaKH = kh.MaKH;
             ddh.NgayDat = DateTime.Now;
             var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
             ddh.NgayGiao = DateTime.Parse(NgayGiao);
             ddh.TinhTrangGiaoHang = true;
             ddh.DaThanhToan = false;
             db.DONDATHANGs.InsertOnSubmit(ddh);
             db.SubmitChanges();
             foreach (var item in lstGioHang)
             {
                 CHITIETDATHANG ctdh = new CHITIETDATHANG();
                 ctdh.MaDonHang = ddh.MaDonHang;
                 ctdh.MaSach = item.iMaSach;
                 ctdh.SoLuong = item.iSoLuong;
                 ctdh.DonGia = (decimal)item.dDonGia;
                 db.CHITIETDATHANGs.InsertOnSubmit(ctdh);
             }

             db.SubmitChanges();
             Session["GioHang"] = null;
             return RedirectToAction("XacNhanDonHang", "GioHang");
         }
 */



    }
}