using System;
using System.Collections.Generic;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;

namespace CharacterSkinManager.Patches;

[HarmonyPatch(typeof(NMerchantRoom), "AfterRoomIsLoaded")]
internal static class CharacterSkinMerchantPatches
{
	private static readonly AccessTools.FieldRef<NMerchantRoom, List<Player>> _playersRef = AccessTools.FieldRefAccess<NMerchantRoom, List<Player>>("_players");

	private static readonly AccessTools.FieldRef<NMerchantRoom, List<NMerchantCharacter>> _playerVisualsRef = AccessTools.FieldRefAccess<NMerchantRoom, List<NMerchantCharacter>>("_playerVisuals");

	private static void Postfix(NMerchantRoom __instance)
	{
		if (!CharacterSkinManagerCompatibility.RuntimeSwapEnabled)
		{
			return;
		}
		List<Player> list = _playersRef.Invoke(__instance);
		List<NMerchantCharacter> list2 = _playerVisualsRef.Invoke(__instance);
		int num = Math.Min(list.Count, list2.Count);
		for (int i = 0; i < num; i++)
		{
			CharacterSkinDefinition selectedOrDefault = CharacterSkinRegistry.GetSelectedOrDefault(((AbstractModel)list[i].Character).Id.Entry);
			if (selectedOrDefault != null)
			{
				Node2D childOrNull = ((Node)list2[i]).GetChildOrNull<Node2D>(0, false);
				if (childOrNull != null)
				{
					CharacterSkinRuntime.TryApplySingleNodeLoop(childOrNull, selectedOrDefault, CharacterSkinSceneKind.Merchant, selectedOrDefault.Merchant.Animation, randomizeTrackTime: true);
				}
			}
		}
	}
}
