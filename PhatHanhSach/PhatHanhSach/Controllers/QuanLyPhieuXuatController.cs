using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;
using PhatHanhSach.Models.ViewModels;
using System.Globalization;

namespace PhatHanhSach.Controllers
{
    public class QuanLyPhieuXuatController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        public static DateTime date = DateTime.Now;
        public static String daily;

        public ActionResult Index()
        {
            var list = db.PHIEUXUATs;
            return View(list);
        }

        [HttpGet]
        public ActionResult XuatSach()
        {

            if (Session["DS_Sach"] == null)
                Session["DS_Sach"] = new List<CT_PhieuXuatViewModel>();

            ViewBag.DS_DaiLy = new SelectList(db.DAILies.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
            return View();
        }

        [HttpPost]
        public ActionResult XuatSach(DAILY dl, FormCollection f)
        {
            PHIEUXUAT px = new PHIEUXUAT();
            px.MaDL = int.Parse(f["MaDL"].ToString());
            String[] temp = f["NgayXuat"].ToString().Split('-');
            DateTime date = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            px.NgayXuat = date;
            px.TrangThai = false;
            db.PHIEUXUATs.Add(px);
            db.SaveChanges();

            int? TongTien = 0;
            foreach (CT_PhieuXuatViewModel ct in Session["DS_Sach"] as List<CT_PhieuXuatViewModel>)
            {
                CT_PHIEUXUAT ctpx = new CT_PHIEUXUAT();
                ctpx.MaPX = px.MaPX;
                ctpx.MaSach = ct.MaSach;
                ctpx.SLXuat = ct.SLXuat;
                ctpx.DonGia = ct.DonGia;
                ctpx.ThanhTien = ctpx.SLXuat * ctpx.DonGia;
                TongTien += ctpx.ThanhTien;
                db.CT_PHIEUXUAT.Add(ctpx);
            }
            px.TongTien = TongTien;
            db.SaveChanges();

            CONGNO_DL congno = new CONGNO_DL();
            congno.MaDL = dl.MaDL;
            congno.ThoiGian = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            congno.TienDaTra = 0;
            congno.TienNo = px.TongTien;
            db.CONGNO_DL.Add(congno);
            db.SaveChanges();
            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Search(string prefix)
        {
            var sach = (from s in db.SACHes
                        where s.TenSach.Contains(prefix) && s.TrangThai == true
                        select new
                        {
                            label = s.TenSach,
                            val = s.MaSach
                        }).ToList();

            return Json(sach);
        }

        [HttpPost]
        public ActionResult ThemChiTiet(SACH sach, FormCollection f)
        {
            SACH s = db.SACHes.SingleOrDefault(n => n.MaSach == sach.MaSach);
            CT_PhieuXuatViewModel ctpx = new CT_PhieuXuatViewModel();
            ctpx.MaSach = s.MaSach;
            ctpx.TenSach = s.TenSach;
            ctpx.DonGia = int.Parse(f["DonGia"]);
            ctpx.SLXuat = int.Parse(f["SLXuat"]);
            ctpx.ThanhTien = ctpx.DonGia * ctpx.SLXuat;
            ((List<CT_PhieuXuatViewModel>)Session["DS_Sach"]).Add(ctpx);
            try
            {
                LuuBienDungChung(f);
                LuuViewBag();
            }
            catch (Exception)
            {
                ViewBag.DS_DaiLy = new SelectList(db.DAILies.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
            }
            return View("XuatSach");
        }

        public ActionResult XoaChiTiet(int MaSach, FormCollection f)
        {
            ((List<CT_PhieuXuatViewModel>)Session["DS_Sach"]).RemoveAll(p => p.MaSach == MaSach);
            try
            {
                LuuViewBag();
            }
            catch (Exception)
            {
                ViewBag.DS_DaiLy = new SelectList(db.DAILies.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
            }
            return View("XuatSach");
        }

        public void LuuBienDungChung(FormCollection f)
        {
            String[] temp = f["NgayXuat"].ToString().Split('-');
            date = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            daily = f["MaDL"].ToString();
        }

        public void LuuViewBag()
        {
            ViewBag.DatePicker = date.ToString("dd-MM-yyyy");
            ViewBag.DaiLy = daily;
            ViewBag.DS_DaiLy = new SelectList(db.DAILies.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
        }

        public ActionResult XemChiTiet(int? MaPX)
        {
            var px = db.PHIEUXUATs.SingleOrDefault(n => n.MaPX == MaPX);
            ViewBag.DS_CTPhieuXuat = px.CT_PHIEUXUAT;
            return View(px);
        }
    }
}