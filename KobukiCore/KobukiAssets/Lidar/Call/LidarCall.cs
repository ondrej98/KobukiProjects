using KobukiCore.Calls;
using KobukiCore.KobukiAssets.Lidar.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.Lidar.Call
{
    public class LidarCall : CallBase, ILidarCall
    {
        #region Properties
        public const int InitNewMeasurementCount = 3;
        protected LidarData Data { get; } = new LidarData();
        public override string Hostname => "127.0.0.1";
        public override int PortRecieve => 52999;
        public override int PortSend => 5299;
        #endregion
        #region Constructors
        public LidarCall() : base() { }
        #endregion
        #region Methods

        public LaserMeasurement GetNewLaserMeasurement()
        {
            var data = ReadStateOnly();
            return LidarData.TryParseBytesIntoLaserMeasurement(data);
        }

        public LidarData GetLidarData()
        {
            return Data;
        }

        public LidarData GetNewLidarData()
        {
            var newMeasurement = GetNewLaserMeasurement();
            Data.AddNewLaserMeasurement(newMeasurement);
            return Data;
        }

        public bool Init()
        {
            WriteCommandOnly(new byte[] { 0x00 });
            for (int i = 0; i < InitNewMeasurementCount; i++)
            {
                var newMeasurement = GetNewLaserMeasurement();
                Data.AddNewLaserMeasurement(newMeasurement);
            }
            return Data.CountAllLaserMeasurement() == InitNewMeasurementCount;
        }
        #endregion
    }
}
