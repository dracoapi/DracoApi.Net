using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;

namespace DracoLib.Core
{
    public class Event : ClientEventService
    {
        private readonly DracoClient client;

        public Event(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }

        public new object ClientLogRecords(List<FClientLogRecord> logRecords)
        {
            return client.Call(client.clientEvent.ClientLogRecords(logRecords));
        }

        public new object OnEventWithCounter(string name, string userId, FClientInfo clientInfo, int counter, string param1, string param2, string param3, string param4, string param5)
        {
            return client.Call(client.clientEvent.OnEventWithCounter(name, userId, clientInfo, counter, param1, param2, param3, param4, param5));
        }

        public new object OptionChanged(string option, string newValue, string defValue)
        {
            return client.Call(client.clientEvent.OptionChanged(option, newValue, defValue));
        }
    }
}
