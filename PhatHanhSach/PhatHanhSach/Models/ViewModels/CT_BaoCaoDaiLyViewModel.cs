using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhatHanhSach.Models.ViewModels
{
    public class CT_BaoCaoDaiLyViewModel
    {
        public int MaSach { get; set; }
        public String TenSach { get; set; }
        public int SoLuongBan { get; set; }
        public int DonGiaBan { get; set; }
        public int ThanhTien { get; set; }
    }
}