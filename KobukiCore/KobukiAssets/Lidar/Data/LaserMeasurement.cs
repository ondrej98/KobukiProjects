using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.Lidar.Data
{
    public class LaserMeasurement
    {
        #region Properties
        public int NumberOfScans { get; set; }
        public List<LaserData> Data { get; set; }
        #endregion
        #region Constructors
        public LaserMeasurement()
        {
            Data = new List<LaserData>();
        }
        #endregion
    }
}
