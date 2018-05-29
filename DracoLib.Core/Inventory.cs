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
            return this.dracoClient.Call("UserCreatureService", "getCreadex", new object[] { }) as FCreadex;
        }

        public FUserCreaturesList GetUserCreatures()
        {
            return this.dracoClient.Call("UserCreatureService", "getUserCreatures", new object[] { }) as FUserCreaturesList;
        }

        public FBagUpdate GetUserItems()
        {
            return this.dracoClient.Call("ItemService", "getUserItems", new object[] { }) as FBagUpdate;
        }

        public object DiscardItem(int id, int count)
        {
            return this.dracoClient.Call("ItemService", "discardItems", new object[] { id, count });
        }

        public FAvaUpdate UseIncense()
        {
            return this.dracoClient.Call("ItemService", "useIncense", new object[] { }) as FAvaUpdate;
        }

        public FUpdate UseShovel(double latitude, double longitude, float horizontalAccuracy = 20)
        {
            return this.dracoClient.Call("ItemService", "useShovel", new object[]
                {
                    new FClientRequest {
                        time = 0,
                        currentUtcOffsetSeconds = this.dracoClient.UtcOffset,
                        coordsWithAccuracy = new GeoCoordsWithAccuracy
                        {
                            latitude = latitude,
                            longitude = longitude,
                            horizontalAccuracy= horizontalAccuracy,
                        },
                    }
            }) as FUpdate;
        }

        public FAvaUpdate UseSuperVision(double latitude, double longitude)
        {
            return this.dracoClient.Call("ItemService", "useSuperVision", new object[]
            {
                new GeoCoords
                {
                    latitude = latitude,
                    longitude = longitude,
                },
            }) as FAvaUpdate;
        }

        public FAvaUpdate UseExperienceBooster()
        {
            return this.dracoClient.Call("ItemService", "useExperienceBooster", new object[] { }) as FAvaUpdate;
        }
    }
}