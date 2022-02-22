using KobukiCore.KobukiAssets.General;
using KobukiCore.KobukiAssets.General.Data;
using KobukiCore.KobukiAssets.Lidar.Call;
using KobukiCore.KobukiAssets.Lidar.Data;
using KobukiCore.KobukiAssets.Skeleton.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KobukiCore.Services
{
    public class KobukiService : IKobukiService, IDisposable
    {
        #region Properties
        public const int MaxHistoryKobukiDataCount = 250;
        protected static bool ThreadsClosing = false;
        protected static bool IsWriting = false;
        protected static KobukiRobot Robot { get; } = new KobukiRobot();
        protected virtual TimeSpan ThreadJointTimeout { get; } = TimeSpan.FromSeconds(60);
        protected static List<KobukiData> HistoryKobukiData { get; } = new List<KobukiData>();
        protected static List<KobukiSimpleData> HistoryKobukiSimpleData { get; } = new List<KobukiSimpleData>();
        protected List<Thread> WorkerThreads { get; }
        protected bool EnableKobuki { get; private set; } = true;
        protected bool EnableSkeleton { get; private set; } = true;
        public KobukiData LastKobukiData { get; protected set; } = null;

        public KobukiSimpleData LastKobukiSimpleData { get; protected set; } = null;

        public LidarData LidarDataMeasurements { get; protected set; } = null;

        public Skeleton SkeletonData { get; protected set; } = null;

        public bool IsPendingWrite { get { return IsWriting; } }
        #endregion
        #region Constructors
        public KobukiService(bool enableKobuki = true, bool enableSkeleton = true)
        {
            WorkerThreads = new List<Thread>();
            EnableKobuki = enableKobuki;
            EnableSkeleton = enableSkeleton;
            if (EnableKobuki)
            {
                SetDefaultPid();
                AddNewKobukiData(GetData());
                Init();
                AddNewLidarData(GetLidarData());
            }
            if (EnableSkeleton)
            {
                AddNewSkeleton(GetSkeleton());
            }
        }
        #endregion
        #region Methods
        private void AddNewKobukiData(KobukiData newData)
        {
            if (newData != null)
            {
                KobukiData lastData = null;
                KobukiSimpleData newSimpleData = null;
                KobukiSimpleData lastSimpleData = null;
                lock (HistoryKobukiData)
                {
                    lastData = HistoryKobukiData.Count > 0 ? HistoryKobukiData[HistoryKobukiData.Count - 1] : newData;
                    if (HistoryKobukiData.Count >= MaxHistoryKobukiDataCount)
                        HistoryKobukiData.RemoveAt(0);
                    if (HistoryKobukiData.Count < MaxHistoryKobukiDataCount)
                    {
                        HistoryKobukiData.Add(newData);
                    }
                    newSimpleData = new KobukiSimpleData()
                    {
                        Battery = newData.Battery,
                        Charger = newData.Charger,
                        EncoderLeft = newData.EncoderLeft,
                        EncoderRight = newData.EncoderRight,
                        GyroAngle = newData.GyroAngle,
                        GyroAngleRate = newData.GyroAngleRate,
                        OverCurrent = newData.OverCurrent,
                        PWMLeft = newData.PWMLeft,
                        PWMRight = newData.PWMRight,
                        Timestamp = newData.Timestamp,
                    };
                }
                if (LastKobukiData == null)
                    LastKobukiData = lastData;
                else
                {
                    lock (LastKobukiData)
                    {
                        LastKobukiData = lastData;
                        Console.WriteLine($"Encoder (l|r): {lastData.EncoderLeft}|{lastData.EncoderRight}");
                        Console.WriteLine($"Gyro (Angle|AngleRate): {lastData.GyroAngle}|{lastData.GyroAngleRate}");
                    }
                }
                lock (HistoryKobukiSimpleData)
                {
                    lastSimpleData = HistoryKobukiSimpleData.Count > 0 ? HistoryKobukiSimpleData[HistoryKobukiSimpleData.Count - 1] : newSimpleData;
                    if (HistoryKobukiSimpleData.Count >= MaxHistoryKobukiDataCount)
                        HistoryKobukiSimpleData.RemoveAt(0);
                    if (HistoryKobukiSimpleData.Count < MaxHistoryKobukiDataCount)
                    {
                        HistoryKobukiSimpleData.Add(newSimpleData);
                    }
                }
                if (LastKobukiSimpleData == null)
                    LastKobukiSimpleData = lastSimpleData;
                else
                {
                    lock (LastKobukiSimpleData)
                    {
                        LastKobukiSimpleData = lastSimpleData;
                    }
                }
            }
        }
        private void AddNewLidarData(LidarData newData)
        {
            if (newData != null)
            {
                if (LidarDataMeasurements == null)
                    LidarDataMeasurements = newData;
                else
                {
                    lock (LidarDataMeasurements)
                    {
                        LidarDataMeasurements = newData;
                    }
                }
            }
        }

        private void AddNewSkeleton(Skeleton newData)
        {
            if (newData != null)
            {
                if (SkeletonData == null)
                    SkeletonData = newData;
                else
                {
                    lock (SkeletonData)
                    {
                        SkeletonData = newData;
                    }
                }
            }
        }
        protected virtual void RobotWorkerCollectHistoryKobukiData()
        {
            int sleepMiliSec = 5;
            while (!ThreadsClosing)
            {
                IsWriting = true;
                KobukiData newData = null;
                lock (Robot)
                {
                    newData = Robot.GetData();
                }
                AddNewKobukiData(newData);
                IsWriting = false;
                Thread.Sleep(sleepMiliSec);
            }
        }
        protected virtual void RobotWorkerCollectLidarDataMeasurements()
        {
            int sleepMiliSec = 100;
            while (!ThreadsClosing)
            {
                IsWriting = true;
                LidarData newData = null;
                lock (Robot)
                {
                    newData = Robot.GetNewLidarData();
                }
                AddNewLidarData(newData);
                IsWriting = false;
                Thread.Sleep(sleepMiliSec);
            }
        }

        protected virtual void RobotWorkerCollectSkeletonData()
        {
            int sleepMiliSec = 30;
            while (!ThreadsClosing)
            {
                IsWriting = true;
                Skeleton newData = null;
                lock (Robot)
                {
                    newData = Robot.GetSkeleton();
                }
                AddNewSkeleton(newData);
                IsWriting = false;
                Thread.Sleep(sleepMiliSec);
            }
        }

        protected void InitThreading()
        {
            if (WorkerThreads != null)
            {                
                if (EnableKobuki)
                {
                    //HistoryKobukiData
                    Thread robotWorkerKobukiData = new Thread(() => RobotWorkerCollectHistoryKobukiData());
                    robotWorkerKobukiData.IsBackground = true;
                    robotWorkerKobukiData.SetApartmentState(ApartmentState.MTA);
                    WorkerThreads.Add(robotWorkerKobukiData);
                    //LidarDataMeasurements
                    Thread robotWorkerLidarData = new Thread(() => RobotWorkerCollectLidarDataMeasurements());
                    robotWorkerLidarData.IsBackground = true;
                    robotWorkerLidarData.SetApartmentState(ApartmentState.MTA);
                    WorkerThreads.Add(robotWorkerLidarData);
                }
                if (EnableSkeleton)
                {
                    //SkeletonData
                    Thread robotWorkerSkeletonData = new Thread(() => RobotWorkerCollectSkeletonData());
                    robotWorkerSkeletonData.IsBackground = true;
                    robotWorkerSkeletonData.SetApartmentState(ApartmentState.MTA);
                    WorkerThreads.Add(robotWorkerSkeletonData);
                }
                //Start threads
                foreach (var worker in WorkerThreads)
                {
                    worker.Start();
                }
            }
        }

        protected bool ThreadingClose()
        {
            var result = false;
            if (WorkerThreads != null && WorkerThreads.Count > 0)
            {
                ThreadsClosing = true;
                for (int i = 0; i < WorkerThreads.Count; i++)
                {
                    var worker = WorkerThreads[i];
                    var respond = worker.Join(ThreadJointTimeout);
                    if (respond)
                    {
                        WorkerThreads.Remove(worker);
                        i -= 1;
                    }
                }
                result = WorkerThreads.Count == 0;
            }
            return result;
        }

        #region IKobukiRobot
        public async Task<bool> SetLedValuesAsync(int led1, int led2)
        {
            return await Robot.SetLedValuesAsync(led1, led2);
        }

        public async Task<bool> SetTranslationSpeedAsync(int mmPerSec)
        {
            return await Robot.SetTranslationSpeedAsync(mmPerSec);
        }

        public async Task<bool> SetRotationSpeedAsync(double radPerSec)
        {
            return await Robot.SetRotationSpeedAsync(radPerSec);
        }

        public async Task<bool> SetArcSpeedAsync(int mmPerSec, int radius)
        {
            return await Robot.SetArcSpeedAsync(mmPerSec, radius);
        }

        public async Task<bool> SetSoundAsync(int noteinHz, int duration)
        {
            return await Robot.SetSoundAsync(noteinHz, duration);
        }

        public async Task<bool> SetDefaultPidAsync()
        {
            return await Robot.SetDefaultPidAsync();
        }

        public async Task<KobukiData> GetDataAsync()
        {
            return await Robot.GetDataAsync();
        }

        public bool SetLedValues(int led1, int led2)
        {
            return Robot.SetLedValues(led1, led2);
        }

        public bool SetTranslationSpeed(int mmPerSec)
        {
            return Robot.SetTranslationSpeed(mmPerSec);
        }

        public bool SetRotationSpeed(double radPerSec)
        {
            return Robot.SetRotationSpeed(radPerSec);
        }

        public bool SetArcSpeed(int mmPerSec, int radius)
        {
            return Robot.SetArcSpeed(mmPerSec, radius);
        }

        public bool SetSound(int noteinHz, int duration)
        {
            return Robot.SetSound(noteinHz, duration);
        }

        public bool SetDefaultPid()
        {
            return Robot.SetDefaultPid();
        }

        public KobukiData GetData()
        {
            return Robot.GetData();
        }

        public Task<bool> InitAsync()
        {
            return Robot.InitAsync();
        }

        public Task<LaserMeasurement> GetNewLaserMeasurementAsync()
        {
            return Robot.GetNewLaserMeasurementAsync();
        }

        public Task<LidarData> GetNewLidarDataAsync()
        {
            return Robot.GetNewLidarDataAsync();
        }

        public Task<LidarData> GetLidarDataAsync()
        {
            return Robot.GetLidarDataAsync();
        }

        public Task<Skeleton> GetSkeletonAsync()
        {
            return Robot.GetSkeletonAsync();
        }

        public bool Init()
        {
            return Robot.Init();
        }

        public LaserMeasurement GetNewLaserMeasurement()
        {
            return Robot.GetNewLaserMeasurement();
        }

        public LidarData GetNewLidarData()
        {
            return Robot.GetNewLidarData();
        }

        public LidarData GetLidarData()
        {
            return Robot.GetLidarData();
        }

        public Skeleton GetSkeleton()
        {
            return Robot.GetSkeleton();
        }
        #endregion

        public void StartWorkers()
        {
            InitThreading();
        }

        public bool CloseWorkers()
        {
            return ThreadingClose();
        }

        public void Dispose()
        {
            ThreadingClose();
            WorkerThreads.Clear();
        }
        #endregion
    }
}
