using KobukiCore.KobukiAssets.Lidar.Data;
using KobukiCore.KobukiAssets.Skeleton.Data;
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

namespace KobukiWPF.UserControls
{
    /// <summary>
    /// Interaction logic for UserControlPaintData.xaml
    /// </summary>
    public partial class UserControlPaintData : UserControl
    {
        public UserControlPaintData()
        {
            InitializeComponent();
        }

        private Ellipse CreateEllipse(double width, double height, Brush stroke, Brush fill, double x = 0.0, double y = 0.0)
        {
            var result = new Ellipse();
            result.Stroke = stroke;
            result.Fill = fill;
            //circle.HorizontalAlignment = HorizontalAlignment.Left;
            //circle.VerticalAlignment = VerticalAlignment.Bottom;
            result.Width = width;
            result.Height = height;
            Canvas.SetLeft(result, x);
            Canvas.SetTop(result, y);
            result.Visibility = Visibility.Visible;
            return result;
        }

        private Ellipse PaintCenterEllipse(double width, double height, Brush stroke, Brush fill, double x = 0.0, double y = 0.0)
        {
            var result = CreateEllipse(width, height, stroke, fill);
            Canvas.SetLeft(result, x - result.Width / 2);
            Canvas.SetTop(result, y - result.Height / 2);
            return result;
        }

        private Rectangle PaintCenterLine(double width, double height, Brush stroke, Brush fill, double x = 0.0, double y = 0.0)
        {
            var result = new Rectangle();
            result.Stroke = stroke;
            result.Fill = fill;
            //circle.HorizontalAlignment = HorizontalAlignment.Left;
            //circle.VerticalAlignment = VerticalAlignment.Bottom;
            result.Width = width;
            result.Height = height;
            result.Visibility = Visibility.Visible;
            Canvas.SetLeft(result, x - result.Width / 2);
            Canvas.SetTop(result, y - result.Height);
            return result;
        }

        public void PaintLaserMeasurement(LaserMeasurement laserMeasurement)
        {
            paintCanvasLidar.Children.Clear();
            if (laserMeasurement != null)
            {
                double f = 628.036;
                double imgWidth = (int)paintCanvasLidar.ActualWidth;
                double imgWidth2 = imgWidth % 2 == 0 ? imgWidth / 2 : imgWidth / 2 + 1;
                double imgHeight = (int)paintCanvasLidar.ActualHeight;
                double imgHeight2 = imgHeight % 2 == 0 ? imgHeight / 2 : imgHeight / 2 + 1;
                double Y = 0.06;
                double xpCenter = imgWidth - (imgWidth2 + 0);
                double ypCenter = imgHeight - (imgHeight2 + 0);
                var circleCenter = PaintCenterEllipse(28, 28, Brushes.Black, Brushes.DarkRed, x: xpCenter, y: ypCenter);
                paintCanvasLidar.Children.Add(circleCenter);
                var line = PaintCenterLine(2, 10, Brushes.Black, Brushes.DarkBlue, x: xpCenter, y: ypCenter);
                paintCanvasLidar.Children.Add(line);
                for (int i = 0; i < laserMeasurement.NumberOfScans; i++)
                {
                    int dist = (int)(laserMeasurement.Data[i].ScanDistance / 15);
                    var angleDeg = (360.0 - laserMeasurement.Data[i].ScanAngle) * Math.PI / 180.0;
                    double Z = dist * Math.Cos(angleDeg);
                    double X = dist * Math.Sin(angleDeg);
                    //double xp = (imgWidth / 2) - ((f * X) / Z);
                    //double yp = (imgHeight / 2) - ((f * Y) / Z);
                    double xp = imgWidth - (imgWidth2 + X);
                    double yp = imgHeight - (imgHeight2 + Z);
                    if (xp < imgWidth && xp > 0 && yp < imgHeight && yp > 0)
                    {
                        var ellipse = CreateEllipse(3, 3, Brushes.DarkBlue, Brushes.DarkBlue, x: xp, y: yp);
                        paintCanvasLidar.Children.Add(ellipse);
                    }
                }
            }
        }

        public void PaintSkeleton(Skeleton skeleton)
        {
            paintCanvasSkeleton.Children.Clear();
            if (skeleton != null)
            {
                double imgWidth = (int)paintCanvasLidar.ActualWidth;
                double imgWidth2 = imgWidth % 2 == 0 ? imgWidth / 2 : imgWidth / 2 + 1;
                double imgHeight = (int)paintCanvasLidar.ActualHeight;
                double imgHeight2 = imgHeight % 2 == 0 ? imgHeight / 2 : imgHeight / 2 + 1;
                for (int i = 0; i < skeleton.Joints.Count; i++)
                {
                    double xp = imgWidth - (imgWidth * skeleton.Joints[i].X);
                    double yp = imgHeight - (imgHeight * skeleton.Joints[i].Y);
                    if (xp < imgWidth && xp > 0 && yp < imgHeight && yp > 0)
                    {
                        var ellipse = CreateEllipse(10, 10, Brushes.DarkRed, Brushes.DarkRed, x: xp, y: yp);
                        paintCanvasSkeleton.Children.Add(ellipse);
                    }
                }
            }
        }
    }
}
