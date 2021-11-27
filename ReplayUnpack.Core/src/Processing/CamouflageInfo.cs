using Razorvine.Pickle;

namespace ReplaysUnpackCS.Processing
{
    internal class CamouflageInfo : IObjectConstructor
    {
        public object construct(object[] args)
        {
            //Console.WriteLine("{0}, {1}", args);
            return string.Format("{0}, {1}", args);
        }
    }
}
