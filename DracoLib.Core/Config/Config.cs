using System.Collections.Generic;

namespace DracoLib.Core
{
    public class Config
    {
        public bool CheckProtocol { get; set; } = true;
        public Dictionary<string, int> EventsCounter { get; set; } = new Dictionary<string, int>();
        public int UtcOffset { get; set; } = 7200;
        public int TimeOut { get; set; } = 0;
        public string Lang { get; set; } = "English";
    }
}