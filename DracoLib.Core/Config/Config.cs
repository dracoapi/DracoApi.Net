using System.Collections.Generic;

namespace DracoLib.Core
{
    public class Config
    {
        public bool CheckProtocol { get; set; }
        public Dictionary<string, int> EventsCounter { get; set; } = new Dictionary<string, int>();
        public int UtcOffset { get; set; }
        public int TimeOut { get; set; }
        public string Lang { get; set; }
    }
}