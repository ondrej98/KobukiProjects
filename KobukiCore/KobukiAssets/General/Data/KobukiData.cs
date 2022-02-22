using KobukiCore.Calls;
using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.General.Data
{
    public class KobukiData
    {
        #region Properties
        public ushort? Timestamp { get; set; }
        #region Bumpers
        public bool? BumperLeft { get; set; }
        public bool? BumperCenter { get; set; }
        public bool? BumperRight { get; set; }
        #endregion
        #region Cliffs
        public bool? CliffLeft { get; set; }
        public bool? CliffCenter { get; set; }
        public bool? CliffRight { get; set; }
        #endregion
        #region WheelDrops
        public bool? WheelDropLeft { get; set; }
        public bool? WheelDropRight { get; set; }
        #endregion
        #region WheelTunning
        public ushort? EncoderRight { get; set; }
        public ushort? EncoderLeft { get; set; }
        public byte? PWMRight { get; set; }
        public byte? PWMLeft { get; set; }
        #endregion
        #region Buttons
        public KobukiButtonPressEnum? ButtonPress { get; set; }
        #endregion
        #region Power supply
        public byte? Charger { get; set; }
        public byte? Battery { get; set; }
        public byte? OverCurrent { get; set; }
        #endregion
        #region Docking IR
        public byte? IRSensorRight { get; set; }
        public byte? IRSensorCenter { get; set; }
        public byte? IRSensorLeft { get; set; }
        #endregion
        #region Inertial Sensor Data
        public short? GyroAngle { get; set; }
        public ushort? GyroAngleRate { get; set; }
        #endregion
        #region Cliff Sensor Data
        public ushort? CliffSensorRight { get; set; }
        public ushort? CliffSensorCenter { get; set; }
        public ushort? CliffSensorLeft { get; set; }
        #endregion
        #region Current
        public byte? WheelCurrentLeft { get; set; }
        public byte? WheelCurrentRight { get; set; }
        #endregion
        #region Raw Data Of 3D Gyro
        public byte? FrameId { get; set; }
        public List<KobukiRawGyroData> GyroData { get; set; }
        #endregion
        #region General Purpose Input
        public ushort? DigitalInput { get; set; }
        public ushort? AnalogInputCh0 { get; set; }
        public ushort? AnalogInputCh1 { get; set; }
        public ushort? AnalogInputCh2 { get; set; }
        public ushort? AnalogInputCh3 { get; set; }
        #endregion
        #region ExtraRequestData
        public ExtraRequestData ExtraData { get; set; }
        #endregion
        #endregion
        #region Constructor
        public KobukiData() { }
        #endregion
        #region Methods
        public static KobukiData TryParseBytesIntoKobukiData(byte[] bytes)
        {
            KobukiData result = null;
            if (bytes != null)
            {
                try
                {
                    result = ParseBytesIntoKobukiData(bytes);
                }
                catch (Exception)
                {
                    result = null;
                }
            }
            return result;
        }

        public static KobukiData ParseBytesIntoKobukiData(byte[] data)
        {
            KobukiData result = null;
            if (data != null)
            {
                if (CallMessage.CheckCRC(data))
                {
                    result = new KobukiData();
                    int checkedValue = 1;
                    while (checkedValue < data[0])
                    {
                        //Basic data subload
                        if (data[checkedValue] == 0x01)
                        {
                            checkedValue++;
                            if (data[checkedValue] != 0x0F)
                            {
                                result = null;
                                throw new ArgumentException("The basic data sub-load not matched!", "data");
                            }
                            checkedValue++;
                            result.Timestamp = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.BumperCenter = (data[checkedValue] & 0x02) > 0;
                            result.BumperLeft = (data[checkedValue] & 0x04) > 0;
                            result.BumperRight = (data[checkedValue] & 0x01) > 0;
                            checkedValue++;
                            result.WheelDropLeft = (data[checkedValue] & 0x02) > 0;
                            result.WheelDropRight = (data[checkedValue] & 0x01) > 0;
                            checkedValue++;
                            result.CliffCenter = (data[checkedValue] & 0x02) > 0;
                            result.CliffLeft = (data[checkedValue] & 0x04) > 0;
                            result.CliffRight = (data[checkedValue] & 0x01) > 0;
                            checkedValue++;
                            result.EncoderLeft = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.EncoderRight = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.PWMLeft = data[checkedValue];
                            checkedValue++;
                            result.PWMRight = data[checkedValue];
                            checkedValue++;
                            result.ButtonPress = (KobukiButtonPressEnum)data[checkedValue];
                            checkedValue++;
                            result.Charger = data[checkedValue];
                            checkedValue++;
                            result.Battery = data[checkedValue];
                            checkedValue++;
                            result.OverCurrent = data[checkedValue];
                            checkedValue++;
                        }
                        //IR sensors
                        else if (data[checkedValue] == 0x03)
                        {
                            checkedValue++;
                            if (data[checkedValue] != 0x03)
                            {
                                result = null;
                                throw new ArgumentException("The IR sensors data sub-load not matched!", "data");
                            }
                            checkedValue++;
                            result.IRSensorRight = data[checkedValue];
                            checkedValue++;
                            result.IRSensorCenter = data[checkedValue];
                            checkedValue++;
                            result.IRSensorLeft = data[checkedValue];
                            checkedValue++;
                        }
                        //Gyro
                        else if (data[checkedValue] == 0x04)
                        {
                            checkedValue++;
                            if (data[checkedValue] != 0x07)
                            {
                                result = null;
                                throw new ArgumentException("The gyro data sub-load not matched!", "data");
                            }
                            checkedValue++;
                            result.GyroAngle = (short)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.GyroAngleRate = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 5;//3 unsued
                        }
                        //Cliffs
                        else if (data[checkedValue] == 0x05)
                        {
                            checkedValue++;
                            if (data[checkedValue] != 0x06)
                            {
                                result = null;
                                throw new ArgumentException("The cliffs data sub-load not matched!", "data");
                            }
                            checkedValue++;
                            result.CliffSensorRight = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.CliffSensorCenter = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.CliffSensorLeft = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                        }
                        //Current
                        else if (data[checkedValue] == 0x06)
                        {
                            checkedValue++;
                            if (data[checkedValue] != 0x02)
                            {
                                result = null;
                                throw new ArgumentException("The current data sub-load not matched!", "data");
                            }
                            checkedValue++;
                            result.WheelCurrentLeft = data[checkedValue];
                            checkedValue++;
                            result.WheelCurrentRight = data[checkedValue];
                            checkedValue++;

                        }
                        //Extra data hardware version patch
                        else if (data[checkedValue] == 0x0A)
                        {
                            checkedValue++;
                            if (data[checkedValue] != 0x04)
                            {
                                result = null;
                                throw new ArgumentException("The extra data hardware version patch sub-load not matched!", "data");
                            }
                            checkedValue++;
                            if (result.ExtraData == null)
                                result.ExtraData = new ExtraRequestData();
                            result.ExtraData.HardwareVersionPatch = data[checkedValue];
                            checkedValue++;
                            result.ExtraData.HardwareVersionMinor = data[checkedValue];
                            checkedValue++;
                            result.ExtraData.HardwareVersionMajor = data[checkedValue];
                            checkedValue += 2;

                        }
                        //Extra data firmware version patch
                        else if (data[checkedValue] == 0x0B)
                        {
                            checkedValue++;
                            if (data[checkedValue] != 0x04)
                            {
                                result = null;
                                throw new ArgumentException("The extra data firmware version patch sub-load not matched!", "data");
                            }
                            checkedValue++;
                            if (result.ExtraData == null)
                                result.ExtraData = new ExtraRequestData();
                            result.ExtraData.FirmwareVersionPatch = data[checkedValue];
                            checkedValue++;
                            result.ExtraData.FirmwareVersionMinor = data[checkedValue];
                            checkedValue++;
                            result.ExtraData.FirmwareVersionMajor = data[checkedValue];
                            checkedValue += 2;
                        }
                        //Gyro Raw Data
                        else if (data[checkedValue] == 0x0D)
                        {
                            checkedValue++;
                            if (data[checkedValue] % 2 != 0)
                            {
                                result = null;
                                throw new ArgumentException("The gyro raw data sub-load not matched!", "data");
                            }
                            checkedValue++;
                            result.FrameId = data[checkedValue];
                            checkedValue++;
                            int howmanyFrames = data[checkedValue] / 3;
                            checkedValue++;
                            result.GyroData = new List<KobukiRawGyroData>();
                            result.GyroData.Clear();
                            for (int hk = 0; hk < howmanyFrames; hk++)
                            {
                                KobukiRawGyroData temp = new KobukiRawGyroData();
                                temp.X = (byte)(data[checkedValue + 1] * 256 + data[checkedValue]);
                                checkedValue += 2;
                                temp.Y = (byte)(data[checkedValue + 1] * 256 + data[checkedValue]);
                                checkedValue += 2;
                                temp.Z = (byte)(data[checkedValue + 1] * 256 + data[checkedValue]);
                                checkedValue += 2;
                                result.GyroData.Add(temp);
                            }
                        }
                        //General Purpose Input
                        else if (data[checkedValue] == 0x10)
                        {
                            checkedValue++;
                            if (data[checkedValue] != 0x10)
                            {
                                result = null;
                                throw new ArgumentException("The general purpose input data sub-load not matched!", "data");
                            }
                            checkedValue++;
                            result.DigitalInput = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.AnalogInputCh0 = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.AnalogInputCh1 = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.AnalogInputCh2 = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 2;
                            result.AnalogInputCh3 = (ushort)(data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 8;//2+6


                        }
                        //Extra data UDIDs
                        else if (data[checkedValue] == 0x13)
                        {
                            checkedValue++;
                            if (data[checkedValue] != 0x0C)
                            {
                                result = null;
                                throw new ArgumentException("The extra data UDIDs sub-load not matched!", "data");
                            }
                            checkedValue++;
                            if (result.ExtraData == null)
                                result.ExtraData = new ExtraRequestData();
                            result.ExtraData.UDID0 = (uint)(data[checkedValue + 3] * 256 * 256 * 256 + data[checkedValue + 2] * 256 * 256 + data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 4;
                            result.ExtraData.UDID1 = (uint)(data[checkedValue + 3] * 256 * 256 * 256 + data[checkedValue + 2] * 256 * 256 + data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 4;
                            result.ExtraData.UDID2 = (uint)(data[checkedValue + 3] * 256 * 256 * 256 + data[checkedValue + 2] * 256 * 256 + data[checkedValue + 1] * 256 + data[checkedValue]);
                            checkedValue += 4;
                        }
                        //
                        else
                        {
                            checkedValue++;
                            checkedValue += data[checkedValue] + 1;
                        }
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
