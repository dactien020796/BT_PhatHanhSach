using PhatHanhSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhatHanhSach.Controllers
{
    public class QuanLyCongNo_NXBController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        // GET: QuanLyCongNo_DL
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CongNoNXB(FormCollection f)
        {
            String[] temp = f["datepicker"].ToString().Split('-');
            DateTime date = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));

            List<CONGNO_NXB> lst_congno_nxb = new List<CONGNO_NXB>();
            List<NHAXUATBAN> lst_nxb = new List<NHAXUATBAN>();
            lst_nxb = db.NHAXUATBANs.ToList();
            foreach (NHAXUATBAN n in lst_nxb)
            {
                CONGNO_NXB congno_nxb = new CONGNO_NXB();
                congno_nxb = db.CONGNO_NXB.Where(x => x.ThoiGian <= date.Date && x.MaNXB == n.MaNXB).OrderByDescending(x => x.ThoiGian).FirstOrDefault();
                if (congno_nxb != null)
                {
                    congno_nxb.TienNo = db.CONGNO_NXB.Where(x => x.ThoiGian <= date.Date && x.MaNXB == n.MaNXB).Sum(x => x.TienNo - x.TienDaTra);
                    lst_congno_nxb.Add(congno_nxb);
                }
            }
            ViewBag.NgayCongNo = date.ToString("dd/MM/yyyy");
            return View(lst_congno_nxb);
        }
    }
}