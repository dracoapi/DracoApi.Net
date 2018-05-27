using DracoLib.Core;
using DracoProtos.Core.Objects;
using System;
using DracoLib.Core.Utils;
using DracoLib.Core.Text;
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
                TimeOut = 0,
                UtcOffset = 7200
            };

            var draco = new DracoClient(null, options);

            Console.WriteLine("Ping...");
            var ping = draco.Ping();
            if (!ping) throw new Exception();

            Console.WriteLine("Boot...");
            draco.Boot(config);

            Console.WriteLine("Login...");
            var login = draco.Login() as FAuthData;
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
            var response = draco.Inventory.GetUserItems() as FBagUpdate;
            foreach (var item in response.items) {
                Console.WriteLine($"  item type { item.type}, count = { item.count}");
            }

            Console.WriteLine("Get map update");
            var map = draco.GetMapUpdate(45.469896, 9.180439, 20) as FUpdate;
            FCreatureUpdate creatures = (FCreatureUpdate)map.items.Find(o => o.GetType() == typeof(FCreatureUpdate));
            FHatchedEggs hatched = (FHatchedEggs)map.items.Find(o => o.GetType() == typeof(FHatchedEggs));
            FChestUpdate chests = (FChestUpdate)map.items.Find(o => o.GetType() == typeof(FChestUpdate));
            FAvaUpdate avatar = (FAvaUpdate)map.items.Find(o => o.GetType() == typeof(FAvaUpdate));
            FBuildingUpdate buildings = (FBuildingUpdate)map.items.Find(o => o.GetType() == typeof(FBuildingUpdate));

            Console.WriteLine($"  { creatures.inRadar.Count} creature(s) in radar");
            foreach (var creature in creatures.inRadar)
            {
                var id = creature.id;
                var name = English.Load[creature.name.ToString()];
                Console.WriteLine($"    creature { name } ({ creature.coords.latitude }, ${ creature.coords.longitude })");
            }
            Console.WriteLine("Done.");
        }
    }
}
