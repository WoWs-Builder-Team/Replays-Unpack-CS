using System.IO;
using ReplaysUnpackCS.Processing;

namespace ReplaysUnpackCS
{
    internal class Program
    {
        public static void Main()
        {
            using var fs = File.OpenRead(@"10.10.wowsreplay");
            ReplayReader.ReadReplay(fs);
        }
    }
}
