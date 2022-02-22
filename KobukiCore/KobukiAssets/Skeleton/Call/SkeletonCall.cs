using KobukiCore.Calls;
using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.Skeleton.Call
{
    public class SkeletonCall : CallBase, ISkeletonCall
    {
        #region Properties
        public override string Hostname => "127.0.0.1";
        public override int PortRecieve => 23432;
        public override int PortSend => 23432;
        #endregion
        #region Constructors
        public SkeletonCall() : base() { }
        #endregion
        #region Methods

        public Data.Skeleton GetSkeleton()
        {
            //udpClient.Client.Bind(Host);
            var data = ReadStateOnly();
            Data.Skeleton result = Data.Skeleton.TryParseBytesIntoSkeleton(data);
            return result;
        }
        #endregion
    }
}
