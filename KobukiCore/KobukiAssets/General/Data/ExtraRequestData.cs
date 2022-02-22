using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.General.Data
{
    public class ExtraRequestData
    {
        #region Properties
        #region Hardware Version
        public byte HardwareVersionMajor { get; set; }
        public byte HardwareVersionMinor { get; set; }
        public byte HardwareVersionPatch { get; set; }
		#endregion
		#region Firmware Version
		public byte FirmwareVersionMajor { get; set; }
		public byte FirmwareVersionMinor { get; set; }
		public byte FirmwareVersionPatch { get; set; }
        #endregion
        #region Unique Device IDentifier(UDID)
        public uint UDID0 { get; set; }
        public uint UDID1 { get; set; }
        public uint UDID2 { get; set; }
		#endregion
		#region Controller Info
		public byte PIDtype { get; set; }
		public uint PIDgainP { get; set; }
		public uint PIDgainI { get; set; }
		public uint PIDgainD { get; set; }
		#endregion
		#endregion
	}
}
