using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Inventory
    {
        private readonly DracoClient dracoClient;

        public Inventory(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }

        public FCreadex GetCreadex()
        {
            return this.dracoClient.Call(new UserCreatureService().GetCreadex());
        }

        public FUserCreaturesList GetUserCreatures()
        {
            return this.dracoClient.Call(new UserCreatureService().GetUserCreatures());
        }

        public FBagUpdate GetUserItems()
        {
            return this.dracoClient.Call(new ItemService().GetUserItems());
        }

        public object DiscardItem(ItemType id, int count)
        {
            return this.dracoClient.Call(new ItemService().DiscardItems(id, count));
        }

        public FAvaUpdate UseIncense()
        {
            return this.dracoClient.Call(new ItemService().UseIncense());
        }

        public FUpdate UseShovel(double latitude, double longitude, float horizontalAccuracy = 20)
        {
            var request = new FClientRequest
            {
                time = 0,
                currentUtcOffsetSeconds = this.dracoClient.UtcOffset,
                coordsWithAccuracy = new GeoCoordsWithAccuracy
                {
                    latitude = latitude,
                    longitude = longitude,
                    horizontalAccuracy = horizontalAccuracy,
                },
            };

            return this.dracoClient.Call(new ItemService().UseShovel(request));
        }

        public FAvaUpdate UseSuperVision(double latitude, double longitude)
        {
            var request = new GeoCoords
            {
                latitude = latitude,
                longitude = longitude,
            };

            return this.dracoClient.Call(new ItemService().UseSuperVision(request));
         }

        public FAvaUpdate UseExperienceBooster()
        {
            return this.dracoClient.Call(new ItemService().UseExperienceBooster());
        }
    }
}