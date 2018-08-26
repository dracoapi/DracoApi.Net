using System;
using System.Collections.Generic;

namespace DracoLib.Core
{
    public class Config
    {
        public bool CheckProtocol { get; set; } = true;
        public Dictionary<string, int> EventsCounter { get; set; } = new Dictionary<string, int>();
        public int UtcOffset { get; set; } = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds;
        public int TimeOut { get; set; } = 20 * 1000;
        public string Lang { get; set; } = "English";
    }
}