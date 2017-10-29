using PhatHanhSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhatHanhSach.Controllers
{
    public class QuanLyCongNo_DLController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        // GET: QuanLyCongNo_DL
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CongNoDL(FormCollection f)
        {
            String[] temp = f["datepicker"].ToString().Split('-');
            DateTime date = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));

            List<CONGNO_DL> lst_congno_dl = new List<CONGNO_DL>();
            List<DAILY> lst_dl = new List<DAILY>();
            lst_dl = db.DAILies.ToList();
            foreach (DAILY dl in lst_dl)
            {
                CONGNO_DL congno_dl = new CONGNO_DL();
                congno_dl = db.CONGNO_DL.Where(x => x.ThoiGian <= date.Date && x.MaDL == dl.MaDL).OrderByDescending(x => x.ThoiGian).FirstOrDefault();
                if (congno_dl != null)
                {
                    congno_dl.TienNo = db.CONGNO_DL.Where(x => x.ThoiGian <= date.Date && x.MaDL == dl.MaDL).Sum(x => x.TienNo - x.TienDaTra);
                    lst_congno_dl.Add(congno_dl);
                }
            }
            ViewBag.NgayCongNo = date.ToString("dd/MM/yyyy");
            return View(lst_congno_dl);
        }
    }
}