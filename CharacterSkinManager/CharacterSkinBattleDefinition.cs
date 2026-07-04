namespace CharacterSkinManager;

public sealed class CharacterSkinBattleDefinition
{
	public required string SkeletonDataPath { get; init; }

	public required string IdleAnimation { get; init; }

	public required string AttackAnimation { get; init; }

	public required string CastAnimation { get; init; }

	public required string HurtAnimation { get; init; }

	public required string DieAnimation { get; init; }

	public string? RelaxedAnimation { get; init; }
}
