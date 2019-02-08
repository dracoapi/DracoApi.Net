<!-- define variables -->
[1.1]: http://i.imgur.com/M4fJ65n.png (ATTENTION)

### DracoLib.Core [![NuGet](https://img.shields.io/nuget/v/DracoLib.Core.svg?maxAge=60)](https://www.nuget.org/packages/DracoLib.Core) - DracoProtos.Core [![NuGet](https://img.shields.io/nuget/v/DracoProtos.Core.svg?maxAge=60)](https://www.nuget.org/packages/DracoProtos.Core)
[![Build status](https://ci.appveyor.com/api/projects/status/nmg6choixfkj372v/branch/master?svg=true)](https://ci.appveyor.com/project/RocketBot/dracolib-core/branch/master) [![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.me/rocketbot) [![Discord](https://img.shields.io/badge/Discord-Online-blue.svg)](https://discord.gg/bsHQC2Y)
===================

![alt text][1.1] <strong><em>`The contents of this repo are a proof of concept and are for educational use only`</em></strong>![alt text][1.1]<br/>

#### `How to use`

More high level methods also exists, here is a more complete example that get user items:

```CSharp
using DracoLib.Core;
using DracoLib.Core.Text;
using DracoLib.Core.Utils;
using DracoProtos.Core.Base;
using System;

User user = new User()
{
	Username = "xxxxxxx@gmail.com",
	Password = "xxxxxxx",
	DeviceId = DracoUtils.GenerateDeviceId(),
	LoginType = AuthType.GOOGLE,
	Language = Langues.English.ToString(),
	UtcOffset = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds * 60,
	TimeOut = 20 * 1000
};

var draco = new DracoClient(user, null /*WebProxy here*/);

if (!draco.Ping())
    throw new Exception();

draco.Boot();
draco.Login();
draco.Load();

var response = draco.Inventory.GetUserItems();
foreach (var item in response.items) 
    Console.WriteLine($"    Item = { draco.Strings.GetItemName(item.type) }, count = { item.count}");

```

More example can be found here: https://github.com/Furtif/DracoLib/tree/master/Demos

## `Developers and Contributors`

### `Requirements`

To contribute to development, you will need to download and install the required software first.

- [Git](https://git-scm.com/downloads)
- [NET Core (>= 2.0) SDK](https://www.microsoft.com/net/download/windows)
- [Visual Studio 2017](https://www.visualstudio.com/vs/whatsnew/) - We are using C# 7.0 code so Visual Studio 2017 is required to compile. Visual Studio 2015 or older will not be able to compile the code.

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
| 1.2.2.0      | 1.10                        | Compatible                |

#### `CREDITS`
 - [DracoApi (niicojs, SL-x-TnT)](https://github.com/dracoapi)
 - [AeonLucid](https://github.com/AeonLucid)
 - [Jwt.Net](https://github.com/jwt-dotnet)
