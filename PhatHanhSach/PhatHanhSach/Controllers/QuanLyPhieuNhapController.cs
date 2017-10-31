using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;
using PhatHanhSach.Models.ViewModels;

namespace PhatHanhSach.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        public static DateTime date = DateTime.Now;
        public static String nxb;

        public ActionResult Index()
        {
            var list = db.PHIEUNHAPs;
            return View(list);
        }

        [HttpGet]
        public ActionResult NhapSach()
        {
            if (Session["DS_Sach_Nhap"] == null)
                Session["DS_Sach_Nhap"] = new List<CT_PhieuNhapViewModel>();

            ViewBag.DS_NXB = new SelectList(db.NHAXUATBANs.Where(n => n.TrangThai == true).ToList(), "MaNXB", "Ten");
            return View();
        }

        [HttpPost]
        public ActionResult NhapSach(NHAXUATBAN nxb, FormCollection f)
        {
            PHIEUNHAP pn = new PHIEUNHAP();
            pn.MaNXB = int.Parse(f["MaNXB"].ToString());
            String[] temp = f["NgayNhap"].ToString().Split('-');
            DateTime date = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            pn.NgayNhap = date;
            pn.TrangThai = false;
            db.PHIEUNHAPs.Add(pn);
            db.SaveChanges();

            int? TongTien = 0;
            foreach (CT_PhieuNhapViewModel ct in Session["DS_Sach_Nhap"] as List<CT_PhieuNhapViewModel>)
            {
                CT_PHIEUNHAP ctpx = new CT_PHIEUNHAP();
                ctpx.MaPN = pn.MaPN;
                ctpx.MaSach = ct.MaSach;
                ctpx.SLNhap = ct.SLNhap;
                ctpx.DonGia = ct.DonGia;
                ctpx.ThanhTien = ctpx.SLNhap * ctpx.DonGia;
                TongTien += ctpx.ThanhTien;
                db.CT_PHIEUNHAP.Add(ctpx);
            }
            pn.TongTien = TongTien;
            db.SaveChanges();

            CONGNO_NXB congno = new CONGNO_NXB();
            congno.MaNXB = nxb.MaNXB;
            congno.ThoiGian = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            congno.TienDaTra = 0;
            congno.TienNo = pn.TongTien;
            db.CONGNO_NXB.Add(congno);
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
            CT_PhieuNhapViewModel ctpx = new CT_PhieuNhapViewModel();
            ctpx.MaSach = s.MaSach;
            ctpx.TenSach = s.TenSach;
            ctpx.DonGia = int.Parse(f["DonGia"]);
            ctpx.SLNhap = int.Parse(f["SLNhap"]);
            ctpx.ThanhTien = ctpx.DonGia * ctpx.SLNhap;
            ((List<CT_PhieuNhapViewModel>)Session["DS_Sach_Nhap"]).Add(ctpx);
            try
            {
                LuuBienDungChung(f);
                LuuViewBag();
            }
            catch (Exception)
            {
                ViewBag.DS_NXB = new SelectList(db.NHAXUATBANs.Where(n => n.TrangThai == true).ToList(), "MaNXB", "Ten");
            }
            return View("NhapSach");
        }

        public ActionResult XoaChiTiet(int MaSach, FormCollection f)
        {
            ((List<CT_PhieuNhapViewModel>)Session["DS_Sach_Nhap"]).RemoveAll(p => p.MaSach == MaSach);
            try
            {
                LuuViewBag();
            }
            catch (Exception)
            {
                ViewBag.DS_NXB = new SelectList(db.NHAXUATBANs.Where(n => n.TrangThai == true).ToList(), "MaNXB", "Ten");
            }
            return View("NhapSach");
        }

        //Lưu biến ngày nhập và mã NXB dùng chung cho các action
        public void LuuBienDungChung(FormCollection f)
        {
            String[] temp = f["NgayNhap"].ToString().Split('-');
            date = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            nxb = f["MaNXB"].ToString();
        }

        //Lưu các giá trị vào viewbag để gửi qua view
        public void LuuViewBag()
        {
            ViewBag.DatePicker = date.ToString("dd-MM-yyyy");           
            ViewBag.NhaXuatBan = nxb;
            ViewBag.DS_NXB = new SelectList(db.NHAXUATBANs.Where(n => n.TrangThai == true).ToList(), "MaNXB", "Ten");
        }

        public ActionResult XemChiTiet(int? MaPN)
        {
            var pn = db.PHIEUNHAPs.SingleOrDefault(n => n.MaPN == MaPN);
            ViewBag.DS_CTPhieuNhap = pn.CT_PHIEUNHAP;
            return View(pn);
        }
    }
}