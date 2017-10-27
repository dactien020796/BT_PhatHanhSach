using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PhatHanhSach.Models
{
    public class Procedure
    {
        PhatHanhSachEntities entities = new PhatHanhSachEntities();
        public static void XOASACH(PhatHanhSachEntities entities, int maSach)
        {
            var param = new SqlParameter[]
            {
                new SqlParameter ("@MaSach", maSach)
            };
            var id = entities.Database.ExecuteSqlCommand("XOASACH @MaSach", param);
            Console.WriteLine("UPDATED: " + id);
        }
    }
}