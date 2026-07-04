using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.RestSite;

namespace CharacterSkinManager.Patches;

[HarmonyPatch(typeof(NRestSiteCharacter), "_Ready")]
internal static class CharacterSkinRestPatches
{
	private static void Postfix(NRestSiteCharacter __instance)
	{
		if (!CharacterSkinManagerCompatibility.RuntimeSwapEnabled)
		{
			return;
		}
		Player player = __instance.Player;
		if (((player != null) ? player.Character : null) != null)
		{
			CharacterSkinDefinition selectedOrDefault = CharacterSkinRegistry.GetSelectedOrDefault(((AbstractModel)__instance.Player.Character).Id.Entry);
			if (selectedOrDefault != null)
			{
				CharacterSkinRuntime.TryApplyLoopToNode((Node)(object)__instance, selectedOrDefault, CharacterSkinSceneKind.Rest, __instance.Player.RunState.CurrentActIndex switch
				{
					0 => selectedOrDefault.Rest.Act0Animation, 
					1 => selectedOrDefault.Rest.Act1Animation, 
					2 => selectedOrDefault.Rest.Act2Animation, 
					_ => selectedOrDefault.Rest.Act0Animation, 
				}, randomizeTrackTime: true);
			}
		}
	}
}
