using DracoLib.Core;
using DracoProtos.Core.Config;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using System;
using System.Collections.Generic;

namespace GetCreatures
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

            var draco = new DracoClient("http://localhost:8888");

            Console.WriteLine("Ping...");
            draco.Ping();

            Console.WriteLine("Boot...");
            draco.Boot(config);

            Console.WriteLine("Login...");
            draco.Login();

            Console.WriteLine("Init client...");
            draco.Load();

            Console.WriteLine("Get creatures...");
            var response = draco.Inventory.GetUserCreatures();
            //for (const creature of response.userCreatures) {
            //    const name = creature.alias || strings.getCreature(DracoNode.enums.CreatureType[creature.name]);
            //    Console.WriteLine(`  ${name
            //}
            //lvl ${creature.leve}
            //          , cp=${creature.cp}`);
            // }

            Console.WriteLine("Done.");
        }
    }
}
