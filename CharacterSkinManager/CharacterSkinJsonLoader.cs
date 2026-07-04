using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace CharacterSkinManager;

internal static class CharacterSkinJsonLoader
{
	private sealed class CharacterSkinJsonDefinition
	{
		[JsonPropertyName("character_id")]
		public string CharacterId { get; set; } = string.Empty;

		[JsonPropertyName("skin_id")]
		public string SkinId { get; set; } = string.Empty;

		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; } = string.Empty;

		[JsonPropertyName("battle")]
		public CharacterSkinBattleJsonDefinition? Battle { get; set; }

		[JsonPropertyName("merchant")]
		public CharacterSkinLoopJsonDefinition? Merchant { get; set; }

		[JsonPropertyName("rest")]
		public CharacterSkinRestJsonDefinition? Rest { get; set; }

		[JsonPropertyName("charselect")]
		public CharacterSkinLoopJsonDefinition? CharacterSelect { get; set; }

		[JsonPropertyName("preview")]
		public CharacterSkinPreviewJsonDefinition? Preview { get; set; }
	}

	private sealed class CharacterSkinBattleJsonDefinition
	{
		[JsonPropertyName("skeleton_data_path")]
		public string SkeletonDataPath { get; set; } = string.Empty;

		[JsonPropertyName("skel")]
		public string? Skel { get; set; }

		[JsonPropertyName("atlas")]
		public string? Atlas { get; set; }

		[JsonPropertyName("idle")]
		public string Idle { get; set; } = string.Empty;

		[JsonPropertyName("attack")]
		public string Attack { get; set; } = string.Empty;

		[JsonPropertyName("cast")]
		public string Cast { get; set; } = string.Empty;

		[JsonPropertyName("hurt")]
		public string Hurt { get; set; } = string.Empty;

		[JsonPropertyName("die")]
		public string Die { get; set; } = string.Empty;

		[JsonPropertyName("relaxed")]
		public string? Relaxed { get; set; }
	}

	private sealed class CharacterSkinLoopJsonDefinition
	{
		[JsonPropertyName("skeleton_data_path")]
		public string SkeletonDataPath { get; set; } = string.Empty;

		[JsonPropertyName("skel")]
		public string? Skel { get; set; }

		[JsonPropertyName("atlas")]
		public string? Atlas { get; set; }

		[JsonPropertyName("animation")]
		public string Animation { get; set; } = string.Empty;
	}

	private sealed class CharacterSkinRestJsonDefinition
	{
		[JsonPropertyName("skeleton_data_path")]
		public string SkeletonDataPath { get; set; } = string.Empty;

		[JsonPropertyName("skel")]
		public string? Skel { get; set; }

		[JsonPropertyName("atlas")]
		public string? Atlas { get; set; }

		[JsonPropertyName("act0")]
		public string Act0 { get; set; } = string.Empty;

		[JsonPropertyName("act1")]
		public string Act1 { get; set; } = string.Empty;

		[JsonPropertyName("act2")]
		public string Act2 { get; set; } = string.Empty;
	}

	private sealed class CharacterSkinPreviewJsonDefinition
	{
		[JsonPropertyName("use_battle_skeleton")]
		public bool UseBattleSkeleton { get; set; } = true;

		[JsonPropertyName("animation")]
		public string Animation { get; set; } = "idle_loop";

		[JsonPropertyName("scale_x")]
		public float ScaleX { get; set; } = 0.25f;

		[JsonPropertyName("scale_y")]
		public float ScaleY { get; set; } = 0.25f;

		[JsonPropertyName("position_x")]
		public float PositionX { get; set; } = 120f;

		[JsonPropertyName("position_y")]
		public float PositionY { get; set; } = 150f;
	}

	private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
	{
		PropertyNameCaseInsensitive = true
	};

	private static readonly HashSet<string> LoadedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

	public static void RegisterExternalJsonSkins()
	{
		foreach (string item in EnumerateSkinJsonPaths("res://skins"))
		{
			try
			{
				CharacterSkinDefinition characterSkinDefinition = LoadDefinition(item);
				if (characterSkinDefinition != null && LoadedPaths.Add(item))
				{
					CharacterSkinRegistry.Register(characterSkinDefinition);
					Log.Info($"[CharacterSkinManager] Registered json skin {characterSkinDefinition.CharacterId}/{characterSkinDefinition.SkinId} from {item}.", 2);
				}
			}
			catch (Exception value)
			{
				Log.Error($"[CharacterSkinManager] Failed to load skin.json at {item}: {value}", 2);
			}
		}
	}

	private static CharacterSkinDefinition? LoadDefinition(string path)
	{
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		Godot.FileAccess val = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
		if (val == null)
		{
			return null;
		}
		CharacterSkinJsonDefinition characterSkinJsonDefinition = JsonSerializer.Deserialize<CharacterSkinJsonDefinition>(val.GetAsText(false), Options);
		if (characterSkinJsonDefinition == null || string.IsNullOrWhiteSpace(characterSkinJsonDefinition.CharacterId) || string.IsNullOrWhiteSpace(characterSkinJsonDefinition.SkinId) || string.IsNullOrWhiteSpace(characterSkinJsonDefinition.DisplayName) || characterSkinJsonDefinition.Battle == null || characterSkinJsonDefinition.Merchant == null || characterSkinJsonDefinition.Rest == null || characterSkinJsonDefinition.CharacterSelect == null)
		{
			return null;
		}
		CharacterSkinPreviewJsonDefinition characterSkinPreviewJsonDefinition = characterSkinJsonDefinition.Preview ?? new CharacterSkinPreviewJsonDefinition();
		string text = ResolveSkeletonDataPath(characterSkinJsonDefinition.SkinId, "battle", characterSkinJsonDefinition.Battle.SkeletonDataPath, characterSkinJsonDefinition.Battle.Skel, characterSkinJsonDefinition.Battle.Atlas);
		string text2 = ResolveSkeletonDataPath(characterSkinJsonDefinition.SkinId, "merchant", characterSkinJsonDefinition.Merchant.SkeletonDataPath, characterSkinJsonDefinition.Merchant.Skel, characterSkinJsonDefinition.Merchant.Atlas);
		string text3 = ResolveSkeletonDataPath(characterSkinJsonDefinition.SkinId, "rest", characterSkinJsonDefinition.Rest.SkeletonDataPath, characterSkinJsonDefinition.Rest.Skel, characterSkinJsonDefinition.Rest.Atlas);
		string text4 = ResolveSkeletonDataPath(characterSkinJsonDefinition.SkinId, "charselect", characterSkinJsonDefinition.CharacterSelect.SkeletonDataPath, characterSkinJsonDefinition.CharacterSelect.Skel, characterSkinJsonDefinition.CharacterSelect.Atlas);
		if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(text2) || string.IsNullOrWhiteSpace(text3) || string.IsNullOrWhiteSpace(text4))
		{
			return null;
		}
		return new CharacterSkinDefinition
		{
			CharacterId = characterSkinJsonDefinition.CharacterId,
			SkinId = characterSkinJsonDefinition.SkinId,
			DisplayName = characterSkinJsonDefinition.DisplayName,
			Battle = new CharacterSkinBattleDefinition
			{
				SkeletonDataPath = text,
				IdleAnimation = characterSkinJsonDefinition.Battle.Idle,
				AttackAnimation = characterSkinJsonDefinition.Battle.Attack,
				CastAnimation = characterSkinJsonDefinition.Battle.Cast,
				HurtAnimation = characterSkinJsonDefinition.Battle.Hurt,
				DieAnimation = characterSkinJsonDefinition.Battle.Die,
				RelaxedAnimation = characterSkinJsonDefinition.Battle.Relaxed
			},
			Merchant = new CharacterSkinLoopDefinition
			{
				SkeletonDataPath = text2,
				Animation = characterSkinJsonDefinition.Merchant.Animation
			},
			Rest = new CharacterSkinRestDefinition
			{
				SkeletonDataPath = text3,
				Act0Animation = characterSkinJsonDefinition.Rest.Act0,
				Act1Animation = characterSkinJsonDefinition.Rest.Act1,
				Act2Animation = characterSkinJsonDefinition.Rest.Act2
			},
			CharacterSelect = new CharacterSkinLoopDefinition
			{
				SkeletonDataPath = text4,
				Animation = characterSkinJsonDefinition.CharacterSelect.Animation
			},
			Preview = new CharacterSkinPreviewDefinition
			{
				UseBattleSkeleton = characterSkinPreviewJsonDefinition.UseBattleSkeleton,
				Animation = (string.IsNullOrWhiteSpace(characterSkinPreviewJsonDefinition.Animation) ? "idle_loop" : characterSkinPreviewJsonDefinition.Animation),
				Scale = new Vector2(characterSkinPreviewJsonDefinition.ScaleX, characterSkinPreviewJsonDefinition.ScaleY),
				Position = new Vector2(characterSkinPreviewJsonDefinition.PositionX, characterSkinPreviewJsonDefinition.PositionY)
			}
		};
	}

	private static string? ResolveSkeletonDataPath(string skinId, string sceneKind, string? directPath, string? skelPath, string? atlasPath)
	{
		if (!string.IsNullOrWhiteSpace(directPath))
		{
			return directPath;
		}
		if (string.IsNullOrWhiteSpace(skelPath) || string.IsNullOrWhiteSpace(atlasPath))
		{
			return null;
		}
		return EnsureGeneratedSkeletonDataResource(skinId, sceneKind, skelPath, atlasPath);
	}

	private static string EnsureGeneratedSkeletonDataResource(string skinId, string sceneKind, string skelPath, string atlasPath)
	{
		string text = SanitizePathSegment(skinId);
		string text2 = SanitizePathSegment(sceneKind);
		string text3 = "user://character_skin_manager/generated/" + text + "/" + text2;
		string text4 = ProjectSettings.GlobalizePath(text3);
		Directory.CreateDirectory(text4);
		string text5 = text + "_" + text2 + "_skel_data.tres";
		string result = text3 + "/" + text5;
		string path = Path.Combine(text4, text5);
		string text6 = "[gd_resource type=\"SpineSkeletonDataResource\" load_steps=3 format=3]\n\n[ext_resource type=\"SpineAtlasResource\" path=\"" + atlasPath + "\" id=\"1\"]\n[ext_resource type=\"SpineSkeletonFileResource\" path=\"" + skelPath + "\" id=\"2\"]\n\n[resource]\natlas_res = ExtResource(\"1\")\nskeleton_file_res = ExtResource(\"2\")\ndefault_mix = 0.2\nanimation_mixes = {}\n";
		if (!File.Exists(path) || !string.Equals(File.ReadAllText(path), text6, StringComparison.Ordinal))
		{
			File.WriteAllText(path, text6, Utf8NoBom);
		}
		return result;
	}

	private static string SanitizePathSegment(string value)
	{
		StringBuilder stringBuilder = new StringBuilder(value.Length);
		foreach (char c in value)
		{
			StringBuilder stringBuilder2 = stringBuilder;
			bool flag = char.IsLetterOrDigit(c);
			if (!flag)
			{
				bool flag2 = ((c == '-' || c == '_') ? true : false);
				flag = flag2;
			}
			stringBuilder2.Append(flag ? c : '_');
		}
		if (stringBuilder.Length != 0)
		{
			return stringBuilder.ToString();
		}
		return "skin";
	}

	private static IEnumerable<string> EnumerateSkinJsonPaths(string root)
	{
		DirAccess dir = DirAccess.Open(root);
		if (dir == null)
		{
			yield break;
		}
		dir.ListDirBegin();
		try
		{
			while (true)
			{
				string next = dir.GetNext();
				if (string.IsNullOrEmpty(next))
				{
					break;
				}
				bool flag = ((next == "." || next == "..") ? true : false);
				if (flag || next.StartsWith('.'))
				{
					continue;
				}
				string text = root + "/" + next;
				if (dir.CurrentIsDir())
				{
					foreach (string item in EnumerateSkinJsonPaths(text))
					{
						yield return item;
					}
				}
				else if (string.Equals(next, "skin.json", StringComparison.OrdinalIgnoreCase))
				{
					yield return text;
				}
			}
		}
		finally
		{
			dir.ListDirEnd();
		}
	}
}
