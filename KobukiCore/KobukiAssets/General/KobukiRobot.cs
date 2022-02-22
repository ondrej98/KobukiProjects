using KobukiCore.KobukiAssets.General.Call;
using KobukiCore.KobukiAssets.General.Data;
using KobukiCore.KobukiAssets.General.Drive;
using KobukiCore.KobukiAssets.Lidar.Call;
using KobukiCore.KobukiAssets.Lidar.Data;
using KobukiCore.KobukiAssets.Skeleton.Call;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KobukiCore.KobukiAssets.General
{
    public class KobukiRobot : IKobukiRobot
    {
        #region Properties
        protected KobukiCall Kobuki { get; }
        protected LidarCall Lidar { get; }
        protected SkeletonCall Skeleton { get; }
        #endregion
        #region Constructors
        public KobukiRobot() { Kobuki = new KobukiCall(); Lidar = new LidarCall(); Skeleton = new SkeletonCall(); }

        public KobukiRobot(IKobukiDrive kobukiDrive) { Kobuki = new KobukiCall(kobukiDrive); Lidar = new LidarCall(); Skeleton = new SkeletonCall(); }
        #endregion
        #region Methods
        #region IKobukiCall
        public bool SetLedValues(int led1, int led2)
        {
            return Kobuki.SetLedValues(led1, led2);
        }
        public bool SetTranslationSpeed(int mmPerSec)
        {
            return Kobuki.SetTranslationSpeed(mmPerSec);
        }
        public bool SetRotationSpeed(double radPerSec)
        {
            return Kobuki.SetRotationSpeed(radPerSec);
        }
        public bool SetArcSpeed(int mmPerSec, int radius)
        {
            return Kobuki.SetArcSpeed(mmPerSec, radius);
        }
        public bool SetSound(int noteinHz, int duration)
        {
            return Kobuki.SetSound(noteinHz, duration);
        }
        public bool SetDefaultPid()
        {
            return Kobuki.SetDefaultPid();
        }
        public KobukiData GetData()
        {
            return Kobuki.GetData();
        }
        #endregion
        #region ILidarCall
        public bool Init()
        {
            return Lidar.Init();
        }
        public LaserMeasurement GetNewLaserMeasurement()
        {
            return Lidar.GetNewLaserMeasurement();
        }
        public LidarData GetNewLidarData()
        {
            return Lidar.GetNewLidarData();
        }
        public LidarData GetLidarData()
        {
            return Lidar.GetLidarData();
        }
        #endregion
        #region ISkeletonCall
        public Skeleton.Data.Skeleton GetSkeleton()
        {
            return Skeleton.GetSkeleton();
        }
        #endregion
        #region IKobukiRobot
        public async Task<bool> SetLedValuesAsync(int led1, int led2)
        {
            return await Task.Run(() => SetLedValues(led1, led2));
        }

        public async Task<bool> SetTranslationSpeedAsync(int mmPerSec)
        {
            return await Task.Run(() => SetTranslationSpeed(mmPerSec));
        }

        public async Task<bool> SetRotationSpeedAsync(double radPerSec)
        {
            return await Task.Run(() => SetRotationSpeed(radPerSec));
        }

        public async Task<bool> SetArcSpeedAsync(int mmPerSec, int radius)
        {
            return await Task.Run(() => SetArcSpeed(mmPerSec, radius));
        }

        public async Task<bool> SetSoundAsync(int noteinHz, int duration)
        {
            return await Task.Run(() => SetSound(noteinHz, duration));
        }

        public async Task<bool> SetDefaultPidAsync()
        {
            return await Task.Run(() => SetDefaultPid());
        }

        public async Task<KobukiData> GetDataAsync()
        {
            return await Task.Run(() => GetDataAsync());
        }

        public async Task<bool> InitAsync()
        {
            return await Task.Run(() => Init());
        }

        public async Task<LaserMeasurement> GetNewLaserMeasurementAsync()
        {
            return await Task.Run(() => GetNewLaserMeasurement());
        }

        public async Task<LidarData> GetNewLidarDataAsync()
        {
            return await Task.Run(() => GetNewLidarData());
        }

        public async Task<LidarData> GetLidarDataAsync()
        {
            return await Task.Run(() => GetLidarData());
        }

        public async Task<Skeleton.Data.Skeleton> GetSkeletonAsync()
        {
            return await Task.Run(() => GetSkeleton());
        }
        #endregion
        #endregion
    }
}
