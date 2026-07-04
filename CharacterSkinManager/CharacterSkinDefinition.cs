namespace CharacterSkinManager;

public sealed class CharacterSkinDefinition
{
	public required string CharacterId { get; init; }

	public required string SkinId { get; init; }

	public required string DisplayName { get; init; }

	public required CharacterSkinBattleDefinition Battle { get; init; }

	public required CharacterSkinLoopDefinition Merchant { get; init; }

	public required CharacterSkinRestDefinition Rest { get; init; }

	public required CharacterSkinLoopDefinition CharacterSelect { get; init; }

	public required CharacterSkinPreviewDefinition Preview { get; init; }
}
