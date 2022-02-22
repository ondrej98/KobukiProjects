using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.Lidar.Data
{
    public class LidarData
    {
        #region Properties
        public const int MaxLocalMeasurementCount = 10;
        protected List<LaserMeasurement> LocalMeasurements { get; }
        #endregion
        #region Constructors
        public LidarData()
        {
            LocalMeasurements = new List<LaserMeasurement>();
        }
        #endregion
        #region Methods
        public void AddNewLaserMeasurement(LaserMeasurement newLaserMeasurement)
        {
            if (newLaserMeasurement != null)
            {
                if (LocalMeasurements.Count >= MaxLocalMeasurementCount)
                    LocalMeasurements.RemoveAt(0);
                LocalMeasurements.Add(newLaserMeasurement);
            }
        }

        public LaserMeasurement GetNewestLaserMeasurement()
        {
            return (LocalMeasurements.Count > 0 ? LocalMeasurements[LocalMeasurements.Count - 1] : null);
        }

        public LaserMeasurement GetOldestLaserMeasurement()
        {
            return (LocalMeasurements.Count > 0 ? LocalMeasurements[0] : null);
        }

        public List<LaserMeasurement> GetAllLaserMeasurement()
        {
            return LocalMeasurements;
        }

        public int CountAllLaserMeasurement()
        {
            return LocalMeasurements.Count;
        }

        public static LaserMeasurement TryParseBytesIntoLaserMeasurement(byte[] data)
        {
            LaserMeasurement result = null;
            if (data != null)
            {
                try
                {
                    result = ParseBytesIntoLaserMeasurement(data);
                }
                catch (Exception)
                {
                    result = null;
                }
            }
            return result;
        }

        public static LaserMeasurement ParseBytesIntoLaserMeasurement(byte[] data)
        {
            LaserMeasurement result = null;
            if (data != null)
            {
                result = new LaserMeasurement();
                for (int i = 0; i < data.Length; i += LaserData.Size)
                {
                    var singleLaserData = new LaserData();
                    singleLaserData.ScanQuality = BitConverter.ToInt32(data, i + 0 * 4);
                    singleLaserData.ScanAngle = BitConverter.ToDouble(data, i + 1 * 8);
                    singleLaserData.ScanDistance = BitConverter.ToDouble(data, i + 2 * 8);
                    result.Data.Add(singleLaserData);
                }
                result.NumberOfScans = data.Length / LaserData.Size;
            }
            return result;
        }
        #endregion
    }
}
