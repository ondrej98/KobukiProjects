using KobukiCore.KobukiAssets.Lidar.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.Lidar.Call
{
    public interface ILidarCall
    {
        public bool Init();

        public LaserMeasurement GetNewLaserMeasurement();

        public LidarData GetNewLidarData();

        public LidarData GetLidarData();
    }
}
