using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;
using System.IO;

namespace PhatHanhSach.Controllers
{
    public class QuanLySachController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        // GET: QuanLySach
        public ActionResult Index()
        {
            var list = db.SACHes;
            return View(list);
        }

        [HttpGet]
        public ActionResult ThemSach()
        {
            return View();
        }

        [HttpPost]  
        public ActionResult ThemSach(SACH s, HttpPostedFileBase HinhAnh)
        {
            //Kiểm tra tên hình có tồn tại chưa
            if (HinhAnh.ContentLength > 0)
            {
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);

                //Nếu tên hình đã tồn tại thì xuất ra
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình đã tồn tại";
                    return View();
                }
                else
                {
                    //Lấy hình quăng vô folder HinhAnhSP
                    HinhAnh.SaveAs(path);
                    s.HinhAnh = fileName;
                }
            }
            db.SACHes.Add(s);
            db.SaveChanges();
            return RedirectToAction("Index", "QuanLySach");
        }

        [HttpGet]
        public ActionResult SuaSach(int MaSach)
        {
            if (MaSach == null)
            {
                return HttpNotFound();
            }
            var sp = db.SACHes.SingleOrDefault(n => n.MaSach == MaSach);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);
        }

        [HttpPost]
        public ActionResult SuaSach(SACH s, HttpPostedFileBase HinhAnh)
        {
            if (HinhAnh == null)
            {
                s.HinhAnh = ViewBag.HinhAnh;
            }
            //Kiểm tra tên hình có tồn tại chưa
            else 
            {
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);

                //Nếu tên hình đã tồn tại thì xuất ra
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình đã tồn tại";
                    return View();
                }
                else
                {
                    //Lấy hình quăng vô folder HinhAnhSP
                    HinhAnh.SaveAs(path);
                    s.HinhAnh = fileName;
                }
            }

            var result = db.SACHes.SingleOrDefault(b => b.MaSach == s.MaSach);
            if (result != null)
            {
                result.TenSach = s.TenSach;
                result.GhiChu = s.GhiChu;
                result.HinhAnh = s.HinhAnh;
                result.LinhVuc = s.LinhVuc;
                result.TacGia = s.TacGia;
                result.TrangThai = s.TrangThai;
                result.DonGiaNhap = s.DonGiaNhap;
                result.DonGiaXuat = s.DonGiaXuat;

                db.SaveChanges();
            }
            return RedirectToAction("Index", "QuanLySach");
        }
    }
}