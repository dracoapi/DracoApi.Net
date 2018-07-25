using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Inventory : ItemService
    {
        private readonly DracoClient client;
        private readonly UserCreatureService userCreatureService;

        public Inventory(DracoClient dracoClient)
        {
            this.client = dracoClient;
            userCreatureService = new UserCreatureService();
        }

        public FCreadex GetCreadex()
        {
            return client.Call(userCreatureService.GetCreadex());
        }

        public FUserCreaturesList GetUserCreatures()
        {
            return client.Call(userCreatureService.GetUserCreatures());
        }

        public new FBagUpdate GetUserItems()
        {
            return client.Call(base.GetUserItems());
        }

        public new object DiscardItems(ItemType id, int count)
        {
            return client.Call(base.DiscardItems(id, count));
        }

        public new FAvaUpdate UseIncense()
        {
            return client.Call(base.UseIncense());
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

            return client.Call(base.UseShovel(request));
        }

        public FAvaUpdate UseSuperVision(double latitude, double longitude)
        {
            var request = new GeoCoords
            {
                latitude = latitude,
                longitude = longitude,
            };

            return client.Call(base.UseSuperVision(request));
         }

        public new FAvaUpdate UseExperienceBooster()
        {
            return client.Call(base.UseExperienceBooster());
        }
    }
}