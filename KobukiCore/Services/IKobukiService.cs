using KobukiCore.KobukiAssets.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.Services
{
    public interface IKobukiService : IKobukiRobot
    {
        public void StartWorkers();

        public bool CloseWorkers();
    }
}
