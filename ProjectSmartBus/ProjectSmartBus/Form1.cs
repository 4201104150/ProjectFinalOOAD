using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;
using MessagingToolkit;
using WebCam_Capture;
using System.Data.SqlClient;
using System.Configuration;

namespace ProjectSmartBus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataSmartBusDataContext db = new DataSmartBusDataContext();

        SqlConnection con;

        int tien = 0;
        int dem = 0;
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        MessagingToolkit.QRCode.Codec.QRCodeEncoder QRCodeEncoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BarcodeReader Reader = new BarcodeReader();
            Result result = Reader.Decode((Bitmap)pictureBox1.Image);
            try
            {
                string decoded = result.ToString().Trim();
                if (decoded != "")
                {
                    timer1.Stop();
                    label2.Text = decoded;
                }
            }
            catch (Exception ex)
            {
            }
        }
        public int kiemtra(string a)
        {
            string s1id = "SELECT ID FROM ThongTinKhachHang";
            SqlCommand cmd = new SqlCommand(s1id, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (a == dr[0].ToString())
                    {
                        dr.Close();
                        return 1;
                    }
                }
            }
            dr.Close();
            return 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer4.Start();
            label2.Hide();
            string conString = ConfigurationManager.ConnectionStrings["smartbus"].ConnectionString.ToString();
            con = new SqlConnection(conString);
            con.Open();
            //----
            comboBox1.Hide();
            textBox1.Enabled = false;
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevice)
            {
                comboBox1.Items.Add(Device.Name);
            }

            comboBox1.SelectedIndex = 0;
            //FinalFrame = new VideoCaptureDevice();

            FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalFrame.IsRunning == true)
            {
                FinalFrame.Stop();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_TextChanged(object sender, EventArgs e)
        {

            ThongTinKhachHang thongTin = db.ThongTinKhachHangs.SingleOrDefault(p => p.ID == label2.Text);

            if (thongTin == null)
            {
                if ((dem) % 2 == 0)
                {
                    MessageBox.Show("Mời quét lại !");
                    label2.Hide();
                   
                }
            }
            else
            {
                
                int check;
                label2.Show();
                KhachHang kh = new KhachHang(label2.Text, thongTin.HoTen, thongTin.NgaySinh.ToString(), thongTin.GioiTinh, thongTin.SDT, int.Parse(thongTin.SoTienHienTai.ToString()), int.Parse(thongTin.CheckHanhDong.ToString()));
                textBox1.Text = kh.ToString();
                tien = thongTin.SoTienHienTai;
                timer2.Start();//quét lần 1
                check = int.Parse(thongTin.CheckHanhDong.ToString());
                if(check==1)//quét lần nữa
                {
                    timer3.Start();
                }
            }

            //string ten = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.HoTen).ToString();
            //string ngaysinh = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.NgaySinh).ToString();
            //string gioitinh = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.GioiTinh).ToString();
            //string SDT = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.SDT).ToString();
            //string tien = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.SoTienHienTai).ToString();
            //string checkHD = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.CheckHanhDong).ToString();

            //label1.Text = kh.ToString();

            /*
            
            else
            {*/

            /*
            //Mã hóa ID
            MD5 mh = MD5.Create();
            //Chuyển kiểu chuổi thành kiểu byte
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(maquet.Text);
            //mã hóa chuỗi đã chuyển
            byte[] hash = mh.ComputeHash(inputBytes);
            //tạo đối tượng StringBuilder (làm việc với kiểu dữ liệu lớn)
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            //End Mã Hóa 

            //Form1 fmb = new Form1(maquet.Text);
            //fmb.FormClosed += new FormClosedEventHandler(fmb_FormClosed);
            //fmb.Show();
            //this.Hide();
            textBox1.Text = thongTin.ToString();
        }*/

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            string sql = "UPDATE ThongTinKhachHang SET CheckHanhDong = 1 WHERE ID = @maid";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@maid", SqlDbType.VarChar).Value = label2.Text;
            cmd.ExecuteNonQuery();
            timer5.Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            string sql1 = "UPDATE ThongTinKhachHang SET CheckHanhDong = 0, SoTienHienTai = @tien1 - 2000 WHERE ID = @maid";
            SqlCommand cmd1 = new SqlCommand(sql1, con);
            cmd1.Parameters.Add("@maid", SqlDbType.VarChar).Value = label2.Text;
            cmd1.Parameters.Add("@tien1", SqlDbType.Int).Value = tien;
            cmd1.ExecuteNonQuery();
            timer3.Stop();
            timer5.Start();
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            string sql2 = "UPDATE ThongTinKhachHang SET CheckHanhDong = 0, SoTienHienTai = SoTienHienTai-6000 WHERE CheckHanhDong = 1";
            SqlCommand cmd2 = new SqlCommand(sql2, con);
            cmd2.ExecuteNonQuery();
            timer4.Stop();
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            timer5.Stop();
            dem++;
            if(dem%2==0)
            {
                dem++;
            }
            label2.Text = null;
            textBox1.Text = null;
            
        }
    }
}
