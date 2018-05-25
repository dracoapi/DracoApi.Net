using DracoLib.Core;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using System;
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
                DeviceId = "xxxxxxx-xxxxxxx-xxxxxxx-xxxxxxx-xxxxxxx",
                Login = "GOOGLE"
            };

            var draco = new DracoClient();

            Console.WriteLine("Ping...");
            draco.Ping();

            Console.WriteLine("Boot...");
            draco.Boot(config);

            Console.WriteLine("Login...");
            draco.Login();

            Console.WriteLine("Init client...");
            draco.Load();

            Console.WriteLine("Get user items...");
            //var response = draco.Inventory.GetUserItems();
            //foreach (var item in response) {
                //Console.WriteLine($"  item type { item.type}, count = ${ item.count}`);
            //}

            Console.WriteLine("Get map update");
            var mapreponse = draco.GetMapUpdate((float)45.469896, (float)9.180439, (float)20, null);
            var creatures = mapreponse.items.Find(o => o.GetType() == typeof(FCreatureUpdate));
            var hatched = mapreponse.items.Find(o => o.GetType() == typeof(FHatchedEggs));
            var chests = mapreponse.items.Find(o => o.GetType() == typeof(FChestUpdate));
            var avatar = mapreponse.items.Find(o => o.GetType() == typeof(FAvaUpdate));
            var buildings = mapreponse.items.Find(o => o.GetType() == typeof(FBuildingUpdate));

            //Console.WriteLine($"  { creatures.inRadar.length} creature(s) in radar");
            //for (const creature of creatures.inRadar) {
            //    const id = creature.coords.id;
            //    const coords = { lat: creature.coords.latitude, lng: creature.coords.longitude };
            //Console.WriteLine(`    creature ${ creature.name}
            //(${ coords.lat.toFixed(5)}, ${ coords.lng.toFixed(5)})`);

            Console.WriteLine("Done.");
        }
    }
}
