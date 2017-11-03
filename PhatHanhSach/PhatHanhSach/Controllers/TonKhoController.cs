using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;

namespace PhatHanhSach.Controllers
{
    public class TonKhoController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        List<TONKHO> list;
        public static DateTime Ngay = DateTime.Now;

        public ActionResult Index()
        {
            //Hiện tồn kho tất cả sách
            list = new List<TONKHO>();
            TonKhoTatCa();
            return View();
        }

        public ActionResult TonKhoMotSach(int? MaSach, FormCollection f)
        {
            list = new List<TONKHO>();
            String[] temp = f["datepicker"].ToString().Split('-');
            Ngay = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            if (f["MaSach"].ToString().Equals(""))
                TonKhoTatCa();
            else
                TonKho1Sach(MaSach, Ngay);
            return View("Index");
        }

        public void TonKhoTatCa()
        {
            foreach (SACH s in db.SACHes)
            {
                TONKHO tk = db.TONKHOes.Where(n => n.MaSach == s.MaSach && n.ThoiGian <= Ngay && n.SLTon != 0 && n.TangGiam != 0).OrderByDescending(n => n.ThoiGian).FirstOrDefault();
                list.Add(tk);
            }
            ViewBag.TonKho = list;
            ViewBag.Ngay = Ngay.ToString("dd-MM-yyyy");
        }

        public void TonKho1Sach(int? MaSach, DateTime Ngay)
        {
            TONKHO tk = db.TONKHOes.Where(n => n.MaSach == MaSach && n.ThoiGian <= Ngay && n.SLTon != 0 && n.TangGiam != 0).OrderByDescending(n => n.ThoiGian).FirstOrDefault();
            list.Add(tk);
            ViewBag.TonKho = list;
            ViewBag.Ngay = Ngay.ToString("dd-MM-yyyy");
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
    }
}