using DracoLib.Core;
using DracoProtos.Core.Objects;
using System;
using DracoLib.Core.Utils;

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

            var draco = new DracoClient();

            Console.WriteLine("Ping...");
            var ping = draco.Ping();
            if (!ping) throw new Exception();

            Console.WriteLine("Boot...");
            draco.Boot(config);

            Console.WriteLine("Login...");
            draco.Login();

            Console.WriteLine("Init client...");
            draco.Load();

            Console.WriteLine("Get user items...");
            var response = draco.Inventory.GetUserItems() as FBagUpdate;
            foreach (var item in response.items) {
                Console.WriteLine($"  item type { item.type}, count = { item.count}");
            }

            Console.WriteLine("Get map update");
            var mapreponse = draco.GetMapUpdate((float)45.469896, (float)9.180439, (float)20, null) as FUpdate;
            FCreatureUpdate creatures = (FCreatureUpdate)mapreponse.items.Find(o => o.GetType() == typeof(FCreatureUpdate));
            FHatchedEggs hatched = (FHatchedEggs)mapreponse.items.Find(o => o.GetType() == typeof(FHatchedEggs));
            FChestUpdate chests = (FChestUpdate)mapreponse.items.Find(o => o.GetType() == typeof(FChestUpdate));
            FAvaUpdate avatar = (FAvaUpdate)mapreponse.items.Find(o => o.GetType() == typeof(FAvaUpdate));
            FBuildingUpdate buildings = (FBuildingUpdate)mapreponse.items.Find(o => o.GetType() == typeof(FBuildingUpdate));

            Console.WriteLine($"  { creatures.inRadar.Count} creature(s) in radar");
            foreach (var creature in creatures.inRadar)
            {
                var id = creature.id;
                Console.WriteLine($"    creature { creature.name} ({ creature.coords.latitude }, ${ creature.coords.longitude })");
            }
            Console.WriteLine("Done.");
        }
    }
}
