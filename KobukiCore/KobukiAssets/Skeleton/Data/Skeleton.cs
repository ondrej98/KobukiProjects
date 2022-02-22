using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.Skeleton.Data
{
    public class Skeleton
    {
        #region Properties
        public const int ExpectedDataSize = 1800;//size 1800 / 75 => 24 bytes where 1 value (x|y|z) => 8 bytes
        public const int ExpectedJointSize = 24;//24bytes
        public const int ExpectedJointSingleValueSize = 8;//8bytes
        public List<Joint> Joints = new List<Joint>(); //size 75
        #endregion
        #region Methods
        public static Skeleton TryParseBytesIntoSkeleton(byte[] data)
        {
            Skeleton result = null;
            if (data != null)
            {
                try
                {
                    result = ParseBytesIntoSkeleton(data);
                }
                catch (Exception)
                {
                    result = null;
                }
            }
            return result;
        }

        public static Skeleton ParseBytesIntoSkeleton(byte[] data)
        {
            Skeleton result = null;
            if (data != null)
            {
                if (data.Length == ExpectedDataSize)
                {
                    result = new Skeleton();
                    for (int i = 0; i < data.Length; i += ExpectedJointSize)
                    {
                        var joint = new Joint();
                        joint.X = BitConverter.ToDouble(data, i + 0 * ExpectedJointSingleValueSize);
                        joint.Y = BitConverter.ToDouble(data, i + 1 * ExpectedJointSingleValueSize);
                        joint.Z = BitConverter.ToDouble(data, i + 2 * ExpectedJointSingleValueSize);
                        result.Joints.Add(joint);
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
