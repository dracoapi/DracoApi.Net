using DracoLib.Core;
using DracoLib.Core.Text;
using DracoLib.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreateUser
{
    class Program
    {
        private static Random random = new Random();

        private static string GenerateNickname()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz";
            var name = "";
            for (var i = 0; i < 8; i++)
            {
                name += new string(Enumerable.Repeat(chars, 1)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            return name;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            var nickname = GenerateNickname();
            Console.WriteLine("Generate nickname: " + nickname);

            Console.WriteLine("Creating new Configuration...");

            User config = new User()
            {
                Nickname = nickname,
                DeviceId = DracoUtils.GenerateDeviceId(),
                Login = "DEVICE"
            };

            Config options = new Config()
            {
                CheckProtocol = true,
                EventsCounter = new Dictionary<string, int>(),
                Lang = Langues.English.ToString(),
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

            var response = draco.ValidateNickName(config.Nickname);
            if (response == null) throw new Exception("Unable to register nickname. Error: " + response.error);

            Console.WriteLine("Accept tos...");
            draco.AcceptToS();

            Console.WriteLine("Register account...");
            draco.Register(config.Nickname);

            Console.WriteLine("Set avatar...");
            draco.SetAvatar(271891);

            Console.WriteLine("Load...");
            draco.Load();

            Console.WriteLine("Done.\r\nPress one key to exit...");
            Console.ReadKey();
        }
    }
}
