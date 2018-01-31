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
using System.IO;
using System.Drawing.Imaging;

namespace AforgeCamera
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Graphics g;
        Bitmap video;
        bool OnOf = false;
        int TolgahanMuhcı = 5;

        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;

        private void Form1_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevice) 
            {
                comboBox1.Items.Add(Device.Name);
            }
            comboBox1.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();
        }

        private void BtnBaşla_Click(object sender, EventArgs e)
        {
            FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();

        }

        void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            video = (Bitmap)eventArgs.Frame.Clone();
            Bitmap video2 = (Bitmap)eventArgs.Frame.Clone();
            if (OnOf == true)
            {
                g = Graphics.FromImage(video2);
                g.DrawString(TolgahanMuhcı.ToString(), new Font("Arial", 20), new SolidBrush(Color.White), new PointF(2, 2));
                g.Dispose();
            }
            pictureBox1.Image = video2;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalFrame.IsRunning == true)
            {
                FinalFrame.Stop();
            }
        }

        private void BtnResimÇek_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.Save(@"E:\C #DERSLER Açıklamalı\AforgeCamera\AforgeCamera\bin\Debug.bmp", ImageFormat.Bmp);

                MessageBox.Show("Resim Kayıt Edildi...");

            }
            else
            {
                MessageBox.Show("Kaydetme İşlemi Gerçekleşmedi Lütfen Tekrar Deneyiniz...");
            }
        }
        //-----------------------------------------Video2------------------------------------------------------------------------------
        private void BtnOtomatik_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            OnOf = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TolgahanMuhcı--;
            if (TolgahanMuhcı == 0)
            {
                timer1.Enabled = false;
                OnOf = false;
                pictureBox2.Image = video;
            }
        }
    }
}
