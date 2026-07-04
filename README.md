# CharacterSkinManager Maintained

这是 `CharacterSkinManager` 的非官方维护版本，用来在《杀戮尖塔 2》更新后继续修复兼容性问题。

## Install

从 Releases 下载最新的 `CharacterSkinManager-*.zip`，解压后把 `CharacterSkinManager` 文件夹放到：

```text
<Slay the Spire 2>\mods\
```

最终结构应类似：

```text
<Slay the Spire 2>\mods\CharacterSkinManager\CharacterSkinManager.dll
<Slay the Spire 2>\mods\CharacterSkinManager\CharacterSkinManager.json
```

皮肤资源包按对应皮肤 mod 的说明安装。

## Build

需要本机已安装《杀戮尖塔 2》和 .NET SDK 9 或更新版本。

```powershell
$env:STS2_GAME_DIR = "E:\Games\SteamLibrary\steamapps\common\Slay the Spire 2"
dotnet build .\CharacterSkinManager.csproj -c Release
```

如果仓库就在游戏目录下，也可以直接运行：

```powershell
.\build.ps1
```

## Credits

- `Orca` mod 原作者：`WhiteRaven01`
- 局内皮肤替换 UI 和代码作者：`kawaiNekoMe`
- 本仓库只做后续游戏版本兼容性维护

本项目是非官方维护版本，与 Mega Crit 无关。
