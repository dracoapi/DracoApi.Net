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

        public FCatchingCreature StartCatchingCreature(string creatureId)
        {
            var request = new FCreatureRequest
            {
                id = creatureId,
                //veryFirst = true
            };

            var response = client.Call(client.clientGamePlay.StartCatchingCreature(request));

            //this.dracoClient.Event("IsArAvailable", "False");

            return response;
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
