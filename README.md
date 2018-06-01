`DracoLib.Core` [![NuGet](https://img.shields.io/nuget/v/DracoLib.Core.svg?maxAge=60)](https://www.nuget.org/packages/DracoLib.Core) [![NuGet](https://img.shields.io/nuget/v/DracoProtos.Core.svg?maxAge=60)](https://www.nuget.org/packages/DracoProtos.Core) [![Build status](https://ci.appveyor.com/api/projects/status/9t9ivl3bahv92u45/branch/master?svg=true)](https://ci.appveyor.com/project/RocketBot/dracolib/branch/master) [![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=SNATC29B4ZJD4) [![Discord](https://img.shields.io/badge/Discord-Online-blue.svg)](https://discord.gg/bsHQC2Y)
===================

#### `How to use`

All api calls can be done manuall using the `.Call(service, method, args)` method.

```CSharp
using DracoLib.Core;
using DracoProtos.Core.Objects;
using System;

DracoClient draco = new DracoClient(/*"http://localhost:8888"*/); //Proxys here

var response = draco.Call("AuthService", "trySingIn", new object[]
{
    new AuthData() { authType = this.Auth.Type, profileId = this.Auth.ProfileId, tokenId = this.Auth.TokenId },
    this.ClientInfo,
    new FRegistrationInfo(this.Auth.Reg) { email = this.User.Username },
}) as FAuthData;
```

More high level methods also exists, here is a more complete example that get user items:

```CSharp
using DracoLib.Core;
using DracoLib.Core.Text;
using DracoLib.Core.Utils;
using DracoProtos.Core.Objects;
using System;

User config = new User()
{
    Username = "xxxxxxx@gmail.com",
    Password = "xxxxxxx",
    DeviceId = DracoUtils.GenerateDeviceId(),
    Login = "GOOGLE"
};

var draco = new DracoClient(/*"http://localhost:8888"*/); //Proxys here

if (!draco.Ping())
    throw new Exception();

draco.Boot(config);
draco.Login();
draco.Load();

var response = draco.Inventory.GetUserItems() as FBagUpdate;
foreach (var item in response.items) {
	Console.WriteLine($"  item = { English.Load["key.item." + item.type.ToString()]}, count = { item.count}");
}
```

More example can be found here: https://github.com/Furtif/DracoLib/tree/master/Demos

#### `Cloning Source Code`

Next, you need to get the source code.  This source code repository uses git submodules. So when you clone the source code, you will need to clone recursively:

```
git clone --recursive https://github.com/Furtif/DracoLib.git
```

Or if you already cloned without the recursive option, you can update the submodules by running:

```
git clone --recursive https://github.com/Furtif/DracoLib.git
cd DracoLib
git submodule update --init --recursive
```

#### `Versioning`

We are following [semantic versioning](http://semver.org/) for DracoProtos.  Every version will be mapped to their current Draconius Go version.

| Version      | App Version                 | Extra                     |
|--------------|-----------------------------|---------------------------|
| 1.0.4        | 1.8                         | first tested              |

#### `CREDITS`
[DracoApi (niicojs, SL-x-TnT)](https://github.com/dracoapi)
