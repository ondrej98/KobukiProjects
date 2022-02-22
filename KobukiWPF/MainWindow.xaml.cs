using KobukiCore.KobukiAssets.Lidar.Data;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace KobukiWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        #region Properties
        private static bool IsCameraShowAllow = false;
        private static bool IsLidarShowAllow = false;
        private static bool IsSkeletonShowAllow = false;
        private readonly VideoCapture capture;
        private readonly BackgroundWorker bkgWorkerDisplayOptions;
        private readonly BackgroundWorker bkgWorkerDataDisplay;
        #endregion
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            capture = new VideoCapture();
            bkgWorkerDisplayOptions = new BackgroundWorker { WorkerSupportsCancellation = true };
            bkgWorkerDisplayOptions.DoWork += WorkerDisplayOptions_DoWork;
            bkgWorkerDataDisplay = new BackgroundWorker { WorkerSupportsCancellation = true };
            bkgWorkerDataDisplay.DoWork += WorkerDataDisplay_DoWork;

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }
        #endregion
        #region Methods
        private void DisplayLidarData(LaserMeasurement laserMeasurement)
        {
            if (laserMeasurement != null)
            {

            }
        }

        private void ChangeVisibility(UIElement uIElement, Visibility visibility)
        {
            if (uIElement != null)
            {
                lock (uIElement)
                {
                    uIElement.Visibility = visibility;
                }
            }
        }

        private void CheckBoxCheckedHandle(CheckBox checkBox)
        {
            if (checkBox != null)
            {
                if (checkBox.Equals(checkBoxShowCamera))
                {
                    IsCameraShowAllow = checkBoxShowCamera.IsChecked.HasValue ? checkBoxShowCamera.IsChecked.Value : false;
                    ChangeVisibility(ucPaintData.cameraImage, IsCameraShowAllow ? Visibility.Visible : Visibility.Hidden);
                }
                else if (checkBox.Equals(checkBoxShowLidar))
                {
                    IsLidarShowAllow = checkBoxShowLidar.IsChecked.HasValue ? checkBoxShowLidar.IsChecked.Value : false;
                    ChangeVisibility(ucPaintData.paintCanvasLidar, IsLidarShowAllow ? Visibility.Visible : Visibility.Hidden);
                }
                else if (checkBox.Equals(checkBoxShowSkeleton))
                {
                    IsSkeletonShowAllow = checkBoxShowSkeleton.IsChecked.HasValue ? checkBoxShowSkeleton.IsChecked.Value : false;
                    ChangeVisibility(ucPaintData.paintCanvasSkeleton, IsCameraShowAllow ? Visibility.Visible : Visibility.Hidden);
                }
            }
        }

        private void CheckBoxUncheckedHandle(CheckBox checkBox)
        {
            if (checkBox != null)
            {
                if (checkBox.Equals(checkBoxShowCamera))
                {
                    IsCameraShowAllow = checkBoxShowCamera.IsChecked.HasValue ? checkBoxShowCamera.IsChecked.Value : false;
                    ChangeVisibility(ucPaintData.cameraImage, IsCameraShowAllow ? Visibility.Visible : Visibility.Hidden);
                }
                else if (checkBox.Equals(checkBoxShowLidar))
                {
                    IsLidarShowAllow = checkBoxShowLidar.IsChecked.HasValue ? checkBoxShowLidar.IsChecked.Value : false;
                    ChangeVisibility(ucPaintData.paintCanvasLidar, IsLidarShowAllow ? Visibility.Visible : Visibility.Hidden);
                }
                else if (checkBox.Equals(checkBoxShowSkeleton))
                {
                    IsSkeletonShowAllow = checkBoxShowSkeleton.IsChecked.HasValue ? checkBoxShowSkeleton.IsChecked.Value : false;
                    ChangeVisibility(ucPaintData.paintCanvasSkeleton, IsCameraShowAllow ? Visibility.Visible : Visibility.Hidden);
                }
            }
        }

        private void ButtonTotalStopHandle()
        {
            App.RobotService.SetRotationSpeed(0);
            App.RobotService.SetRotationSpeed(0);
        }

        private void ButtonRobotControlHandle(Button button, System.Windows.Input.KeyEventArgs e)
        {
            if (button != null)
            {
                if ((e == null && button.Equals(BtnForward)) || (e != null && e.Key == System.Windows.Input.Key.W))
                {
                    App.RobotService.SetTranslationSpeed(50);
                }
                else if ((e == null && button.Equals(BtnBackWard)) || (e != null && e.Key == System.Windows.Input.Key.S))
                {
                    App.RobotService.SetTranslationSpeed(-50);
                }
                else if ((e == null && button.Equals(BtnLeft)) || (e != null && e.Key == System.Windows.Input.Key.A))
                {
                    App.RobotService.SetRotationSpeed(0.5);
                }
                else if ((e == null && button.Equals(BtnRight)) || (e != null && e.Key == System.Windows.Input.Key.D))
                {
                    App.RobotService.SetRotationSpeed(-0.5);
                }
            }
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            capture.Open(App.CameraCaptureLink);
            if (!capture.IsOpened())
            {
                Close();
                return;
            }
            bkgWorkerDisplayOptions.RunWorkerAsync();
            bkgWorkerDataDisplay.RunWorkerAsync();
            CheckBoxCheckedHandle(checkBoxShowCamera);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            bkgWorkerDisplayOptions.CancelAsync();
            bkgWorkerDataDisplay.CancelAsync();
            Thread.Sleep(500);
            capture.Dispose();
        }

        private void WorkerDisplayOptions_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                Dispatcher.Invoke(() =>
                {
                    lock (ucPaintData.cameraImage)
                    {
                        if (IsCameraShowAllow)
                        {
                            lock (capture)
                            {
                                using (var frameMat = capture.RetrieveMat())
                                {
                                    ucPaintData.cameraImage.Source = frameMat.ToWriteableBitmap();
                                }
                            };
                        }
                        else
                        {
                            ucPaintData.cameraImage.Source = null;
                        }
                    }
                    lock (ucPaintData.paintCanvasLidar)
                    {
                        var laserMeasurement = IsLidarShowAllow ? App.RobotService.LidarDataMeasurements.GetNewestLaserMeasurement() : null;
                        ucPaintData.PaintLaserMeasurement(laserMeasurement);
                    }
                    lock (ucPaintData.paintCanvasSkeleton)
                    {
                        var skeleton = IsSkeletonShowAllow ? App.RobotService.SkeletonData : null;
                        ucPaintData.PaintSkeleton(skeleton);
                    }
                });
                Thread.Sleep(80);
            }
            if (!capture.IsDisposed)
                capture.Release();
        }

        private void WorkerDataDisplay_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                Dispatcher.Invoke(() =>
                {
                    ucKobukiSimpleData.UpdateTextBoxes(App.RobotService.LastKobukiSimpleData);
                });
                Thread.Sleep(500);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateLayout();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
                CheckBoxCheckedHandle(checkBox);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
                CheckBoxUncheckedHandle(checkBox);
        }

        private void BtnTotalStop_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(BtnTotalStop))
                ButtonTotalStopHandle();
        }
        private void BtnTotalStop_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender.Equals(BtnTotalStop) && e.Key == System.Windows.Input.Key.Space)
                ButtonTotalStopHandle();
        }

        private void BtnRobotControl_Clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
                ButtonRobotControlHandle(button, null);
        }

        private void BtnRobotControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is Button button)
                ButtonRobotControlHandle(button, e);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                ButtonTotalStopHandle();
            else
                ButtonRobotControlHandle(BtnTotalStop, e);
        }
        #endregion
    }
}
