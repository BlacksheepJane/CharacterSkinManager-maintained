using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Random;
using CacheMode = Godot.ResourceLoader.CacheMode;

namespace CharacterSkinManager;

internal static class CharacterSkinRuntime
{
	public static bool HasSkins(CharacterModel? model)
	{
		if (model != null)
		{
			return CharacterSkinRegistry.HasRegisteredSkins(((AbstractModel)model).Id.Entry);
		}
		return false;
	}

	public static bool HasSkinsForCreature(NCreature creature)
	{
		Creature entity = creature.Entity;
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
		if (obj != null)
		{
			return HasSkins(creature.Entity.Player.Character);
		}
		return false;
	}

	public static CharacterSkinDefinition? GetSelectedSkin(CharacterModel model)
	{
		return CharacterSkinRegistry.GetSelectedOrDefault(((AbstractModel)model).Id.Entry);
	}

	public static MegaSkeletonDataResource? LoadSkeleton(CharacterSkinDefinition definition, CharacterSkinSceneKind kind)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		string text = kind switch
		{
			CharacterSkinSceneKind.Battle => definition.Battle.SkeletonDataPath, 
			CharacterSkinSceneKind.Merchant => definition.Merchant.SkeletonDataPath, 
			CharacterSkinSceneKind.Rest => definition.Rest.SkeletonDataPath, 
			CharacterSkinSceneKind.CharacterSelect => definition.CharacterSelect.SkeletonDataPath, 
			_ => definition.Battle.SkeletonDataPath, 
		};
		Resource val = ResourceLoader.Load<Resource>(text, (string)null, CacheMode.Reuse);
		if (val == null)
		{
			Log.Warn("[CharacterSkinManager] Missing skeleton resource at " + text + ".", 2);
			return null;
		}
		return new MegaSkeletonDataResource((Variant)(GodotObject)(object)val);
	}

	public static bool TryApplyBattleSkeleton(NCreature creature)
	{
		Creature entity = creature.Entity;
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
		if (val == null)
		{
			return false;
		}
		CharacterSkinDefinition selectedSkin = GetSelectedSkin(val);
		MegaSprite val2 = CharacterSkinNodeResolver.TryGetSpineController(creature);
		if (selectedSkin == null || val2 == null)
		{
			return false;
		}
		MegaSkeletonDataResource val3 = LoadSkeleton(selectedSkin, CharacterSkinSceneKind.Battle);
		if (val3 == null)
		{
			return false;
		}
		try
		{
			val2.SetSkeletonDataRes(val3);
			return true;
		}
		catch (Exception value)
		{
			Log.Error($"[CharacterSkinManager] Failed to apply battle skeleton for {((AbstractModel)val).Id.Entry}: {value}", 2);
			return false;
		}
	}

	public static CreatureAnimator? CreateBattleAnimator(CharacterModel model, MegaSprite spine, string context)
	{
		return model.GenerateAnimator(spine);
	}

	public static bool TryApplyLoopToNode(Node node, CharacterSkinDefinition definition, CharacterSkinSceneKind kind, string animationName, bool randomizeTrackTime)
	{
		MegaSkeletonDataResource val = LoadSkeleton(definition, kind);
		if (val == null)
		{
			return false;
		}
		bool result = false;
		foreach (Node2D item in CharacterSkinNodeResolver.FindSpineNodes(node))
		{
			MegaSprite val2 = CharacterSkinNodeResolver.TryWrap(item);
			if (val2 == null)
			{
				continue;
			}
			try
			{
				val2.SetSkeletonDataRes(val);
				MegaAnimationState animationState = val2.GetAnimationState();
				animationState.SetAnimation(animationName, true, 0);
				MegaTrackEntry val3 = animationState.GetCurrent(0);
				if (val3 != null && randomizeTrackTime)
				{
					val3.SetTrackTime(val3.GetAnimationEnd() * Rng.Chaotic.NextFloat(1f));
				}
				result = true;
			}
			catch (Exception value)
			{
				Log.Error($"[CharacterSkinManager] Failed to apply {kind} loop '{animationName}' on node '{((Node)item).Name}': {value}", 2);
			}
		}
		return result;
	}

	public static bool TryApplySingleNodeLoop(Node2D node, CharacterSkinDefinition definition, CharacterSkinSceneKind kind, string animationName, bool randomizeTrackTime)
	{
		MegaSkeletonDataResource val = LoadSkeleton(definition, kind);
		MegaSprite val2 = CharacterSkinNodeResolver.TryWrap(node);
		if (val == null || val2 == null)
		{
			return false;
		}
		try
		{
			val2.SetSkeletonDataRes(val);
			MegaAnimationState animationState = val2.GetAnimationState();
			animationState.SetAnimation(animationName, true, 0);
			MegaTrackEntry val3 = animationState.GetCurrent(0);
			if (val3 != null && randomizeTrackTime)
			{
				val3.SetTrackTime(val3.GetAnimationEnd() * Rng.Chaotic.NextFloat(1f));
			}
			return true;
		}
		catch (Exception value)
		{
			Log.Error($"[CharacterSkinManager] Failed to apply {kind} loop '{animationName}' on '{((Node)node).Name}': {value}", 2);
			return false;
		}
	}

	public static bool TryApplyCharacterSelectPreview(NCharacterSelectScreen screen, CharacterModel model)
	{
		CharacterSkinDefinition selectedSkin = GetSelectedSkin(model);
		if (selectedSkin == null)
		{
			return false;
		}
		Node val = ((IEnumerable)((Node)CharacterSkinPanelController.GetBackgroundContainer(screen)).GetChildren(false)).Cast<Node>().LastOrDefault();
		if (val == null)
		{
			return false;
		}
		return TryApplyLoopToNode(val, selectedSkin, CharacterSkinSceneKind.CharacterSelect, selectedSkin.CharacterSelect.Animation, randomizeTrackTime: false);
	}

	private static string ResolveAnimation(IReadOnlyList<string> names, params string?[] preferred)
	{
		foreach (string preferredName in preferred.Where((string p) => !string.IsNullOrWhiteSpace(p)))
		{
			string text = names.FirstOrDefault((string name) => string.Equals(name, preferredName, StringComparison.OrdinalIgnoreCase));
			if (text != null)
			{
				return text;
			}
			string text2 = names.FirstOrDefault((string name) => name.Contains(preferredName, StringComparison.OrdinalIgnoreCase));
			if (text2 != null)
			{
				return text2;
			}
		}
		return names[0];
	}
}
