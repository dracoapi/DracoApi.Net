using DracoProtos.Core.Classes;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DracoLib.Core
{
    public class DracoClient
    {
        public FClientInfo ClientInfo { get; set; }
        public UserInfo User { get; set; }
        private SerializerContext serializer;
        private RestClient client;
        //private int[] eventsCounter;

        public DracoClient() : this("iOS 11.2.6", "iPhone8,1", DracoUtils.GenerateDeviceId())
        {
        }

        public DracoClient(string platformVersion, string deviceModel, string deviceid)
        {
            this.User = new UserInfo
            {
                DeviceId = deviceid,
                DeviceAdId = DracoUtils.GenerateDeviceId(),
                
            };

            ClientInfo = new FClientInfo()
            {
                platform = "IPhonePlayer",
                platformVersion = platformVersion,
                revision = FGameObjects.ClientVersion.ToString(),
                deviceModel = deviceModel,
                screenWidth = 750,
                screenHeight = 1334,
                language = "English",
                iOsAdvertisingIdentifier = this.User.DeviceAdId,
                iOsAdvertisingTrackingEnabled = true,
                iOsVendorIdentifier = this.User.DeviceId,
                googleAdvertisingId = null,
                googleTrackingEnabled = false
            };

            this.serializer = new SerializerContext("portal", FGameObjects.CLASSES, FGameObjects.ProtocolVersion);

            this.client = new RestClient("https://us.draconiusgo.com");
            this.client.ClearHandlers();
            this.client.AddDefaultHeader("Protocol-Version", FGameObjects.ProtocolVersion.ToString());
            this.client.AddDefaultHeader("Client-Version", FGameObjects.ClientVersion.ToString());
            this.client.UserAgent = $"DraconiusGO/{FGameObjects.ClientVersion} CFNetwork/897.15 Darwin/17.5.0";
            this.client.AddDefaultHeader("Accept", "*/*");
            this.client.AddDefaultHeader("Accept-Language", "en-us");
            this.client.AddDefaultHeader("X-Unity-Version", "2017.1.3f1");
            this.client.CookieContainer = new CookieContainer();
        }

        public void SetProxy(string proxy)
        {
            if (proxy != null)
            {
                this.client.Proxy = new WebProxy(proxy);
            }
            else
            {
                this.client.Proxy = null;
            }
        }

        public bool Ping()
        {
            var request = new RestRequest("ping", Method.POST);
            request.AddHeader("Content-Type", "application /x-www-form-urlencoded");
            var response = client.Execute(request);

            this.client.AddDefaultParameter("Path", "/", ParameterType.Cookie);
            this.client.AddDefaultParameter("path", "/", ParameterType.Cookie);
            this.client.AddDefaultParameter("domain", ".draconiusgo.com", ParameterType.Cookie);

            return response.StatusCode == HttpStatusCode.OK;
        }
        
        public object ServiceCall(string service, string method, object body)
        {
            var rawbody = serializer.Serialize(body);

            var request = new RestRequest("serviceCall", Method.POST);
            request.AddHeader("Protocol-Version", serializer.protocolVersion.ToString());
            if (this.User.PortalId != null)
            {
                request.AddHeader("dcportal", this.User.PortalId);
            }
            request.AddParameter("service", service);
            request.AddParameter("method", method);
            request.AddFile("args", rawbody, "args.dat", "application/octet-stream");

            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Invalid status received: " + response.StatusDescription);
            }

            var protocolVersion = response.Headers.ToList().Find(x => x.Name == "Protocol-Version" || x.Name == "protocol-version").Value.ToString();
            var dcportal = response.Headers.ToList().Find(x => x.Name == "dcportal").Value.ToString();
            if (dcportal != null) this.User.PortalId = dcportal;
            if (protocolVersion != serializer.protocolVersion.ToString())
            {
                throw new Exception("Incorrect protocol version received: " + protocolVersion);
            }
            
            var data = serializer.Deserialize(response.RawBytes);
            (data ?? "").ToString();

            return data;
        }

        public void Event(string name, string one = null, string two = null, string three = null)
        {
            //const int eventCounter = this.eventsCounter[name] ?? 1;
            this.ServiceCall("ClientEventService", "onEvent", new object[]
            {
                name,
                this.User.UserId,
                this.ClientInfo,
                //eventCounter,
                one,
                two,
                three,
                null,
                null
            });
            //this.eventsCounter[name] = eventCounter + 1;
        }

        public void Boot()
        {
            this.Event("LoadingScreenPercent", "100");
            this.Event("Initialized");
        }

        public FAuthData TrySignIn()
        {
            this.Event("TrySingIn", "DEVICE");
            var data = this.ServiceCall("AuthService", "trySingIn", new object[]
            {
                new AuthData() { authType = AuthType.DEVICE, profileId = this.User.DeviceId },
                this.ClientInfo,
                new FRegistrationInfo() { age = "", email = "", gender = "", regType = "dv", socialId = "" },
            }) as FAuthData;

            if (data != null)
            {
                this.User.UserId = data.info.userId;
                this.User.Avatar = data.info.avatarAppearanceDetails;
            }

            return data;
        }

        public string ValidateNickName(string nickname, bool takeSuggested = true)
        {
            this.Event("ValidateNickname", nickname);
            var result = this.ServiceCall("AuthService", "validateNickname", new object[] { nickname }) as FNicknameValidationResult;
            if (result == null) return nickname;
            else if (result.error == FNicknameValidationError.DUPLICATE)
            {
                this.Event("ValidateNicknameError", "DUPLICATE");
                if (takeSuggested) return ValidateNickName(result.suggestedNickname, true);
                else return null;
            }
            else
            {
                return null;
            }
        }

        public void AcceptToS()
        {
            this.Event("LicenceShown");
            this.Event("LicenceAccepted");
        }

        public FAuthData Register(string nickname)
        {
            this.Event("Register", "DEVICE", nickname);
            var data = this.ServiceCall("AuthService", "register", new object[]
            {
                new AuthData() { authType = AuthType.DEVICE, profileId = this.User.DeviceId },
                nickname,
                this.ClientInfo,
                new FRegistrationInfo() { age = "", email = "", gender = "", regType = "dv", socialId = "" },
            }) as FAuthData;

            this.User.UserId = data.info.userId;

            this.Event("ServerAuthSuccess", this.User.UserId);

            return data;
        }

        public void SetAvatar()
        {
            this.Event("AvatarPlayerGenderRace", "1", "1");
            this.Event("AvatarPlayerSubmit", "271891");
            this.ServiceCall("PlayerService", "saveUserSettings", new object[] { this.User.Avatar });
        }

        public void Init()
        {
            this.Event("LoadingScreenPercent", "100");
            this.Event("CreateAvatarByType", "MageMale");
            this.Event("LoadingScreenPercent", "100");
            this.Event("AvatarUpdateView", this.User.Avatar.ToString());
            this.Event("InitPushNotifications", "True");
        }

        public FBagUpdate GetUserItems()
        {
            var data = this.ServiceCall("ItemService", "getUserItems", null) as FBagUpdate;
            return data;
        }

        public FCreadex GetCreadex()
        {
            var data = this.ServiceCall("UserCreatureService", "getCreadex", new object[] { }) as FCreadex;
            return data;
        }

        public FUserCreaturesList GetUserCreatures()
        {
            var data = this.ServiceCall("UserCreatureService", "getUserCreatures", new object[] { }) as FUserCreaturesList;
            return data;
        }

        public FUpdate GetMapUpdate(float latitude, float longitude)
        {
            var data = this.ServiceCall("MapService", "getUpdate", new object[] {
                new FUpdateRequest()
                {
                    clientRequest = new FClientRequest()
                    {
                        time = 0,
                        currentUtcOffsetSeconds = 7200,
                        coordsWithAccuracy = new GeoCoordsWithAccuracy()
                        {
                            latitude = latitude,
                            longitude = longitude,
                            horizontalAccuracy = 20,
                        },
                    },
                    clientPlatform = ClientPlatform.IOS,
                    tilesCache = new Dictionary<FTile, long>(),
                }
            }) as FUpdate;
            return data;
        }
    }
}
