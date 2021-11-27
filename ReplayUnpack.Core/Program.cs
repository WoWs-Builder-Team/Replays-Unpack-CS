using System;
using System.IO;
using System.Linq;
using ReplaysUnpackCS.Data;
using ReplaysUnpackCS.Processing;

namespace ReplaysUnpackCS
{
    internal static class Program
    {
        public static void Main()
        {
            using var fs = File.OpenRead(@"10.10.wowsreplay");
            PlayerDataList? playerDataList = null;
            foreach (var replayData in ReplayReader.ReadReplay(fs))
            {
                switch (replayData)
                {
                    case PlayerDataList dataList:
                        playerDataList = dataList;
                        break;
                    case ChatMessage(var entityId, var messageGroup, var messageContent):
                        var player = playerDataList?.DataList.FirstOrDefault(data => (int)data.Properties[PlayerProperty.AvatarId] == entityId);
                        Console.WriteLine($"{player?.Name} : {messageGroup} : {messageContent}");
                        break;
                }
            }
        }
    }
}
