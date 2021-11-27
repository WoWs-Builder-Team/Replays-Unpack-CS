using System;
using System.IO;

namespace ReplaysUnpackCS.Data
{
    public class EntityMethod
    {
        public EntityMethod(Stream stream)
        {
            var bEntityId = new byte[4];
            var bMessageId = new byte[4];

            stream.Read(bEntityId);
            stream.Read(bMessageId);

            EntityId = BitConverter.ToUInt32(bEntityId);
            MessageId = BitConverter.ToUInt32(bMessageId);

            Data = new BinaryStream(stream);
        }

        public uint EntityId { get; }

        public uint MessageId { get; }

        public BinaryStream Data { get; }
    }
}