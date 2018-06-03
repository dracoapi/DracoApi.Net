using DracoLib.Core;
using DracoLib.Core.Utils;
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
                DeviceId = DracoUtils.GenerateDeviceId(),
                Login = "GOOGLE"
            };

            Config options = new Config()
            {
                CheckProtocol = true,
                EventsCounter = new Dictionary<string, int>(),
                Lang = "English",
                TimeOut = 0,
                UtcOffset = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds
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

            Console.WriteLine("Get creatures...");
            var response = draco.Inventory.GetUserCreatures();
            foreach (var creature in response.userCreatures) {
                var name = creature.alias ?? draco.Strings.Load("creature." + creature.name.ToString());
                Console.WriteLine($"  { name } lvl { creature.level}, cp= {creature.cp}");
             }

            Console.WriteLine("Done.\r\nPress one key to exit...");
            Console.ReadKey();
        }
    }
}
