using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
//using AForge;
using System.IO;
using System.Drawing.Imaging;

namespace Camera
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection devices;
        private VideoCaptureDevice videoSource;
        private Random rndNumber = new Random();
        private bool btnStartStopRec = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Gets the available video devices
            devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo device in devices)
            {
                comboBox.Items.Add(device.Name);
            }

            // Makes the first item in combobox the default selected item
            comboBox.SelectedIndex = 0;

            videoSource = new VideoCaptureDevice();

        }

        // Camera start
        private void btnStart_Click(object sender, EventArgs e)
        {
            videoSource = new VideoCaptureDevice(devices[comboBox.SelectedIndex].MonikerString);

            // Set NewFrame event handler
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);

            videoSource.Start();
        }

        // NewFrame event handler
        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            pictureBox.Image = image;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource.IsRunning == true)
            {
                videoSource.Stop();
            }
        }

        // Camera stop
        private void btnStop_Click(object sender, EventArgs e)
        {
            videoSource.Stop();
            pictureBox.Image = null;
            pictureBox.Invalidate();
        }

        // Image capturing
        private void btnImgCapture_Click(object sender, EventArgs e)
        {
            int randomNumber = rndNumber.Next(-2147483648, 2147483647);

            if (videoSource.IsRunning == false)
            {
                string message = "You must start the camera first!";
                string caption = "Camera ERROR";

                MessageBoxButtons button = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, button);
            }
            else
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string baseFolderPath = $@"{currentDirectory}\Images";

                if (!Directory.Exists(baseFolderPath))
                {
                    Directory.CreateDirectory(baseFolderPath);

                    string message = "A folder has been created!";
                    string caption = "Folder Creation";

                    MessageBoxButtons button = MessageBoxButtons.OK;

                    MessageBox.Show(message, caption, button);
                }

                //Save First
                Bitmap varBmp = new Bitmap(pictureBox.Image);
                Bitmap newBitmap = new Bitmap(varBmp);

                pictureBox1.Image = newBitmap;

                varBmp.Save($@"{currentDirectory}\Images\Img{randomNumber}.png", ImageFormat.Png);

                //Dispose to free the memory
                varBmp.Dispose();
                varBmp = null;

            }
        }

        // Video recording
        private void btnStartStopRecord_Click(object sender, EventArgs e)
        {
            if (videoSource.IsRunning == false)
            {
                string message = "You must start the camera first!";
                string caption = "Camera ERROR";

                MessageBoxButtons button = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, button);
            }
            else
            {
                // Start recording
                if (btnStartStopRec == false)
                {
                    btnStartStopRec = true;
                    panel1.BackColor = Color.LightGreen;
                    //TODO: Start recording logic
                }

                // Stop recording
                else
                {
                    btnStartStopRec = false;
                    panel1.BackColor = Color.Red;
                    //TODO: Stop recording logic
                }
            }
        }
    }
}
