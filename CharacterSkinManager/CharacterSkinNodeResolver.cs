using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace CharacterSkinManager;

internal static class CharacterSkinNodeResolver
{
	public static Node? FindCreatureVisualsNode(NCreature creature)
	{
		foreach (Node child in ((Node)creature).GetChildren(false))
		{
			if (((object)child).GetType().Name.Equals("NCreatureVisuals", StringComparison.Ordinal))
			{
				return child;
			}
		}
		return null;
	}

	public static Node2D? FindPrimaryVisualBody(Node visualsRoot)
	{
		foreach (Node2D item in EnumerateNode2D(visualsRoot))
		{
			if (((object)((Node)item).Name).ToString().Equals("Visuals", StringComparison.OrdinalIgnoreCase))
			{
				return item;
			}
		}
		return EnumerateNode2D(visualsRoot).FirstOrDefault((Func<Node2D, bool>)((Node2D node) => !(node is Marker2D)));
	}

	public static IEnumerable<Node2D> FindSpineNodes(Node root)
	{
		foreach (Node2D item in EnumerateNode2D(root))
		{
			if (((GodotObject)item).GetClass().Equals("SpineSprite", StringComparison.Ordinal))
			{
				yield return item;
			}
		}
	}

	public static MegaSprite? TryGetSpineController(NCreature creature)
	{
		Node val = FindCreatureVisualsNode(creature);
		return TryWrap((val != null) ? FindPrimaryVisualBody(val) : null);
	}

	public static MegaSprite? TryWrap(Node2D? node)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		if (node == null)
		{
			return null;
		}
		try
		{
			return new MegaSprite((Variant)(GodotObject)(object)node);
		}
		catch
		{
			return null;
		}
	}

	private static IEnumerable<Node2D> EnumerateNode2D(Node root)
	{
		foreach (Node child in root.GetChildren(false))
		{
			Node2D val = (Node2D)(object)((child is Node2D) ? child : null);
			if (val != null)
			{
				yield return val;
			}
			foreach (Node2D item in EnumerateNode2D(child))
			{
				yield return item;
			}
		}
	}
}
