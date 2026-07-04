using System.Collections.Generic;

namespace CharacterSkinManager;

public static class CharacterSkinManagerApi
{
	public static void RegisterSkin(CharacterSkinDefinition definition)
	{
		CharacterSkinRegistry.Register(definition);
	}

	public static void RegisterSkins(IEnumerable<CharacterSkinDefinition> definitions)
	{
		CharacterSkinRegistry.RegisterRange(definitions);
	}

	public static IReadOnlyList<CharacterSkinDefinition> GetRegisteredSkins(string characterId)
	{
		return CharacterSkinRegistry.GetRegisteredSkins(characterId);
	}

	public static CharacterSkinDefinition? GetSelectedOrDefault(string characterId)
	{
		return CharacterSkinRegistry.GetSelectedOrDefault(characterId);
	}

	public static string? GetSelectedSkinId(string characterId)
	{
		return CharacterSkinSelectionStore.GetSelectedSkinId(characterId);
	}

	public static void SetSelectedSkinId(string characterId, string skinId)
	{
		CharacterSkinSelectionStore.SetSelectedSkinId(characterId, skinId);
	}
}
