using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.General.Drive
{
    public interface IKobukiDrive
    {
        public List<byte> GetMotherBoardBytes();

        public byte[] GetNewLedBytes(int led1, int led2);

        public byte[] GetNewTranslationSpeedBytes(int mmPerSec);

        public byte[] GetRotationSpeedBytes(double radPerSec);

        public byte[] GetNewArcSpeedBytes(int mmPerSec, int radius);

        public byte[] GetNewSoundBytes(int noteinHz, int duration);

        public byte[] GetDefaultPidBytes();
    }
}
