using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Game : GamePlayService
    {
        private readonly DracoClient client;

        public Game(DracoClient dracoClient)
        {
            client = dracoClient;
        }

        public new FCatchingCreature FeedCreature(string creatureId, ItemType item, Tile parentTile)
        {
            return client.Call(client.clientGamePlay.FeedCreature(creatureId, item, parentTile));
        }

        public new FCatchingCreature StartCatchingCreature(FCreatureRequest request)
        {
            return client.Call(client.clientGamePlay.StartCatchingCreature(request));
        }

        public new object StopCatchingCreature(string creatureId)
        {
            return client.Call(client.clientGamePlay.StopCatchingCreature(creatureId));
        }

        public new FCatchCreatureResult TryCatchCreature(string creatureId, ItemType type, float quality, bool spin)
        {
            return client.Call(client.clientGamePlay.TryCatchCreature(creatureId, type, quality, spin));
        }
    }
}
