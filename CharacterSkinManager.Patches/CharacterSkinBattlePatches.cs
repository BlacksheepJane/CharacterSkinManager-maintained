using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace CharacterSkinManager.Patches;

[HarmonyPatch(typeof(NCreature), "_Ready")]
internal static class CharacterSkinBattlePatches
{
	private static readonly AccessTools.FieldRef<NCreature, CreatureAnimator> _spineAnimatorRef = AccessTools.FieldRefAccess<NCreature, CreatureAnimator>("_spineAnimator");

	private static readonly Action<NCreature> _connectSignals = AccessTools.MethodDelegate<Action<NCreature>>(AccessTools.Method(typeof(NCreature), "ConnectSpineAnimatorSignals", (Type[])null, (Type[])null), (object)null, true, (Type[])null);

	private static void Postfix(NCreature __instance)
	{
		if (!CharacterSkinManagerCompatibility.RuntimeSwapEnabled || !CharacterSkinRuntime.HasSkinsForCreature(__instance))
		{
			return;
		}
		Creature entity = __instance.Entity;
		object obj;
		if (entity == null)
		{
			obj = null;
		}
		else
		{
			Player player = entity.Player;
			obj = ((player != null) ? player.Character : null);
		}
		CharacterModel val = (CharacterModel)obj;
		if (val == null || !CharacterSkinRuntime.TryApplyBattleSkeleton(__instance))
		{
			return;
		}
		MegaSprite val2 = CharacterSkinNodeResolver.TryGetSpineController(__instance);
		if (val2 != null)
		{
			CreatureAnimator val3 = CharacterSkinRuntime.CreateBattleAnimator(val, val2, "NCreature._Ready");
			if (val3 != null)
			{
				_spineAnimatorRef.Invoke(__instance) = val3;
				_connectSignals(__instance);
			}
		}
	}
}
