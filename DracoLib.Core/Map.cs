using DracoLib.Core.Exceptions;
using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;

namespace DracoLib.Core
{
    public class Map : MapService
    {
        private readonly DracoClient client;

        public Map(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }

        public new FBuilding CancelBuildingPersonalization(FBuildingRequest building)
        {
            return client.Call(client.clientMap.CancelBuildingPersonalization(building));
        }

        public FUpdate GetMapUpdate(GeoCoords coords, float horizontalAccuracy = 0, Dictionary<FTile, long> tilescache = null)
        {
            horizontalAccuracy = horizontalAccuracy > 0 ? horizontalAccuracy : client.GetAccuracy();
            tilescache = tilescache ?? new Dictionary<FTile, long>() { };
            long _time_sever = client.TimeServer <= 0 ? 0 : client.TimeServer;

            FUpdate data = client.Call(client.clientMap.GetUpdate(new FUpdateRequest()
            {
                clientRequest = new FClientRequest()
                {
                    time = 0, // _time_sever,
                    currentUtcOffsetSeconds = client.UtcOffset,
                    coordsWithAccuracy = new GeoCoordsWithAccuracy()
                    {
                        latitude = coords.latitude,
                        longitude = coords.longitude,
                        horizontalAccuracy = horizontalAccuracy,
                    },
                },
                configCacheHash = client.FConfig.GetMd5Hash(),
                language = client.ClientInfo.language,
                clientPlatform = ClientPlatform.IOS,
                tilesCache = tilescache,
            }));

            if (data == null)
                throw new DracoError("Null error no data.");

            client.TimeServer = data.serverTime;

            if (data.items != null)
            {
                FConfig config = data.items.Find(i => i?.GetType() == typeof(FConfig)) as FConfig;
                if (config != null) client.FConfig = config;
            }

            return data;
        }        
 
        public new FBuilding InsertLure(FBuildingRequest building)
        {
            return client.Call(client.clientMap.InsertLure(building));
        }

        public FUpdate LeaveDungeon(double latitude, double longitude, float horizontalAccuracy = 0)
        {
            horizontalAccuracy = horizontalAccuracy > 0 ? horizontalAccuracy : client.GetAccuracy();
            long _time_sever = client.TimeServer <= 0 ? 0 : client.TimeServer;

            FUpdate fUpdate = client.Call(client.clientMap.LeaveDungeon(new FClientRequest
            {
                time = 0, //_time_sever,
                currentUtcOffsetSeconds = client.UtcOffset,
                coordsWithAccuracy = new GeoCoordsWithAccuracy
                {
                    latitude = latitude,
                    longitude = longitude,
                    horizontalAccuracy = horizontalAccuracy,
                },
            }));

            client.TimeServer = fUpdate.serverTime;
            return fUpdate;
        }

        public new FOpenChestResult OpenChestResult(FChest dto)
        {
            return client.Call(client.clientMap.OpenChestResult(dto));
        }

        public new FBuilding PersonalizeBuilding(FBuildingRequest building, PersonalizedStop type)
        {
            return client.Call(client.clientMap.PersonalizeBuilding(building, type));
        }

        public new object StartOpeningChest(FChest dto)
        {
            return client.Call(client.clientMap.StartOpeningChest(dto));
        }

        public FUpdate TryUseBuilding(GeoCoords playerLocation, FBuilding building, float horizontalAccuracy = 0)
        {
            horizontalAccuracy = horizontalAccuracy > 0 ? horizontalAccuracy : client.GetAccuracy();
            long _time_sever = client.TimeServer <= 0 ? 0 : client.TimeServer;
            string dungeonId = building.dungeonId;

            switch (building.type)
            {
                case BuildingType.OBELISK:
                    dungeonId = null;
                    break;
            }

            FUpdate fUpdate = client.Call(client.clientMap.TryUseBuilding(new FClientRequest
            {
                time = 0, // _time_sever,
                currentUtcOffsetSeconds = client.UtcOffset,
                coordsWithAccuracy = new GeoCoordsWithAccuracy
                {
                    latitude = playerLocation.latitude,
                    longitude = playerLocation.longitude,
                    horizontalAccuracy = horizontalAccuracy
                },
            },
                new FBuildingRequest(building.id, building.coords, dungeonId)
            ));

            client.TimeServer = fUpdate.serverTime;
            return fUpdate;
        }
    }
}
