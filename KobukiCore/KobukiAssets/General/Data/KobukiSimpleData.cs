using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.General.Data
{
    public class KobukiSimpleData
    {
        #region Properties
        public int? Timestamp { get; set; }
        #region WheelTunning
        public int? EncoderRight { get; set; }
        public int? EncoderLeft { get; set; }
        public short? PWMRight { get; set; }
        public short? PWMLeft { get; set; }
        #endregion
        #region Power supply
        public short? Charger { get; set; }
        public short? Battery { get; set; }
        public short? OverCurrent { get; set; }
        #endregion
        #region Inertial Sensor Data
        public int? GyroAngle { get; set; }
        public int? GyroAngleRate { get; set; }
        #endregion
        #endregion
        #region Operators
        public static KobukiSimpleData operator +(KobukiSimpleData a) => a;
        public static KobukiSimpleData operator -(KobukiSimpleData a) => new KobukiSimpleData()
        {
            Battery = (short?)-a.Battery,
            Charger = (short?)-a.Charger,
            EncoderLeft = -a.EncoderLeft,
            EncoderRight = -a.EncoderRight,
            GyroAngle = -a.GyroAngle,
            GyroAngleRate = -a.GyroAngleRate,
            OverCurrent = (short?)-a.OverCurrent,
            PWMLeft = (short?)-a.PWMLeft,
            PWMRight = (short?)-a.PWMRight,
            Timestamp = -a.Timestamp,
        };
        public static KobukiSimpleData operator +(KobukiSimpleData a, KobukiSimpleData b) => a + b;
        public static KobukiSimpleData operator -(KobukiSimpleData a, KobukiSimpleData b) => a + (-b);
        #endregion
    }
}
