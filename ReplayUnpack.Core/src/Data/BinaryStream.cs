using System;
using System.IO;

namespace ReplaysUnpackCS.Data
{
    public class BinaryStream
    {
        public BinaryStream(Stream memoryStream)
        {
            var bLen = new byte[4];
            memoryStream.Read(bLen);
            var length = BitConverter.ToUInt32(bLen);
            var bValue = new byte[length];
            memoryStream.Read(bValue);
            Value = new MemoryStream(bValue);
        }

        public MemoryStream Value { get; }
    }
}