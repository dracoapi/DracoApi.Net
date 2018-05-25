using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Inventory
    {
        private DracoClient dracoClient;

        public Inventory(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }

        public object GetCreadex()
        {
            return this.dracoClient.Call("UserCreatureService", "getCreadex", new object[] { });
        }

        public object GetUserCreatures()
        {
            return this.dracoClient.Call("UserCreatureService", "getUserCreatures", new object[] { }) as FUserCreaturesList;
        }

        public object GetUserItems()
        {
            return this.dracoClient.Call("ItemService", "getUserItems", new object[] { });
        }

        public object DiscardItem(int id, int count)
        {
            return this.dracoClient.Call("ItemService", "discardItems", new object[] { id, count });
        }

        public object useIncense()//: Promise<objects.FAvaUpdate> 
        {
            return this.dracoClient.Call("ItemService", "useIncense", new object[] { }) as FAvaUpdate;
        }

        public object UseShovel(float latitude, float longitude, float horizontalAccuracy = 20)
        {
            return this.dracoClient.Call("ItemService", "useShovel", new object[]
                {
                    new FClientRequest {
                        time = 0,
                        currentUtcOffsetSeconds = this.dracoClient.utcOffset,
                        coordsWithAccuracy = new GeoCoordsWithAccuracy
                        {
                            latitude = latitude,
                            longitude = longitude,
                            horizontalAccuracy= horizontalAccuracy,
                        },
                    }
            }) as FUpdate;
        }

        public object UseSuperVision(float latitude, float longitude)
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

        public object UseExperienceBooster()
        {
            return this.dracoClient.Call("ItemService", "useExperienceBooster", new object[] { }) as FAvaUpdate;
        }
    }
}