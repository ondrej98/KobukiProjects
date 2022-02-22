using KobukiCore.KobukiAssets.General.Data;
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
    /// Interaction logic for UserControlKobukiSimpleData.xaml
    /// </summary>
    public partial class UserControlKobukiSimpleData : UserControl
    {
        public KobukiSimpleData KSimpleData { get; set; }
        public UserControlKobukiSimpleData()
        {
            InitializeComponent();
            UpdateTextBoxes(KSimpleData);
        }
        public void UpdateTextBoxes(KobukiSimpleData kSimpleData)
        {
            textBoxTimetamp.Text = kSimpleData?.Timestamp?.ToString();
            textBoxEncoderLeft.Text = kSimpleData?.EncoderLeft?.ToString();
            textBoxEncoderRight.Text = kSimpleData?.EncoderRight?.ToString();
            textBoxGyroAngle.Text = kSimpleData?.GyroAngle?.ToString();
            textBoxGyroAngleRate.Text = kSimpleData?.GyroAngleRate?.ToString();
        }
    }
}
