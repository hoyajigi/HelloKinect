using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Kinect;

namespace WpfApplication2
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor Kinect;
        
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { DIscoverKinectSensor(); };
            Kinect.ColorStream.Enable();
            Kinect.ColorFrameReady += sensor_ColorFrameReady;
            Kinect.Start();

        }

        void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var x=e.OpenColorImageFrame())
            {
                if (x == null) return;
                byte[] colorData=new byte[x.PixelDataLength];
                x.CopyPixelDataTo(colorData);
                KinectVideo.Source = BitmapSource.Create(x.Width, x.Height, 96, 96, PixelFormats.Bgr32, null,colorData,x.Width * x.BytesPerPixel);
            }
        }

        private void DIscoverKinectSensor()
        {
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            this.Kinect = KinectSensor.KinectSensors.FirstOrDefault(x => x.Status == KinectStatus.Connected);
        }

        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Connected:
                    if (this.Kinect == null)
                    {
                        this.Kinect = e.Sensor;
                    }
                    break;
                case KinectStatus.DeviceNotGenuine:
                    break;
                case KinectStatus.DeviceNotSupported:
                    break;
                case KinectStatus.Disconnected:
                    if (this.Kinect == e.Sensor)
                    {
                        this.Kinect = null;

                    }
                    break;
                case KinectStatus.Error:
                    break;
                case KinectStatus.Initializing:
                    break;
                case KinectStatus.InsufficientBandwidth:
                    break;
                case KinectStatus.NotPowered:
                    break;
                case KinectStatus.NotReady:
                    break;
                case KinectStatus.Undefined:
                    break;
                default:
                    break;
            }
        }
    }
}
