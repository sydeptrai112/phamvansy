using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ShopDoOnline.Models;

namespace DoAn.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }




        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection,ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            var sTaiKhoan = collection["TaiKhoan"];
            var sHoTen = collection["HoTen"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sSoDienThoai = collection["SoDienThoai"];

            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNL"];
            var sCauHoi = collection["CauHoi"];
            var sCauTraLoi = collection["CauTraLoi"];

            if (String.IsNullOrEmpty(sTaiKhoan))
            {
                ViewData["err1"] = "Tài khoản không được rỗng";
            }
            else if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err2"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDiaChi))
            {
                ViewData["err3"] = "Địa chỉ không được rỗng";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err4"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sSoDienThoai))
            {
                ViewData["err5"] = "Số điện thoại không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err6"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err7"] = "Phải nhập lại mật khẩu ";
            }
            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err7"] = "Mật khẩu nhập lại không khớp";
            }
            else if (String.IsNullOrEmpty(sCauTraLoi))
            {
                ViewData["err8"] = "Phải nhập câu trả lời ";
            }
            else if (db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.ThanhViens.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                tv.TaiKhoan = sTaiKhoan;
                tv.HoTen = sHoTen;
                tv.DiaChi = sDiaChi;
                tv.Email = sEmail;
                tv.SoDienThoai = sSoDienThoai;
                tv.MatKhau = sMatKhau;
                tv.CauHoi = sCauHoi;
                tv.CauTraLoi = sCauTraLoi;
                db.ThanhViens.InsertOnSubmit(tv);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }

            return this.DangKy();






            /* ViewBag.ThongBao = "Thêm Thành Công!!!";
             db.ThanhViens.Add(tv);
             db.SaveChanges();*/


        }


        // load câu hỏi bí mật
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Cầu thủ mà bạn yêu thích?");
            lstCauHoi.Add("Tên cha bạn là gì?");
            return lstCauHoi;

        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                TaiKhoan,
                DateTime.Now,
                DateTime.Now.AddHours(3),
                false,
                Quyen,
                FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            /* string TaiKhoan = f["TaiKhoan"].ToString();
             string MatKhau = f["MatKhau"].ToString();
             ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == TaiKhoan && n.MatKhau == MatKhau);
             if(tv!= null)
             {
                 IEnumerable<LoaiThanhVien_Quyen> lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                 string Quyen = "";
                 foreach(var item in lstQuyen)
                 {
                     Quyen += item.Quyen.TenQuyen + ",";
                 }
                 Quyen = Quyen.Substring(0, Quyen.Length - 1);
                 PhanQuyen(tv.TaiKhoan, Quyen);
                 Session["TaiKhoan"] = tv;
                 return RedirectToAction("Index", "Home");
             }
             return Content("Tài khoản hoặc mật khẩu không chính xác");*/


            /*int state = int.Parse(Request.QueryString["id"]);*/
            var sTenDN = f["TaiKhoan"];
            var sMatKhau = f["MatKhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (tv != null)
                {

                    ViewBag.ThongBao = "Đăng nhập thành công";
                    Session["TaiKhoan"] = tv;
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();

        }
       
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}