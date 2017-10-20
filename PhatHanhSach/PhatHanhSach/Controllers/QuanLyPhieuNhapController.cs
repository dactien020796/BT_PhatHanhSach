using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;

namespace PhatHanhSach.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();

        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNXB = db.NHAXUATBANs;
            ViewBag.ListSach = db.SACHes;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PHIEUNHAP model, IEnumerable<CT_PHIEUNHAP> lstModel)
        {
            ViewBag.MaNXB = db.NHAXUATBANs;
            ViewBag.ListSach = db.SACHes;
            return View();
        }
    }
}