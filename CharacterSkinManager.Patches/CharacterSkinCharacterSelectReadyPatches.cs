using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace CharacterSkinManager.Patches;

[HarmonyPatch(typeof(NCharacterSelectScreen), "_Ready")]
internal static class CharacterSkinCharacterSelectReadyPatches
{
	private static void Postfix(NCharacterSelectScreen __instance)
	{
		Log.Info("[CharacterSkinManager] CharacterSelect _Ready patch fired; injecting panel.", 2);
		CharacterSkinJsonLoader.RegisterExternalJsonSkins();
		CharacterSkinPanelController.EnsureInjected(__instance);
	}
}
