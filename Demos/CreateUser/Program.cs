using DracoLib.Core;
using DracoLib.Core.Utils;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using System;
using System.Collections.Generic;

namespace CreateUser
{
    class Program
    {
        private static string GenerateNickname()
        {
            //var chars = "abcdefghijklmnopqrstuvwxyz";
            var name = "";
            for (var i = 0; i < 8; i++)
            {
                //name += chars[Math.Floor(Math.random() * chars.length)];
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
            var response = draco.ValidateNickName(nickname);
            //while (response != null && response == FNicknameValidationError.DUPLICATE)
            //{
            //    nickname = response.suggestedNickname;
            //    response = await this.validateNickname(nickname);
            //}
            //if (response) throw new Error("Unable to register nickname. Error: " + response.error);
            Console.WriteLine("  nickname: " + nickname);

            Console.WriteLine("Accept tos...");
            draco.AcceptToS();

            Console.WriteLine("Register account...");
            //draco.ProtocolVersion(nickname);

            Console.WriteLine("Set avatar...");
            response = (string)draco.SetAvatar(271891);

            //Console.WriteLine("Save data into users.json...");
            //var users = [];
            //if (fs.existsSync("users.json"))
            //{
            //    users = JSON.parse(fs.readFileSync("users.json", "utf8"));
            //}
            //users.push(draco.user);
            //fs.writeFileSync("users.json", JSON.stringify(users, null, 2), "utf8");

            // Console.WriteLine("Load...");
            // await draco.load();

            Console.WriteLine("Done.");
        }
    }
}
