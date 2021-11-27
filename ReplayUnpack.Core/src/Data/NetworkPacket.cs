using System;
using System.IO;

namespace ReplaysUnpackCS.Data
{
    public class NetworkPacket
    {
        public NetworkPacket(Stream stream)
        {
            var payloadSize = new byte[4];
            var payloadType = new byte[4];
            var payloadTime = new byte[4];

            stream.Read(payloadSize);
            stream.Read(payloadType);
            stream.Read(payloadTime);

            Size = BitConverter.ToUInt32(payloadSize);
            Type = BitConverter.ToUInt32(payloadType).ToString("X2");
            Time = BitConverter.ToSingle(payloadTime);

            var data = new byte[Size];
            stream.Read(data);
            RawData = new MemoryStream(data);
        }

        public uint Size { get; }

        public string Type { get; }

        public float Time { get; }

        public MemoryStream RawData { get; }
    }
}