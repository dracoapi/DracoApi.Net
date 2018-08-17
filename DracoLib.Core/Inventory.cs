using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Inventory : ItemService
    {
        private readonly DracoClient client;

        public Inventory(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }

        public FCreadex GetCreadex()
        {
            return client.Call(client.userCreature.GetCreadex());
        }

        public FUserCreaturesList GetUserCreatures()
        {
            return client.Call(client.userCreature.GetUserCreatures());
        }

        public new FBagUpdate GetUserItems()
        {
            return client.Call(client.Item.GetUserItems());
        }

        public new bool DiscardItems(ItemType id, int count)
        {
            return client.Call(client.Item.DiscardItems(id, count));
        }

        public new FAvaUpdate UseIncense()
        {
            return client.Call(client.Item.UseIncense());
        }

        public FUpdate UseShovel(double latitude, double longitude, float horizontalAccuracy = 20)
        {
            var request = new FClientRequest
            {
                time = 0,
                currentUtcOffsetSeconds = client.UtcOffset,
                coordsWithAccuracy = new GeoCoordsWithAccuracy
                {
                    latitude = latitude,
                    longitude = longitude,
                    horizontalAccuracy = horizontalAccuracy,
                },
            };

            return client.Call(client.Item.UseShovel(request));
        }

        public FAvaUpdate UseSuperVision(double latitude, double longitude)
        {
            var request = new GeoCoords
            {
                latitude = latitude,
                longitude = longitude,
            };

            return client.Call(client.Item.UseSuperVision(request));
         }

        public new FAvaUpdate UseExperienceBooster()
        {
            return client.Call(client.Item.UseExperienceBooster());
        }
    }
}