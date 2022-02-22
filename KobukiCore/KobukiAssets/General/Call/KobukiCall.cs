using KobukiCore.Calls;
using KobukiCore.KobukiAssets.General.Data;
using KobukiCore.KobukiAssets.General.Drive;
using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.General.Call
{
    public class KobukiCall : CallBase, IKobukiCall
    {
        #region Properties
        public override string Hostname => "127.0.0.1";
        public override int PortSend => 5300;
        public override int PortRecieve => 53000;

        public IKobukiDrive KobukiDrive { get; private set; }
        #endregion
        #region Constructors
        public KobukiCall() : base()
        {
            KobukiDrive = new KobukiDrive();
        }

        public KobukiCall(IKobukiDrive kobukiDrive) : base()
        {
            if (kobukiDrive == null)
                throw new ArgumentNullException("kobukiDrive");
            KobukiDrive = kobukiDrive;
        }
        #endregion
        #region Methods
        public bool SetLedValues(int led1, int led2)
        {
            return WriteCommandOnly(KobukiDrive.GetNewLedBytes(led1, led2));
        }

        public bool SetTranslationSpeed(int mmPerSec)
        {
            return WriteCommandOnly(KobukiDrive.GetNewTranslationSpeedBytes(mmPerSec));
        }

        public bool SetRotationSpeed(double radPerSec)
        {
            return WriteCommandOnly(KobukiDrive.GetRotationSpeedBytes(radPerSec));
        }

        public bool SetArcSpeed(int mmPerSec, int radius)
        {
            return WriteCommandOnly(KobukiDrive.GetNewArcSpeedBytes(mmPerSec, radius));
        }

        public bool SetSound(int noteinHz, int duration)
        {
            return WriteCommandOnly(KobukiDrive.GetNewSoundBytes(noteinHz, duration));
        }

        public bool SetDefaultPid()
        {
            return WriteCommandOnly(KobukiDrive.GetDefaultPidBytes());
        }

        public KobukiData GetData()
        {
            KobukiData result = null;
            var data = ReadStateOnly();
            result = KobukiData.TryParseBytesIntoKobukiData(data);
            return result;
        }
        #endregion
    }
}
