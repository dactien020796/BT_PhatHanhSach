using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhatHanhSach.Models.ViewModels
{
    public class CT_PhieuNhapViewModel
    {
        public int MaSach { get; set; }
        public String TenSach { get; set; }
        public int SLNhap { get; set; }
        public int DonGia { get; set; }
        public int ThanhTien { get; set; }
    }
}