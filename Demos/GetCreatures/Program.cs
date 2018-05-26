using DracoLib.Core;
using DracoLib.Core.Utils;
using DracoProtos.Core.Objects;
using System;

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
                DeviceId = DracoUtils.GenerateDeviceId(),
                Login = "GOOGLE"
            };

            var draco = new DracoClient("http://localhost:8888");

            Console.WriteLine("Ping...");
            var ping = draco.Ping();
            if (!ping) throw new Exception();

            Console.WriteLine("Boot...");
            draco.Boot(config);

            Console.WriteLine("Login...");
            draco.Login();

            Console.WriteLine("Init client...");
            draco.Load();

            Console.WriteLine("Get creatures...");
            var response = draco.Inventory.GetUserCreatures() as FUserCreaturesList;
            foreach (var creature in response.userCreatures) {
                var name = creature.alias;
                Console.WriteLine($"  { name } lvl { creature.level}, cp= {creature.cp}");
             }

            Console.WriteLine("Done.");
        }
    }
}
