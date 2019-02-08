using DracoLib.Core.Exceptions;
using DracoLib.Core.Extensions;
using DracoLib.Core.Providers;
using DracoLib.Core.Text;
using DracoLib.Core.Utils;
using DracoProtos.Core.Base;
using DracoProtos.Core.Extensions;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DracoLib.Core
{
    public class User
    {
        internal string Id { get; set; }
        internal int Avatar { get; set; }
        public string DeviceId { get; set; } = DracoUtils.GenerateDeviceId();
        public AuthType LoginType { get; set; } = AuthType.GOOGLE;
        public string Username { get; set; }
        public string Password { get; set; }
        public string Language { get; set; } = Langues.English.ToString();
        public int UtcOffset { get; set; } = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds * 60;
        public int TimeOut { get; set; } = 20 * 1000;
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
        private readonly SerializerContext serializer;
        private RestClient client;
        private readonly Semaphore _rpcQueue = new Semaphore(1, 1);

        internal User User { get; set; }
        internal FClientInfo ClientInfo { get; set; }
        internal string ClientVersion { get; set; }
        internal string ProtocolVersion { get; set; }
        internal FConfig FConfig { get; set; }
        internal int UtcOffset;
        internal string TokenId { get; set; }
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

        public DracoClient(User user, IWebProxy proxy = null)
        {
            this.User = user ?? throw new DracoError("User data no found");

            this.ProtocolVersion = FGameObjects.ProtocolVersion.ToString();
            this.ClientVersion = "12883";

            if (this.User.UtcOffset > 0)
            {
                this.UtcOffset = this.User.UtcOffset;
            }
            else
            {
                this.UtcOffset = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds * 60;
            }

            this.Proxy = proxy;
            int timeout = 20 * 1000;
            if (this.User.TimeOut > 0)
            {
                timeout = this.User.TimeOut;
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
                iOsVendorIdentifier = this.User.DeviceId,
                language = this.User.Language,
                platform = "IPhonePlayer",
                platformVersion = "iOS 12.1.1",
                revision = this.ClientVersion,
                screenHeight = 1334,
                screenWidth = 750
            };

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
            this.Strings = new Strings(this.User.Language, this);
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

        public FConfig Boot()
        {
            return this.Auth.GetConfig(this.User.Language);
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

                if (response != null)
                {
                    var protocolVersion = response.Headers.ToList().Find(x => x.Name == "Protocol-Version" || x.Name == "protocol-version").Value.ToString();
                    if (protocolVersion != this.ProtocolVersion)
                    {
                        ServerCodeError(HttpStatusCode.Conflict);
                    }

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ServerCodeError(response.StatusCode);
                    }

                    var dcportal = response.Headers.ToList().Find(x => x.Name == "dcportal").Value.ToString();
                    if (dcportal != null) this.Dcportal = dcportal;

                    var data = this.serializer.Deserialize(response.RawBytes);
                    (data ?? string.Empty).ToString();
                    return (T)data;
                }

                ServerCodeError(HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                ServerCodeError(HttpStatusCode.Forbidden);
            }
            finally
            {
                _rpcQueue.Release();
            }

            throw new DracoError("Unknown error.");
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
                    string profileId = await this.GoogleLogin();

                    var response = this.Auth.TrySingIn(new AuthData
                    {
                        authType = AuthType.GOOGLE,
                        profileId = profileId,
                        tokenId = this.TokenId
                    },
                    this.ClientInfo,
                    new FRegistrationInfo("gl")
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

        private async Task<string> GoogleLogin()
        {
            return await Task.Run(async () =>
            {
                var login = await new Google().Login(this.User.Username, this.User.Password);
                if (login == null)
                    throw new DracoError(Strings.GetString($"key.ServiceException.Cause.AUTH_ERROR"));

                this.TokenId = login["Auth"];

                var token = JsonConvert.DeserializeObject<JObject>(new CustomJsonWebToken().Decode(this.TokenId, null, false));

                if (token == null)
                    throw new DracoError(Strings.GetString($"key.ServiceException.Cause.USER_NOT_FOUND"));

                bool verified = (bool)token["email_verified"];

                if (!verified)
                    throw new DracoError("You mail is not verified, please verify this before.");

                string sub = (string)token["sub"];

                if (string.IsNullOrEmpty(sub))
                    throw new DracoError("You mail is not verified, please verify this before.");

                return sub;
            });
        }

        private void ServerCodeError(HttpStatusCode response)
        {
            FServiceError fserviceError = null;

            switch (response)
            {
                case HttpStatusCode.BadGateway:
                    fserviceError = new FServiceError("SERVER_MAINTENANCE", new object[0], false);
                    break;

                case HttpStatusCode.Conflict:
                    fserviceError = new FServiceError("PROTOCOL_MISMATCH", new object[0], false);
                    break;

                case HttpStatusCode.Forbidden:
                    fserviceError = new FServiceError("USER_BANNED", new object[0], false);
                    break;

                case HttpStatusCode.InternalServerError:
                    fserviceError = new FServiceError("TRY_AGAIN_LATER", new object[0], false);
                    break;

                case HttpStatusCode.NotFound:
                    fserviceError = new FServiceError("SERVER_DOWN", new object[0], false);
                    break;

                case HttpStatusCode.Gone:
                    fserviceError = new FServiceError("SESSION_GONE", new object[0], false);
                    break;

                case HttpStatusCode.ProxyAuthenticationRequired:
                    throw new DracoError($"Proxy Authentication Required.");

                case HttpStatusCode.ServiceUnavailable:
                    fserviceError = new FServiceError("SERVER_MAINTENANCE", new object[0], false);
                    break;
            }

            if (fserviceError != null)
            {
                if (fserviceError.cause == "SESSION_GONE")
                {
                    throw new DracoError(Strings.GetString($"key.ServiceException.Cause.SESSION_GONE"));
                }
                else if (fserviceError.cause == "SERVER_DOWN")
                {
                    throw new DracoError(Strings.GetString($"key.ServiceException.Cause.SERVER_DOWN"));
                }
                else if (fserviceError.cause == "SERVER_MAINTENANCE")
                {
                    throw new DracoError(Strings.GetString($"key.ServiceException.Cause.SERVER_MAINTENANCE"));
                }
                else if (fserviceError.cause == "USER_BANNED")
                {
                    throw new DracoUserBanned(Strings.GetString($"key.ServiceException.Cause.USER_BANNED.Cheating").Replace("{0}", DateTime.Now.AddYears(20).ToString()));
                }
                else if (fserviceError.cause == "PROTOCOL_MISMATCH")
                {
                    throw new DracoError(Strings.GetString($"key.ServiceException.Cause.PROTOCOL_MISMATCH"));
                }
                else if (fserviceError.cause == "TRY_AGAIN_LATER")
                {
                    throw new DracoError(Strings.GetString($"key.ServiceException.Cause.TRY_AGAIN_LATER"));
                }
                else if (fserviceError.cause == "TOO_HIGH_SPEED_FOR_USE")
                {
                    throw new DracoError(Strings.GetString("key.ServiceException.Cause.TOO_HIGH_SPEED_FOR_USE"));
                }
            }
            else
            {
                throw new DracoError($"Invalid status received: { response }.");
            }
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
