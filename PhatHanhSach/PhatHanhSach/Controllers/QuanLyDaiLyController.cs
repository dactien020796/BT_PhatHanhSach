using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;

namespace PhatHanhSach.Controllers
{
    public class QuanLyDaiLyController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        // GET: QuanLyDaiLy
        public ActionResult Index()
        {
            var list = db.DAILies.Where(n=>n.TrangThai == true);
            return View(list);
        }

        [HttpGet]
        public ActionResult ThemDaiLy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemDaiLy(DAILY dl)
        {
            db.DAILies.Add(dl);
            db.SaveChanges();
            return RedirectToAction("Index", "QuanLyDaiLy");
        }

        [HttpGet]
        public ActionResult SuaDaiLy(int MaDL)
        {
            if (MaDL == null)
            {
                return HttpNotFound();
            }
            var dl = db.DAILies.SingleOrDefault(n => n.MaDL == MaDL);
            if (dl == null)
            {
                return HttpNotFound();
            }
            return View(dl);
        }

        [HttpPost]
        public ActionResult SuaDaiLy(DAILY dl)
        {
            var daily = db.DAILies.SingleOrDefault(n => n.MaDL == dl.MaDL);
            if (daily != null)
            {
                daily.Ten = dl.Ten;
                daily.DiaChi = dl.DiaChi;
                daily.SoDT = dl.SoDT;
                daily.TrangThai = dl.TrangThai;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "QuanLyDaiLy");
        }

        public ActionResult XoaDaiLy(int MaDL)
        {
            if (MaDL == null)
            {
                return HttpNotFound();
            }
            var dl = db.DAILies.SingleOrDefault(n => n.MaDL == MaDL);
            if (dl == null)
            {
                return HttpNotFound();
            }
            else
            {
                dl.TrangThai = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}