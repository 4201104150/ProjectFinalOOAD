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

namespace ProjectSmartBus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataSmartBusDataContext db = new DataSmartBusDataContext();

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
                    label1.Text = decoded;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            label2.Text = "";
            timer1.Enabled = true;
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_TextChanged(object sender, EventArgs e)
        {
            ThongTinKhachHang thongTin = db.ThongTinKhachHangs.SingleOrDefault(p => p.ID == label2.Text);
            if(thongTin!=null)
            {
                thongTin.CheckHanhDong = 1;
            }
            db.SubmitChanges();
            string ten = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.HoTen).ToString();
            string ngaysinh = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.NgaySinh).ToString();
            string gioitinh = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.GioiTinh).ToString();
            string SDT = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.SDT).ToString();
            string tien = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.SoTienHienTai).ToString();
            string checkHD = db.ThongTinKhachHangs.Where(p => p.ID == label2.Text).Select(p => p.CheckHanhDong).ToString();

            KhachHang kh = new KhachHang(label2.Text,ten,ngaysinh,gioitinh,SDT,int.Parse(tien),int.Parse(checkHD));
            textBox1.Text = kh.ToString();
        }
    }
}
