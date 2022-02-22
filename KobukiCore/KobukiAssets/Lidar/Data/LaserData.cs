using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.Lidar.Data
{
    public class LaserData
    {
        #region Properties
        public const int Size = 4+4/*fake bytes from source*/+8+8;
        public int ScanQuality { get; set; }
        public double ScanAngle { get; set; }
        public double ScanDistance { get; set; }
        #endregion
    }
}
