# CharacterSkinManager Maintained

Unofficial compatibility maintenance for the Slay the Spire 2 mod `CharacterSkinManager`.

The original mod appears to no longer be updated, while Slay the Spire 2 is still changing quickly. This repository exists to keep `CharacterSkinManager` working across game updates by applying small compatibility fixes, documenting breakages, and publishing rebuilt DLLs when the game API changes.

## Current Status

- Target game version: `v0.108.0`
- Original mod manifest version: `v0.0.3`
- Maintenance patch: `1.0.1-sts2.0.108.0`
- Known fixed issue: `MegaAnimationState.SetAnimation(...)` no longer returns `MegaTrackEntry` in `v0.108.0`; the mod now calls `SetAnimation(...)` and then `GetCurrent(0)`.

## Repository Scope

This repository is for compatibility maintenance only.

- Keep `CharacterSkinManager` loading on current Slay the Spire 2 builds.
- Prefer minimal fixes over feature changes.
- Do not include Slay the Spire 2 game binaries or assets.
- Do not include third-party skin asset packs unless their license explicitly allows redistribution.

## Build

Requirements:

- .NET SDK 9 or newer
- A local Slay the Spire 2 installation

From PowerShell:

```powershell
$env:STS2_GAME_DIR = "E:\Games\SteamLibrary\steamapps\common\Slay the Spire 2"
dotnet build .\CharacterSkinManager.csproj -c Release
```

If this repository is checked out directly inside the game directory, `STS2_GAME_DIR` is optional because the project defaults to the parent directory.

The project references these local game assemblies at build time:

- `data_sts2_windows_x86_64\sts2.dll`
- `data_sts2_windows_x86_64\GodotSharp.dll`
- `data_sts2_windows_x86_64\0Harmony.dll`

## Install Locally

Copy the built `CharacterSkinManager.dll` and the manifest from `manifest/CharacterSkinManager.json` into:

```text
<Slay the Spire 2>\mods\CharacterSkinManager
```

Then place compatible skin packs beside it as usual.

## Maintenance Notes

When a new Slay the Spire 2 version breaks the mod:

1. Reproduce with a clean mod list.
2. Check `C:\Users\<you>\AppData\Roaming\SlayTheSpire2\logs`.
3. Search for `CharacterSkinManager`, `MissingMethodException`, `MissingFieldException`, `TypeLoadException`, or Harmony patch failures.
4. Keep fixes small and note the affected game version in `CHANGELOG.md`.

## Credits

Original author in the mod manifest: `BiliBili-KawaiNekoMe`.

This is an unofficial maintenance fork. It is not affiliated with Mega Crit or the original mod author.
