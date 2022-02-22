using KobukiCore.KobukiAssets.General.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.General.Call
{
    public interface IKobukiCall
    {
        public bool SetLedValues(int led1, int led2);

        public bool SetTranslationSpeed(int mmPerSec);

        public bool SetRotationSpeed(double radPerSec);

        public bool SetArcSpeed(int mmPerSec, int radius);

        public bool SetSound(int noteinHz, int duration);

        public bool SetDefaultPid();

        public KobukiData GetData();
    }
}
