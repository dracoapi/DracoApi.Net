using DracoLib.Core;
using DracoProtos.Core.Objects;
using System;
using DracoLib.Core.Utils;
using System.Collections.Generic;

namespace GetMapUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            Console.WriteLine("Creating new Configuration...");
            User config = new User()
            {
                Username = "xxxxxxx@gmail.com",
                Password = "xxxxxxx",
                DeviceId = DracoUtils.GenerateDeviceId(),
                Login = "GOOGLE"
            };

            Config options = new Config()
            {
                CheckProtocol = true,
                EventsCounter = new Dictionary<string, int>(),
                Lang = "English",
                TimeOut = 20 * 1000,
                UtcOffset = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds,
                Delay = 1000
            };

            var draco = new DracoClient(null, options);

            Console.WriteLine("Ping...");
            var ping = draco.Ping();
            if (!ping) throw new Exception();

            Console.WriteLine("Boot...");
            draco.Boot(config);

            Console.WriteLine("Login...");
            var login = draco.Login().Result;
            if (login == null) throw new Exception("Unable to login");

            var newLicence = login.info.newLicense;

            if (login.info.sendClientLog)
            {
                Console.WriteLine("Send client log is set to true! Please report.");
            }

            draco.Post("https://us.draconiusgo.com/client-error", new
            {
                appVersion = draco.ClientVersion,
                deviceInfo = $"platform = iOS\"nos ={ draco.ClientInfo.platformVersion }\"ndevice = iPhone 6S",
                userId = draco.User.Id,
                message = "Material doesn\"t have a texture property \"_MainTex\"",
                stackTrace = "",
            });

            if (newLicence > 0)
            {
                draco.AcceptLicence(newLicence);
            }

            Console.WriteLine("Init client...");
            draco.Load();

            Console.WriteLine("Get user items...");
            var response = draco.Inventory.GetUserItems();
            foreach (var item in response.items) {
                Console.WriteLine($"    Item = { draco.Strings.GetItemName(item.type.ToString()) }, count = { item.count }");
            }
            
            Console.WriteLine("Get map update");
            FUpdate map = draco.GetMapUpdate(45.469896, 9.180439, 20);
            FCreatureUpdate creatures = map.items.Find(o => o.GetType() == typeof(FCreatureUpdate)) as FCreatureUpdate;
            FHatchedEggs hatched = map.items.Find(o => o.GetType() == typeof(FHatchedEggs)) as FHatchedEggs;
            FChestUpdate chests = map.items.Find(o => o.GetType() == typeof(FChestUpdate)) as FChestUpdate;
            FAvaUpdate avatar = map.items.Find(o => o.GetType() == typeof(FAvaUpdate)) as FAvaUpdate;
            FBuildingUpdate buildings = map.items.Find(o => o.GetType() == typeof(FBuildingUpdate)) as FBuildingUpdate;

            if (creatures.inRadar.Count > 0)
            {
                Console.WriteLine($"    { creatures.inRadar.Count } creature(s) in radar");
                foreach (var creature in creatures.inRadar)
                {
                    var id = creature.id;
                    var name = draco.Strings.GetCreatureName(creature.name.ToString());
                    Console.WriteLine($"    Creature: { name } ({ creature.coords.latitude }, { creature.coords.longitude } [id: { id }])");
                }
            }

            if (hatched != null)
            {
                Console.WriteLine($"Hatched(s): { hatched.loot.lootList.Count }");
            }

            if (chests.chests.Count > 0)
            {
                Console.WriteLine($"{ chests.chests.Count} chest(s) in radar");
                foreach (var chest in chests.chests)
                {
                    var id = chest.id;
                    Console.WriteLine($"Chest: { id } ({ chest.coords.latitude }, { chest.coords.longitude })");
                }
            }

            if (avatar != null)
            {
                if (avatar.dungeonId != null)
                {
                    Console.WriteLine($" Avatar: { avatar.altarCoords.longitude }, { avatar.altarCoords.longitude } in radar");
                }
            }

            if (buildings.tileBuildings.Count > 0)
            {
                Console.WriteLine($"{ buildings.tileBuildings.Count} buildings(s) in radar");
                foreach (var building in buildings.tileBuildings)
                {
                    var id = building.Key.dungeonId;
                    Console.WriteLine($"    Building: { id } ({ building.Key.tile.ToUserString() })");
                }
            }

            Console.WriteLine("Done.\r\nPress one key to exit...");
            Console.ReadKey();
        }
    }
}
