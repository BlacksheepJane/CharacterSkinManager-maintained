using System;
using System.Collections.Generic;
using System.Linq;

namespace CharacterSkinManager;

public static class CharacterSkinRegistry
{
	private static readonly Dictionary<string, List<CharacterSkinDefinition>> _skinsByCharacter = new Dictionary<string, List<CharacterSkinDefinition>>(StringComparer.OrdinalIgnoreCase);

	public static void Register(CharacterSkinDefinition definition)
	{
		ArgumentNullException.ThrowIfNull(definition, "definition");
		if (!_skinsByCharacter.TryGetValue(definition.CharacterId, out List<CharacterSkinDefinition> value))
		{
			value = new List<CharacterSkinDefinition>();
			_skinsByCharacter[definition.CharacterId] = value;
		}
		CharacterSkinDefinition characterSkinDefinition = value.FirstOrDefault((CharacterSkinDefinition def) => string.Equals(def.SkinId, definition.SkinId, StringComparison.OrdinalIgnoreCase));
		if (characterSkinDefinition != null)
		{
			value.Remove(characterSkinDefinition);
		}
		value.Add(definition);
	}

	public static void RegisterRange(IEnumerable<CharacterSkinDefinition> definitions)
	{
		foreach (CharacterSkinDefinition definition in definitions)
		{
			Register(definition);
		}
	}

	public static bool HasRegisteredSkins(string characterId)
	{
		if (_skinsByCharacter.TryGetValue(characterId, out List<CharacterSkinDefinition> value))
		{
			return value.Count > 0;
		}
		return false;
	}

	public static IReadOnlyList<CharacterSkinDefinition> GetRegisteredSkins(string characterId)
	{
		if (!_skinsByCharacter.TryGetValue(characterId, out List<CharacterSkinDefinition> value))
		{
			return new List<CharacterSkinDefinition>();
		}
		return value;
	}

	public static CharacterSkinDefinition? GetSkin(string characterId, string skinId)
	{
		return GetRegisteredSkins(characterId).FirstOrDefault((CharacterSkinDefinition def) => string.Equals(def.SkinId, skinId, StringComparison.OrdinalIgnoreCase));
	}

	public static CharacterSkinDefinition? GetDefaultSkin(string characterId)
	{
		return GetRegisteredSkins(characterId).FirstOrDefault();
	}

	public static CharacterSkinDefinition? GetSelectedOrDefault(string characterId)
	{
		string selectedSkinId = CharacterSkinSelectionStore.GetSelectedSkinId(characterId);
		if (!string.IsNullOrWhiteSpace(selectedSkinId))
		{
			CharacterSkinDefinition skin = GetSkin(characterId, selectedSkinId);
			if (skin != null)
			{
				return skin;
			}
		}
		return GetDefaultSkin(characterId);
	}
}
