using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace CharacterSkinManager;

public static class CharacterSkinSelectionStore
{
	private static readonly string _configDir = ProjectSettings.GlobalizePath("user://mods/character_skin_manager");

	private static readonly Dictionary<string, string?> _cache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	public static string? GetSelectedSkinId(string characterId)
	{
		if (_cache.TryGetValue(characterId, out string value))
		{
			return value;
		}
		string path = GetPath(characterId);
		try
		{
			if (File.Exists(path))
			{
				string text = File.ReadAllText(path).Trim();
				_cache[characterId] = text;
				return text;
			}
		}
		catch
		{
		}
		_cache[characterId] = null;
		return null;
	}

	public static void SetSelectedSkinId(string characterId, string skinId)
	{
		_cache[characterId] = skinId;
		Directory.CreateDirectory(_configDir);
		File.WriteAllText(GetPath(characterId), skinId);
	}

	private static string GetPath(string characterId)
	{
		return Path.Combine(_configDir, characterId.ToLowerInvariant() + ".txt");
	}
}
