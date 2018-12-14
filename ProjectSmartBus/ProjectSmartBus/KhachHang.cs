using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSmartBus
{
    public class KhachHang
    {
        public string maKH { get; set; }
        public string tenKH { get; set; }
        public string ngaySinh { get; set; }
        public string gioiTinh { get; set; }
        public string SDT { get; set; }
        public int soTien { get; set; }
        public int checkHanhDong { get; set; }

        public KhachHang()
        {
        }

        public KhachHang(string maKH, string tenKH, string ngaySinh, string gioiTinh, string sDT, int soTien, int checkHanhDong)
        {
            this.maKH = maKH;
            this.tenKH = tenKH;
            this.ngaySinh = ngaySinh;
            this.gioiTinh = gioiTinh;
            SDT = sDT;
            this.soTien = soTien;
            this.checkHanhDong = checkHanhDong;
        }

        public override string ToString()
        {
            return "ID: "+maKH+"\n Tên khách hàng: "+tenKH+"\nNgày sinh: "+ngaySinh+"\nGiới tính: "+gioiTinh+"\nSố điện thoại: "+SDT+"\nSố tiền hiện có: "+soTien;
        }

        public int tinhTien()
        {
            int Tien;
            if (checkHanhDong > 1)//có quét lại thẻ
            {
                Tien = soTien - 2000;
            }
            else//không quét lại thẻ
            {
                Tien = soTien - 6000;
            }
            return Tien;
        }

    }
}
