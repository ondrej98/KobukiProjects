using KobukiCore.KobukiAssets.General.Call;
using KobukiCore.KobukiAssets.General.Data;
using KobukiCore.KobukiAssets.Lidar.Call;
using KobukiCore.KobukiAssets.Lidar.Data;
using KobukiCore.KobukiAssets.Skeleton.Call;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KobukiCore.KobukiAssets.General
{
    public interface IKobukiRobot : IKobukiCall, ILidarCall, ISkeletonCall
    {
        public Task<bool> SetLedValuesAsync(int led1, int led2);
        public Task<bool> SetTranslationSpeedAsync(int mmPerSec);
        public Task<bool> SetRotationSpeedAsync(double radPerSec);
        public Task<bool> SetArcSpeedAsync(int mmPerSec, int radius);
        public Task<bool> SetSoundAsync(int noteinHz, int duration);
        public Task<bool> SetDefaultPidAsync();
        public Task<KobukiData> GetDataAsync();
        public Task<bool> InitAsync();
        public Task<LaserMeasurement> GetNewLaserMeasurementAsync();
        public Task<LidarData> GetNewLidarDataAsync();
        public Task<LidarData> GetLidarDataAsync();
        public Task<Skeleton.Data.Skeleton> GetSkeletonAsync();
    }
}
