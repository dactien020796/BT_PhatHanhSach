using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;

namespace PhatHanhSach.Controllers
{
    public class BaoCaoDaiLyController : Controller
    {
        // GET: BaoCaoDaiLy
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        List<BAOCAODL> list;
        public static DateTime Ngay = DateTime.Now;
        public ActionResult Index()
        {
            //Hiện danh sach bao cao
            var list = db.BAOCAODLs;
            return View(list);
        }

    }
}