using DracoLib.Core;
using DracoLib.Core.Text;
using DracoLib.Core.Utils;
using DracoProtos.Core.Base;
using System;
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

            User user = new User()
            {
                Username = nickname,
                DeviceId = DracoUtils.GenerateDeviceId(),
                LoginType = AuthType.DEVICE,
                Language = Langues.English.ToString(),
                UtcOffset = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds * 60,
                TimeOut = 20 * 1000,
            };

            var draco = new DracoClient(user, null);

            Console.WriteLine("Ping...");
            var ping = draco.Ping();
            if (!ping) throw new Exception();

            Console.WriteLine("Boot...");
            draco.Boot();

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

            var response = draco.Auth.ValidateNickname(user.Username);
            if (response == null) throw new Exception("Unable to register nickname. Error: " + response.error);

            Console.WriteLine("Register account...");
            //draco.Login();
                
            Console.WriteLine("Set avatar...");
            draco.Player.SaveUserSettings(271891);

            Console.WriteLine("Load...");
            draco.Load();

            Console.WriteLine("Done.\r\nPress one key to exit...");
            Console.ReadKey();
        }
    }
}
