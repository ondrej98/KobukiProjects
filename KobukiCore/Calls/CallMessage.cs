using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.Calls
{
    public class CallMessage
    {
        #region Properties
        public const byte Header0 = 0xAA;
        public const byte Header1 = 0x55;
        public const byte MaxLength = byte.MaxValue;
        public const byte InitDataSize = 4;
        public const byte IndexHeader0 = 0;
        public const byte IndexHeader1 = 1;
        public const byte IndexLength = 2;
        public const byte IndexData0 = 3;
        public static readonly byte[] InitDataArray = new byte[InitDataSize] { Header0, Header1, 0x00, 0x00 };
        protected byte[] Data = InitDataArray;

        public byte Length { get { return (byte)(Data.Length - InitDataSize); } }
        public byte[] Payload
        {
            get
            {
                var result = new byte[Length];
                for (var i = IndexData0; i < Data.Length - 1; i++)
                {
                    result[i] = Data[i];
                }
                return result;
            }
        }

        public byte CRC { get; private set; }
        #endregion
        #region Methods
        public static byte CountCRC(byte[] byteArray)
        {
            byte result = 0;
            if (byteArray == null)
                throw new ArgumentNullException("byteArray");
            for (int i = IndexLength; i < byteArray.Length - 1; i++)
            {
                result ^= byteArray[i];
            }
            return result;
        }

        public static bool CheckCRC(byte[] byteArray)
        {
            if (byteArray == null)
                throw new ArgumentNullException("byteArray");
            byte result = 0;
            for (int i = 0; i < byteArray[0] + 2; i++)
            {
                result ^= byteArray[i];
            }
            return result == 0;
        }

        public static byte[] GetCallMessageToByteArray(CallMessage callMessage)
        {
            if (callMessage == null)
                throw new ArgumentNullException("callMessage");
            return callMessage.Data;
        }

        public static CallMessage GetCallMessageFromByteArray(CallMessage callMessage, byte[] byteArray)
        {
            CallMessage result = null;
            if (byteArray == null)
                throw new ArgumentNullException("byteArray");
            if (byteArray.Length > MaxLength)
                throw new ArgumentException($"The provided byte array length is greater then the max allowed value {MaxLength}!", "byteArray");
            result = new CallMessage();
            InitData(callMessage, byteArray);
            return result;
        }

        protected static void InitData(CallMessage callMessage, byte[] payload)
        {
            if (callMessage == null)
                throw new ArgumentNullException("callMessage");
            if (payload == null)
                throw new ArgumentNullException("payload");
            if (payload.Length > MaxLength)
                throw new ArgumentException($"The payload length is greater then the max allowed value {MaxLength}!", "payload");
            callMessage.Data = new byte[InitDataSize + payload.Length];
            for (int i = 0; i < callMessage.Data.Length; i++)
            {
                if (i == IndexHeader0)
                    callMessage.Data[i] = Header0;
                else if (i == IndexHeader1)
                    callMessage.Data[i] = Header1;
                else if (i == IndexLength)
                    callMessage.Data[i] = callMessage.Length;
                else if (i >= IndexData0 && i < callMessage.Data.Length - 1)
                    callMessage.Data[i] = payload[i - IndexData0];
                else if (i == callMessage.Data.Length - 1)
                    callMessage.Data[i] = CountCRC(callMessage.Data);
            }
        }

        public void SetPayload(byte[] byteArray)
        {
            if (byteArray == null)
                throw new ArgumentNullException("byteArray");
            if (byteArray.Length > MaxLength)
                throw new ArgumentException($"The byte array length is greater then the max allowed value {MaxLength}!", "byteArray");
            InitData(this, byteArray);
        }

        public string GetMessageAsString()
        {
            var result = string.Empty;
            if (Payload != null)
            {
                result = Convert.ToBase64String(Payload);
            }
            return result;
        }

        public byte[] ToByteArray()
        {
            return GetCallMessageToByteArray(this);
        }

        public byte CountCRC()
        {
            return CountCRC(ToByteArray());
        }
        #endregion
    }
}
