
using System.Collections.Generic;

namespace DracoProtos.Core.Config
{
    public class Config
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string DeviceId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Lang { get; set; }
        public string Proxy { get; set; }
        public bool CheckProtocol { get; set; }
        public int UtcOffset { get; set; }
        public int TimeOut { get; set; }
        public int Delay { get; set; }
        public Dictionary<string, int> EventsCounter { get; set; } = new Dictionary<string, int>();
    }
}
