using KobukiCore.Calls;
using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.General.Drive
{
    public class KobukiDrive : IKobukiDrive
    {
        #region Properties
        public CallMessage Message { get; private set; } = new CallMessage();
        #endregion
        #region Methods
        public byte[] GetNewArcSpeedBytes(int mmPerSec, int radius)
        {
            if (radius == 0)
            {
                return GetNewTranslationSpeedBytes(mmPerSec);
            }
            int speedvalue = mmPerSec * ((radius + (radius > 0 ? 230 : -230)) / 2) / radius;
            var motherBoardBytes = GetMotherBoardBytes();
            motherBoardBytes.AddRange(new byte[] { 0x01, 0x04, (byte)(speedvalue % 256), (byte)(speedvalue >> 8), (byte)(radius % 256), (byte)(radius >> 8) });
            Message.SetPayload(motherBoardBytes.ToArray());
            return Message.ToByteArray();
        }

        public List<byte> GetMotherBoardBytes()
        {
            return new List<byte>() { 0x0c, 0x02, 0xf0, 0x00 };
        }

        public byte[] GetDefaultPidBytes()
        {
            var motherBoardBytes = GetMotherBoardBytes();
            motherBoardBytes.AddRange(new byte[] { 0x0D, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, });
            Message.SetPayload(motherBoardBytes.ToArray());
            return Message.ToByteArray();
        }

        public byte[] GetNewLedBytes(int led1, int led2)
        {
            var bytes = new byte[] { 0x0c, 0x02, (byte)((led1 + led2 * 4) % 256), 0x00 };
            Message.SetPayload(bytes);
            return Message.ToByteArray();
        }

        public byte[] GetRotationSpeedBytes(double radPerSec)
        {
            int speedvalue = Convert.ToInt32(radPerSec * 230.0f / 2.0f);
            var motherBoardBytes = GetMotherBoardBytes();
            motherBoardBytes.AddRange(new byte[] { 0x01, 0x04, (byte)(speedvalue % 256), (byte)(speedvalue >> 8), 0x01, 0x00 });
            Message.SetPayload(motherBoardBytes.ToArray());
            return Message.ToByteArray();
        }

        public byte[] GetNewSoundBytes(int noteinHz, int duration)
        {
            int notevalue = (int)Math.Floor((double)1.0 / ((double)noteinHz * 0.00000275) + 0.5);
            var motherBoardBytes = GetMotherBoardBytes();
            motherBoardBytes.AddRange(new byte[] { 0x03, 0x03, (byte)(notevalue % 256), (byte)(notevalue >> 8), (byte)(duration % 256), 0x00 });
            Message.SetPayload(motherBoardBytes.ToArray());
            return Message.ToByteArray();
        }

        public byte[] GetNewTranslationSpeedBytes(int mmPerSec)
        {
            var motherBoardBytes = GetMotherBoardBytes();
            motherBoardBytes.AddRange(new byte[] { 0x01, 0x04, (byte)(mmPerSec % 256), (byte)(mmPerSec >> 8), 0x00, 0x00 });
            Message.SetPayload(motherBoardBytes.ToArray());
            return Message.ToByteArray();
        }
        #endregion
    }
}
