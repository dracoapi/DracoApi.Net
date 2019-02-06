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

        public new bool DiscardItems(ItemType type, int count)
        {
            return client.Call(client.clientItem.DiscardItems(type, count));
        }

        public new FBagUpdate GetUserItems()
        {
            return client.Call(client.clientItem.GetUserItems());
        }

        public new FAvaUpdate UseExperienceBooster()
        {
            return client.Call(client.clientItem.UseExperienceBooster());
        }

        public new FAvaUpdate UseIncense()
        {
            return client.Call(client.clientItem.UseIncense());
        }

        public new FAvaUpdate UseRangeExtender()
        {
            return client.Call(client.clientItem.UseRangeExtender());
        }

        public FUpdate UseShovel(double latitude, double longitude, float horizontalAccuracy = 0)
        {
            horizontalAccuracy = horizontalAccuracy > 0 ? horizontalAccuracy : client.GetAccuracy();
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

            return client.Call(client.clientItem.UseShovel(request));
        }

        public new FAvaUpdate UseSuperVision(GeoCoords geoCoords)
        {
            return client.Call(client.clientItem.UseSuperVision(geoCoords));
        }
    }
}
