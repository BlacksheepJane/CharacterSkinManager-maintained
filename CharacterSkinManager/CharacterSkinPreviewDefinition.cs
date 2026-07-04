using Godot;

namespace CharacterSkinManager;

public sealed class CharacterSkinPreviewDefinition
{
	public bool UseBattleSkeleton { get; init; } = true;

	public string Animation { get; init; } = "idle_loop";

	public Vector2 Scale { get; init; } = new Vector2(0.285f, 0.285f);

	public Vector2 Position { get; init; } = new Vector2(142f, 188f);
}
