# CharacterSkinManager Maintained

本仓库为 银龙奥卡战士皮肤-奥卡奥伽尼斯 的换肤依赖 `CharacterSkinManager` 的反编译后非官方维护版本，用于在《Slay the Spire 2》更新后修复兼容性问题。

可在`v0.108.0`上正常运行。

## Install

[![Release](https://img.shields.io/github/v/release/BlacksheepJane/CharacterSkinManager-maintained?label=release)](https://github.com/BlacksheepJane/CharacterSkinManager-maintained/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/BlacksheepJane/CharacterSkinManager-maintained/total?label=downloads)](https://github.com/BlacksheepJane/CharacterSkinManager-maintained/releases)

从 Releases 下载最新的 `CharacterSkinManager-*.zip`，解压后用 `CharacterSkinManager` 文件夹替换：

```text
<Slay the Spire 2>\mods\Orca\CharacterSkinManager v0.0.2
```

最终结构应类似：

```text
<Slay the Spire 2>\mods\Orca\CharacterSkinManager\CharacterSkinManager.dll
<Slay the Spire 2>\mods\Orca\CharacterSkinManager\CharacterSkinManager.json
```

## Build

如果你希望自行编译或开发：

需要本机已安装《Slay the Spire 2》和 .NET SDK 9 或更新版本。

```powershell
$env:STS2_GAME_DIR = "E:\Games\SteamLibrary\steamapps\common\Slay the Spire 2"
dotnet build .\CharacterSkinManager.csproj -c Release
```

## Credits and Acknowledgements

- `Orca` mod 原作者：`WhiteRaven01`
- 局内皮肤替换 UI 和代码作者：`kawaiNekoMe`
- Codex参与了本项目开发
