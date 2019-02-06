using DracoLib.Core.Exceptions;
using DracoLib.Core.Extensions;
using DracoLib.Core.Providers;
using DracoLib.Core.Text;
using DracoProtos.Core.Base;
using DracoProtos.Core.Extensions;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DracoLib.Core
{
    public class User
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public string Nickname { get; set; }
        public int Avatar { get; set; }
        public AuthType LoginType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    internal class ApiAuth
    {
        public AuthType Type { get; set; }
        public string Reg { get; set; }
        public string ProfileId { get; set; }
        public string TokenId { get; set; }
    }

    public class DracoClient
    {
        public Encounter Encounter { get; private set; }
        public Inventory Inventory { get; private set; }
        public Game Game { get; private set; }
        public Map Map { get; private set; }
        public Auth Auth { get; private set; }
        public Event Event { get; private set; }
        public DevMode DevMode { get; private set; }
        public Player Player { get; private set; }
        public Contest Contest { get; private set; }
        public Magic Magic { get; private set; }
        public Creatures Creatures { get; private set; }
        public Battle Battle { get; private set; }
        public Strings Strings { get; private set; }

        private RestRequest Request { get; set; }
        private IWebProxy Proxy { get; set; }
        private string Dcportal { get; set; }
        private bool CheckProtocol { get; set; }
        private ApiAuth ApiAuth { get; set; }
        private Dictionary<string, int> EventsCounter { get; set; } = new Dictionary<string, int>();
        private readonly SerializerContext serializer;
        private RestClient client;
        private readonly Semaphore _rpcQueue = new Semaphore(1, 1);

        internal User User { get; set; }
        internal FClientInfo ClientInfo { get; set; }
        internal string ClientVersion { get; set; }
        internal string ProtocolVersion { get; set; }
        internal FConfig FConfig { get; set; }
        internal sbyte[] ConfigHash { get; set; }
        internal int UtcOffset;
        internal Config Config { get; set; }
        internal readonly GamePlayService clientGamePlay = new GamePlayService();
        internal readonly ContestMapService clientContestMap = new ContestMapService();
        internal readonly DevModeService clientDevMode = new DevModeService();
        internal readonly ClientEventService clientEvent = new ClientEventService();
        internal readonly BattleService clientBattle = new BattleService();
        internal readonly PlayerService clientPlayer = new PlayerService();
        internal readonly MagicService clientMagic = new MagicService();
        internal readonly ItemService clientItem = new ItemService();
        internal readonly EncounterService clientEncounter = new EncounterService();
        internal readonly MapService clientMap = new MapService();
        internal readonly UserCreatureService clientUserCreature = new UserCreatureService();
        internal readonly AuthService clientAuth = new AuthService();

        //Others
        internal long TimeServer { get; set; }
        //internal readonly List<RequestListener> _listeners = new List<RequestListener>();

        internal float GetAccuracy()
        {
            double random = new Random().Next(20, 65) * 2;
            return (float)Math.Floor(random);
        }

        internal sbyte[] BuildConfigHash(FConfig config)
        {
            FConfig = config;
            this.ConfigHash = FConfig.GetMd5HashAsSbyte(config);
            return this.ConfigHash;
        }

        internal void FixTexture()
        {
            var _client = new RestClient("https://us.draconiusgo.com/client-error");
            var _request = new RestRequest(Method.POST);
            _request.AddHeader("dcportal", this.Dcportal);
            _request.AddObject(new
            {
                appVersion = this.ClientVersion,
                deviceInfo = $"platform = iOS\"nos ={ this.ClientInfo.platformVersion }\"ndevice = iPhone 7",
                userId = this.User.Id,
                message = "Material doesn\"t have a texture property \"_MainTex\"",
                stackTrace = "",
            });

            var response = _client.Execute(_request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Invalid status received: " + response.StatusDescription);
            }
        }

        public DracoClient(IWebProxy proxy = null, Config config = null)
        {
            this.Config = config ?? new Config();

            this.ProtocolVersion = FGameObjects.ProtocolVersion.ToString();
            this.ClientVersion = "12883";
            if (this.Config.CheckProtocol) this.CheckProtocol = this.Config.CheckProtocol;
            if (this.Config.EventsCounter.Any()) this.EventsCounter = this.Config.EventsCounter;
            if (this.Config.UtcOffset > 0)
            {
                this.UtcOffset = this.Config.UtcOffset;
            }
            else
            {
                this.UtcOffset = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds * 60;
            }

            this.Proxy = proxy;
            int timeout = 20 * 1000;
            if (this.Config.TimeOut > 0)
            {
                timeout = this.Config.TimeOut;
            }

            this.serializer = new SerializerContext("portal", FGameObjects.CLASSES, FGameObjects.ProtocolVersion);

 
            this.client = new RestClient("https://us.draconiusgo.com");

            if (this.Proxy != null)
                this.client.Proxy = this.Proxy;

            this.client.ClearHandlers();
            this.client.AddDefaultHeader("X-Unity-Version", "2017.1.3f1");
            this.client.AddDefaultHeader("Accept", "*/*");
            this.client.AddDefaultHeader("Protocol-Version", this.ProtocolVersion);
            this.client.AddDefaultHeader("Client-Version", this.ClientVersion);
            this.client.AddDefaultHeader("Accept-Language", "en-us");
            this.client.UserAgent = $"DraconiusGO/{this.ClientVersion} CFNetwork/975.0.3 Darwin/18.2.0";
            this.client.CookieContainer = new CookieContainer();
            //this.client.Encoding = null;
            this.client.Timeout = timeout;

            this.ClientInfo = new FClientInfo
            {
                deviceModel = "iPhone9,3",
                iOsAdvertisingTrackingEnabled = false,
                language = this.Config.Lang,
                platform = "IPhonePlayer",
                platformVersion = "iOS 12.1.1",
                revision = this.ClientVersion,
                screenHeight = 1334,
                screenWidth = 750,
            };

            this.User = new User();
            this.Encounter = new Encounter(this);
            this.Inventory = new Inventory(this);
            this.Game = new Game(this);
            this.Map = new Map(this);
            this.Auth = new Auth(this);
            this.Contest = new Contest(this);
            this.Event = new Event(this);
            this.DevMode = new DevMode(this);
            this.Magic = new Magic(this);
            this.Player = new Player(this);
            this.Creatures = new Creatures(this);
            this.Battle = new Battle(this);
            this.Strings = new Strings(config.Lang, this);
        }

        public bool Ping()
        {
            Request = new RestRequest("ping", Method.POST);
            Request.AddHeader("Content-Type", "application /x-www-form-urlencoded");
            var response = client.Execute(Request);

            this.client.AddDefaultParameter("Path", "/", ParameterType.Cookie);
            this.client.AddDefaultParameter("path", "/", ParameterType.Cookie);
            this.client.AddDefaultParameter("domain", ".draconiusgo.com", ParameterType.Cookie);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public FConfig Boot(User userinfo)
        {
            this.User.Id = userinfo.Id;
            this.User.DeviceId = userinfo.DeviceId;
            this.User.LoginType = userinfo.LoginType;
            this.User.Username = userinfo.Username;
            this.User.Password = userinfo.Password;
            this.ClientInfo.iOsVendorIdentifier = userinfo.DeviceId;
            return this.Auth.GetConfig(this.ClientInfo.language);
        }

        internal T Call<T>(Async<T> request)
        {
            try
            {
                _rpcQueue.WaitOne();

                var service = request.GetType().GetField("service", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(request);
                var method = request.GetType().GetField("methodName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(request);
                var body = request.GetType().GetField("args", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(request);

                var rawbody = this.serializer.Serialize(body);

                Request = new RestRequest("serviceCall", Method.POST);
                Request.AddHeader("Protocol-Version", this.ProtocolVersion);
                Request.AddHeader("Client-Version", this.ClientVersion);
                if (this.Dcportal != null)
                {
                    Request.AddHeader("dcportal", this.Dcportal);
                }
                Request.AddParameter("service", service);
                Request.AddParameter("method", method);
                Request.AddFile("args", rawbody, "args.dat", "application/octet-stream");

                var response = this.client.Execute(Request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new DracoError("Invalid status received: " + response.StatusDescription);
                }

                var protocolVersion = response.Headers.ToList().Find(x => x.Name == "Protocol-Version" || x.Name == "protocol-version").Value.ToString();
                if (protocolVersion != this.ProtocolVersion && this.CheckProtocol)
                {
                    throw new DracoError("Incorrect protocol version received: " + protocolVersion);
                }

                var dcportal = response.Headers.ToList().Find(x => x.Name == "dcportal").Value.ToString();
                if (dcportal != null) this.Dcportal = dcportal;

                var data = this.serializer.Deserialize(response.RawBytes);
                (data ?? string.Empty).ToString();
                return (T)data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _rpcQueue.Release();
            }
        }

        public async Task<FAuthData> Login()
        {
            switch (this.User.LoginType)
            {
                case AuthType.DEV:
                    break;

                case AuthType.DEVICE:
                    //Identifiers.deviceId = this.User.DeviceId;
                    break;

                case AuthType.FACEBOOK:
                    break;

                case AuthType.GOOGLE:
                    this.ApiAuth = new ApiAuth
                    {
                        Type = AuthType.GOOGLE,
                        Reg = "gl",
                        ProfileId = "?",
                    };

                    await this.GoogleLogin();

                    var response = this.Auth.TrySingIn(new AuthData
                    {
                        authType = this.ApiAuth.Type,
                        profileId = this.ApiAuth.ProfileId,
                        tokenId = this.ApiAuth.TokenId
                    },
                    this.ClientInfo,
                    new FRegistrationInfo(this.ApiAuth.Reg)
                    {
                        email = this.User.Username
                    });

                    if (response != null && response.info != null)
                    {
                        this.User.Id = response.info.userId;
                        this.User.Avatar = response.info.avatarAppearanceDetails;
                        // fix for textures
                        FixTexture();
                    }

                    return response;
            }

            throw new DracoError("Unsupported login type: " + this.User.LoginType.ToString());
        }

        private async Task GoogleLogin()
        {
            await Task.Run(async () =>
            {
                //this.Event("StartGoogleSignIn");
                var login = await new Google().Login(this.User.Username, this.User.Password);
                if (login == null)
                    throw new DracoError("Unable to login");

                this.ApiAuth.TokenId = login["Auth"];

                var token = JsonConvert.DeserializeObject<JObject>(new CustomJsonWebToken().Decode(this.ApiAuth.TokenId, null, false));

                if (token == null)
                    throw new DracoError("Unable to get the token.");

                bool verified = (bool)token["email_verified"];

                if (!verified)
                    throw new DracoError("You mail is not verified, please verify this before.");

                string sub = (string)token["sub"];

                if (string.IsNullOrEmpty(sub))
                    throw new DracoError("You mail is not verified, please verify this before.");

                this.ApiAuth.ProfileId = sub;
            });
        }

        public void Load()
        {
            if (this.User.Avatar == 0) throw new DracoError("Please login first.");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        internal void Dispose(bool disposing)
        {
            if (!disposing) return;
            _rpcQueue?.Dispose();
        }
    }
}
