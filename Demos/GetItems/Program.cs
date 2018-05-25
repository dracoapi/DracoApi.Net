using DracoLib.Core;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using System;
using System.Collections.Generic;

namespace GetItems
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

            var draco = new DracoClient(/*"http://localhost:8888"*/);

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
            var response = draco.Inventory.GetUserItems();
            //for (const item of response.items) {
            //    Console.WriteLine(`Item type ${ DracoNode.enums.ItemType[item.type]}, count = ${ item.count}`);
            //}

            Console.WriteLine("Done.");
        }
    }
}
