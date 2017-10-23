using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;

namespace PhatHanhSach.Controllers
{
    public class QuanLyNXBController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        // GET: QuanLyNXB
        public ActionResult Index()
        {
            var list = db.NHAXUATBANs.Where(n=>n.TrangThai == true);
            return View(list);
        }

        [HttpGet]
        public ActionResult ThemNXB()
        {
            return View();
        }

        public ActionResult ThemNXB(NHAXUATBAN nxb)
        {
            db.NHAXUATBANs.Add(nxb);
            db.SaveChanges();
            return RedirectToAction("Index", "QuanLyNXB");
        }

        [HttpGet]
        public ActionResult SuaNXB(int MaNXB)
        {
            if (MaNXB == null)
            {
                return HttpNotFound();
            }
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == MaNXB);
            if (nxb == null)
            {
                return HttpNotFound();
            }
            return View(nxb);
        }

        [HttpPost]
        public ActionResult SuaNXB(NHAXUATBAN nxb)
        {
            var n = db.NHAXUATBANs.SingleOrDefault(b => b.MaNXB == nxb.MaNXB);
            if (n != null)
            {
                n.Ten = nxb.Ten;
                n.DiaChi = nxb.DiaChi;
                n.SoDT = nxb.SoDT;
                n.SoTK = nxb.SoTK;
                n.TrangThai = nxb.TrangThai;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "QuanLyNXB");
        }

        public ActionResult XoaNXB(int MaNXB)
        {
            if (MaNXB == null)
            {
                return HttpNotFound();
            }
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == MaNXB);
            if (nxb == null)
            {
                return HttpNotFound();
            }
            else
            {
                nxb.TrangThai = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}