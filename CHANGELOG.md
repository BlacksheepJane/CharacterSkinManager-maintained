# Changelog

## Unreleased

- Organized the decompiled `CharacterSkinManager` source into a GitHub-ready maintenance repository.
- Added local build references for Slay the Spire 2 game assemblies.
- Added repository documentation and compatibility notes.

## 1.0.1-sts2.0.108.0

- Fixed compatibility with Slay the Spire 2 `v0.108.0`.
- Updated animation loop application after `MegaAnimationState.SetAnimation(string, bool, int)` changed from returning `MegaTrackEntry` to a fire-and-forget call.
- Replaced direct use of the old return value with `SetAnimation(...)` followed by `GetCurrent(0)`.
