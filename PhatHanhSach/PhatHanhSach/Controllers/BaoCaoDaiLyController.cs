using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhatHanhSach.Models;
using PhatHanhSach.Models.ViewModels;

namespace PhatHanhSach.Controllers
{
    public class BaoCaoDaiLyController : Controller
    {
        // GET: BaoCaoDaiLy
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        public static DateTime date = DateTime.Now;
        public static DateTime datebd = DateTime.Now;
        public static DateTime datekt = DateTime.Now;
        public static String daily;

        public ActionResult Index()
        {
            //Hiện danh sach bao cao
            int result = KiemTraSLBaoCao(1, 1);
            var list = db.BAOCAODLs;
            return View(list);
        }

        [HttpGet]
        public ActionResult ThemBaoCao()
        {
            if (Session["DS_BaoCao_DaiLy"] == null)
                Session["DS_BaoCao_DaiLy"] = new List<CT_BaoCaoDaiLyViewModel>();

            ViewBag.DS_DL = new SelectList(db.DAILies.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
            return View();
        }

        [HttpPost]
        public ActionResult ThemBaoCao(DAILY dl, FormCollection f)
        {
            BAOCAODL bc = new BAOCAODL();
            bc.MaDL = int.Parse(f["MaDL"].ToString());
            bc.NgayLap = date;
            bc.NgayBD = datebd;
            bc.NgayKT = datekt;
            bc.TinhTrang = true;
            db.BAOCAODLs.Add(bc);
            db.SaveChanges();

            int? TongTien = 0;
            foreach (CT_BaoCaoDaiLyViewModel ct in Session["DS_BaoCao_DaiLy"] as List<CT_BaoCaoDaiLyViewModel>)
            {
                CT_BAOCAODL ctbc = new CT_BAOCAODL();
                ctbc.MaBCDL = bc.MaBCDL;
                ctbc.MaSach = ct.MaSach;
                ctbc.SoLuongBan = ct.SoLuongBan;
                ctbc.DonGiaBan = ct.DonGiaBan;
                ctbc.ThanhTien = ctbc.SoLuongBan * ctbc.DonGiaBan;
                TongTien += ctbc.ThanhTien;
                db.CT_BAOCAODL.Add(ctbc);
            }
            bc.ThanhToan = TongTien;
            db.SaveChanges();

            CONGNO_DL congno = new CONGNO_DL();
            congno.MaDL = int.Parse(f["MaDL"].ToString());
            congno.ThoiGian = date;
            congno.TienDaTra = bc.ThanhToan;
            congno.TienNo = 0;
            db.CONGNO_DL.Add(congno);
            db.SaveChanges();
            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Search(string prefix, string MaDL)
        {
            //(s.MaSach == db.CT_PHIEUXUAT.FirstOrDefault(c => c.MaPX == (db.PHIEUXUATs.FirstOrDefault(p => p.MaDL == 1).MaPX)).MaSach))
            //.Where(db.PHIEUXUATs.Where(p => p.MaDL == 1).Select(p => p.MaPX).Contains())
            //db.CT_PHIEUXUAT.Select(c=>c.MaSach).Contains(s.MaSach))
            int ma = int.Parse(MaDL);
            var sach = (from s in db.SACHes
                        where (s.TenSach.Contains(prefix) && (from c in db.CT_PHIEUXUAT
                                                              where (db.PHIEUXUATs.Where(p => p.MaDL == ma).Select(p => p.MaPX).Contains(c.MaPX))
                                                              select c.MaSach
                                                              ).Contains(s.MaSach))
                        select new
                        {
                            label = s.TenSach,
                            val = s.MaSach,
                            gianhap = s.DonGiaNhap
                        }).ToList();
            return Json(sach);
        }

        [HttpPost]
        public ActionResult ThemChiTiet(SACH sach, FormCollection f)
        {
            try
            {
                LuuBienDungChung(f);
                LuuViewBag();
            }
            catch (Exception)
            {
                ViewBag.DS_DL = new SelectList(db.DAILies.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
            }
            SACH s = db.SACHes.SingleOrDefault(n => n.MaSach == sach.MaSach);
            CT_BaoCaoDaiLyViewModel ctpx = new CT_BaoCaoDaiLyViewModel();
            ctpx.MaSach = s.MaSach;
            ctpx.TenSach = s.TenSach;
            ctpx.DonGiaBan = int.Parse(f["DonGia"]);
            ctpx.SoLuongBan = int.Parse(f["SoLuongBan"]);
            ctpx.ThanhTien = ctpx.DonGiaBan * ctpx.SoLuongBan;
            int temp = KiemTraSLBaoCao(int.Parse(daily), ctpx.MaSach);
            if (ctpx.SoLuongBan <= KiemTraSLBaoCao(int.Parse(daily), ctpx.MaSach))
            {
                ((List<CT_BaoCaoDaiLyViewModel>)Session["DS_BaoCao_DaiLy"]).Add(ctpx);
            }
            else
            {
                //window.location.href = '/BaoCaoDaiLy/ThemBaoCao'
                return Content("<script>alert('SL sách báo cáo phải <= SL sách xuất - SL sách đã báo cáo'); window.location.href = '/BaoCaoDaiLy/ThemBaoCao'</script>");
            }
            return View("ThemBaoCao");
        }
        
        //Lưu biến ngày nhập và mã NXB dùng chung cho các action
        public void LuuBienDungChung(FormCollection f)
        {
            String[] temp = f["NgayLap"].ToString().Split('-');
            date = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            String[] tempbd = f["NgayBD"].ToString().Split('-');
            datebd = new DateTime(int.Parse(tempbd[2]), int.Parse(tempbd[1]), int.Parse(tempbd[0]));
            String[] tempkt = f["NgayKT"].ToString().Split('-');
            datekt = new DateTime(int.Parse(tempkt[2]), int.Parse(tempkt[1]), int.Parse(tempkt[0]));

            daily = f["MaDL"].ToString();
        }

        //Lưu các giá trị vào viewbag để gửi qua view
        public void LuuViewBag()
        {
            ViewBag.DatePicker = date.ToString("dd-MM-yyyy");
            ViewBag.DatePickerBD = datebd.ToString("dd-MM-yyyy");
            ViewBag.DatePickerKT = datekt.ToString("dd-MM-yyyy");
            ViewBag.NhaXuatBan = daily;
            ViewBag.DS_DL = new SelectList(db.DAILies.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
        }


        //ràng buộc SL báo cáo phải <= SL (đã xuất - đã báo cáo trước đó)
        public int KiemTraSLBaoCao(int MaDL, int MaSach)
        {
            //Lấy tổng SL của 1 sách mà ĐL đã báo cáo //(ctbc.MaBCDL == db.BAOCAODLs.FirstOrDefault(n => n.MaDL == MaDL).MaBCDL)
            var SQL_DaBaoCao = from ctbc in db.CT_BAOCAODL
                              where (ctbc.MaSach == MaSach) && db.BAOCAODLs.Where(p => p.MaDL == MaDL).Select(p => p.MaBCDL).Contains(ctbc.MaBCDL)
                               group ctbc by ctbc.MaSach into g
                              select g.Select(n => n.SoLuongBan).Sum();
            int SL_DaBaoCao;
            try
            {
                SL_DaBaoCao = Convert.ToInt32(SQL_DaBaoCao.FirstOrDefault().Value);
            }
            catch (Exception)
            {
                SL_DaBaoCao = 0;
            }


            //Lấy tổng số lượng của 1 sách mà ĐL đã đặt mua (mình đã xuất)
            var SQL_DaDat = from ctpx in db.CT_PHIEUXUAT
                              where (ctpx.MaSach == MaSach) &&  db.PHIEUXUATs.Where(p=>p.MaDL == MaDL).Select(p=>p.MaPX).Contains(ctpx.MaPX)
                              group ctpx by ctpx.MaSach into g
                              select g.Select(n => n.SLXuat).Sum();
            int SL_DaDat;
            try
            {
                SL_DaDat = Convert.ToInt32(SQL_DaDat.FirstOrDefault().Value);
            }
            catch (Exception)
            {
                SL_DaDat = 0;
            }
            return SL_DaDat - SL_DaBaoCao;
        }

        public ActionResult XoaChiTiet(int MaSach, FormCollection f)
        {
            try
            {
                LuuViewBag();
            }
            catch (Exception)
            {
                ViewBag.DS_DL = new SelectList(db.DAILies.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
            }
            ((List<CT_BaoCaoDaiLyViewModel>)Session["DS_BaoCao_DaiLy"]).RemoveAll(p => p.MaSach == MaSach);
            return View("ThemBaoCao");
        }

        public ActionResult XemChiTiet(int? MaBCDL)
        {
            var px = db.BAOCAODLs.SingleOrDefault(n => n.MaBCDL == MaBCDL);
            ViewBag.DS_CTBCDL = px.CT_BAOCAODL.ToList();
            return View(px);
        }
    }
}