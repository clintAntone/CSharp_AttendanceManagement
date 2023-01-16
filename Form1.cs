using AForge.Video.DirectShow;
using AForge.Video;
using System;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq;
using System.Drawing.Imaging;

namespace AttendanceManagement
{

    public partial class Form1 : Form
    {
        private VideoCaptureDevice videoSource;
        public Form1()
        {
            InitializeComponent();
            SelectDevice();
        }
        private void SelectDevice()
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
                throw new ApplicationException("No video capture devices detected.");

            // add devices to combo box
            foreach (FilterInfo device in videoDevices)
            {
                comboBox1.Items.Add(device.Name);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy\nhh:mm:ss tt");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //stop the current video source
            if (videoSource != null && videoSource.IsRunning)
                videoSource.Stop();

            //get the selected device
            string device = comboBox1.SelectedItem.ToString();
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            var deviceMoniker = videoDevices.OfType<FilterInfo>().FirstOrDefault(x => x.Name == device).MonikerString;

            //create video source
            videoSource = new VideoCaptureDevice(deviceMoniker);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.Start();
        }
    }
}
