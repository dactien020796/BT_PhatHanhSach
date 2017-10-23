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

        //[HttpGet]
        //public ActionResult XuatSach()
        //{

        //    if (Session["DS_Sach"] == null)
        //        Session["DS_Sach"] = new List<SACH>();

        //    ViewBag.DS_DaiLy = new SelectList(db.DAILies.ToList(), "MaDL", "Ten");
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult XuatSach(DAILY dl, FormCollection f)
        //{
        //    PHIEUXUAT px = new PHIEUXUAT();
        //    px.MaDL = dl.MaDL;
        //    //px.NgayXuat = f["NgayXuat"].;
        //    px.TrangThai = false;
        //    db.PHIEUXUATs.Add(px);
        //    db.SaveChanges();

        //    int? TongTien = 0;
        //    foreach (SACH s in Session["DS_Sach"] as List<SACH>)
        //    {
        //        CT_PHIEUXUAT ctpx = new CT_PHIEUXUAT();
        //        ctpx.MaPX = px.MaPX;
        //        ctpx.MaSach = s.MaSach;
        //        ctpx.SLXuat = 1;
        //        ctpx.DonGia = 100000;
        //        ctpx.ThanhTien = ctpx.SLXuat * ctpx.DonGia;
        //        TongTien += ctpx.ThanhTien;
        //        db.CT_PHIEUXUAT.Add(ctpx);
        //    }
        //    px.TongTien = TongTien;
        //    db.SaveChanges();

        //    CONGNO_DL congno = new CONGNO_DL();
        //    congno.MaDL = dl.MaDL;
        //    //congno.ThoiGian = f["NgayXuat"];
        //    congno.TienDaTra = 0;
        //    congno.TienNo = px.TongTien;
        //    db.CONGNO_DL.Add(congno);
        //    db.SaveChanges();
        //    return RedirectToAction("XuatSach");
        //}

        //[HttpPost]
        //public JsonResult Search(string prefix)
        //{
        //    var sach = (from s in db.SACHes
        //                     where s.TenSach.Contains(prefix)
        //                     select new
        //                     {
        //                         label = s.TenSach,
        //                         val = s.MaSach
        //                     }).ToList();

        //    return Json(sach);
        //}

        //[HttpPost]
        //public ActionResult ThemChiTiet(SACH sach, FormCollection f)
        //{
        //    SACH s = db.SACHes.SingleOrDefault(n => n.MaSach == sach.MaSach);
        //    CT_PHIEUXUAT ctpx = new CT_PHIEUXUAT();
        //    ctpx.MaSach = s.MaSach;
        //    ctpx.DonGia = int.Parse(f["DonGia"]);
        //    ctpx.SLXuat = int.Parse(f["SLXuat"]);
        //    ((List<CT_PHIEUXUAT>)Session["DS_Sach"]).Add(ctpx);
        //    ViewBag.DS_DaiLy = new SelectList(db.DAILies.ToList(), "MaDL", "Ten");
        //    return View("XuatSach");



        //    ((List<SACH>)Session["DS_Sach"]).Add(s);
        //    ViewBag.DS_DaiLy = new SelectList(db.DAILies.ToList(), "MaDL", "Ten");
        //    //return RedirectToAction("XuatSach");
        //    return View("XuatSach");
        //}

        //public ActionResult XoaChiTiet(int MaSach)
        //{
        //    //((List<CT_PHIEUXUAT>)Session["DS_Sach"]).RemoveAll(p => p.MaSach == MaSach);
        //    ((List<SACH>)Session["DS_Sach"]).RemoveAll(p => p.MaSach == MaSach);
        //    ViewBag.DS_DaiLy = new SelectList(db.DAILies.ToList(), "MaDL", "Ten");
        //    //return RedirectToAction("XuatSach");
        //    return View("XuatSach");
        //}

        [HttpGet]
        public ActionResult XuatSach()
        {

            if (Session["DS_Sach"] == null)
                Session["DS_Sach"] = new List<CT_PhieuXuatViewModel>();

            ViewBag.DS_DaiLy = new SelectList(db.DAILies.ToList(), "MaDL", "Ten");
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
            return RedirectToAction("XuatSach");
        }

        [HttpPost]
        public JsonResult Search(string prefix)
        {
            var sach = (from s in db.SACHes
                        where s.TenSach.Contains(prefix)
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
            ViewBag.DS_DaiLy = new SelectList(db.DAILies.ToList(), "MaDL", "Ten");
            return View("XuatSach");



            //((List<SACH>)Session["DS_Sach"]).Add(s);
            //ViewBag.DS_DaiLy = new SelectList(db.DAILies.ToList(), "MaDL", "Ten");
            ////return RedirectToAction("XuatSach");
            //return View("XuatSach");
        }

        public ActionResult XoaChiTiet(int MaSach)
        {
            //((List<CT_PHIEUXUAT>)Session["DS_Sach"]).RemoveAll(p => p.MaSach == MaSach);
            ((List<CT_PhieuXuatViewModel>)Session["DS_Sach"]).RemoveAll(p => p.MaSach == MaSach);
            ViewBag.DS_DaiLy = new SelectList(db.DAILies.ToList(), "MaDL", "Ten");
            //return RedirectToAction("XuatSach");
            return View("XuatSach");
        }
    }
}