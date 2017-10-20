using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;

namespace PhatHanhSach.Controllers
{
    public class QuanLyPhieuXuatController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        public static List<SACH> list;
        // GET: QuanLyPhieuXuat
        public ActionResult XuatSach()
        {
            list = new List<SACH>();
            return View();
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
        public ActionResult ThemChiTiet(SACH sach)
        {
            SACH s = db.SACHes.SingleOrDefault(n => n.MaSach == sach.MaSach);
            list.Add(s);
            Session["DS_Sach"] = list;
            return View("XuatSach");
        }
        
        public ActionResult XoaChiTiet(int MaSach)
        {
            list.RemoveAll(p => p.MaSach == MaSach);
            Session["DS_Sach"] = list;
            return View("XuatSach");
        }
    }
}