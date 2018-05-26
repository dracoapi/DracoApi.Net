using DracoLib.Core;
using DracoLib.Core.Utils;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
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

            Console.WriteLine("Creating new Configuration...");
            User config = new User()
            {
                Username = "xxxxxxx@gmail.com",
                Password = "xxxxxxx",
                DeviceId = DracoUtils.GenerateDeviceId(),
                Login = "GOOGLE"
            };

            var draco = new DracoClient();

            Console.WriteLine("Boot...");
            draco.Boot(config);

            Console.WriteLine("Init login...");
            draco.Login();

            Console.WriteLine("Generate nickname...");
            var nickname = GenerateNickname();
            var response = draco.ValidateNickName(nickname) as FNicknameValidationResult;
            while (response != null && response.error == FNicknameValidationError.DUPLICATE)
            {
                nickname = response.suggestedNickname;
                response = draco.ValidateNickName(nickname) as FNicknameValidationResult;
            }
            if (response == null) throw new Exception("Unable to register nickname. Error: " + response.error);
            Console.WriteLine("  nickname: " + nickname);

            Console.WriteLine("Accept tos...");
            draco.AcceptToS();

            Console.WriteLine("Register account...");
            draco.Register(nickname);

            Console.WriteLine("Set avatar...");
            draco.SetAvatar(271891);

            Console.WriteLine("Load...");
            draco.Load();

            Console.WriteLine("Done.");
        }
    }
}
