using DracoLib.Core;
using DracoLib.Core.Utils;
using DracoProtos.Core.Base;
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
                DeviceId = DracoUtils.GenerateDeviceId(),
                LoginType = AuthType.GOOGLE
            };

            Config options = new Config()
            {
                CheckProtocol = true,
                EventsCounter = new Dictionary<string, int>(),
                Lang = "English",
                TimeOut = 20 * 1000,
                UtcOffset = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds * 60
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

            if (newLicence > 0)
            {
                draco.Auth.AcceptLicence(newLicence);
            }

            Console.WriteLine("Init client...");
            draco.Load();

            Console.WriteLine("Get user items...");
            var response = draco.Inventory.GetUserItems();
            foreach (var item in response.items) {
                Console.WriteLine($"    Item = { draco.Strings.GetItemName(item.type) }, count = { item.count}");
            }

            Console.WriteLine("Done.\r\nPress one key to exit...");
            Console.ReadKey();
        }
    }
}
