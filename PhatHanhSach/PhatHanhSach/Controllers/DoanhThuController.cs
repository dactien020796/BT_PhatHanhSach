using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;
using PhatHanhSach.Models.ViewModels;

namespace PhatHanhSach.Controllers
{
    public class DoanhThuController : Controller
    {
        // GET: DoanhThu
        PhatHanhSachEntities db = new PhatHanhSachEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThongKe(FormCollection f)
        {
            String[] bd = f["TuNgay"].ToString().Split('-');
            DateTime datebd = new DateTime(int.Parse(bd[2]), int.Parse(bd[1]), int.Parse(bd[0]));

            String[] kt = f["DenNgay"].ToString().Split('-');
            DateTime datekt = new DateTime(int.Parse(kt[2]), int.Parse(kt[1]), int.Parse(kt[0]));

            //List<PHIEUXUAT> list = new List<PHIEUXUAT>();
            //var px = (from s in db.PHIEUXUATs
            //          where (s.NgayXuat >= datebd && s.NgayXuat <= datekt)
            //          select 
            //          ).ToList();
            var px = db.PHIEUXUATs.Where(n => n.NgayXuat >= datebd && n.NgayXuat <= datekt).ToList();
            //ViewBag.ThongKe = new SelectList(db.PHIEUXUATs.Where(n => n.NgayXuat >= datebd && n.NgayXuat <= datekt).ToList());

            return View(px);
        }
    }
}