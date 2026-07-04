using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace CharacterSkinManager;

[ModInitializer("Initialize")]
public static class CharacterSkinManagerMod
{
	private const string HarmonyId = "neko.sts2.character_skin_manager";

	public static void Initialize()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Log.Info("[CharacterSkinManager] Initializing Character Skin Manager...", 2);
		CharacterSkinJsonLoader.RegisterExternalJsonSkins();
		new Harmony("neko.sts2.character_skin_manager").PatchAll(typeof(CharacterSkinManagerMod).Assembly);
	}
}
