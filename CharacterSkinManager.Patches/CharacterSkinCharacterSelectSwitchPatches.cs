using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace CharacterSkinManager.Patches;

[HarmonyPatch(typeof(NCharacterSelectScreen), "SelectCharacter")]
internal static class CharacterSkinCharacterSelectSwitchPatches
{
	private static void Postfix(NCharacterSelectScreen __instance, CharacterModel characterModel)
	{
		Log.Info("[CharacterSkinManager] SelectCharacter patch fired for " + ((AbstractModel)characterModel).Id.Entry + ".", 2);
		CharacterSkinPanelController.NotifyCharacterSelected(characterModel);
		if (CharacterSkinManagerCompatibility.RuntimeSwapEnabled && CharacterSkinRuntime.HasSkins(characterModel))
		{
			CharacterSkinRuntime.TryApplyCharacterSelectPreview(__instance, characterModel);
		}
		CharacterSkinPanelController.Refresh(__instance, characterModel);
	}
}
