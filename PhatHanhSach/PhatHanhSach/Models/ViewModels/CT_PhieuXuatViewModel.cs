using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhatHanhSach.Models.ViewModels
{
    public class CT_PhieuXuatViewModel
    {
        public int MaSach { get; set; }

        public String TenSach { get; set; }
        public int SLXuat { get; set; }
        public int DonGia { get; set; }
        public int ThanhTien { get; set; }
    }
}