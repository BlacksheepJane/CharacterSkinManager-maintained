using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using AlignmentMode = Godot.BoxContainer.AlignmentMode;
using FocusModeEnum = Godot.Control.FocusModeEnum;
using InternalMode = Godot.Node.InternalMode;
using LayoutPreset = Godot.Control.LayoutPreset;
using LayoutPresetMode = Godot.Control.LayoutPresetMode;
using MouseFilterEnum = Godot.Control.MouseFilterEnum;
using UpdateMode = Godot.SubViewport.UpdateMode;

namespace CharacterSkinManager;

internal static class CharacterSkinPanelController
{
	private const string PanelName = "CharacterSkinManagerPanel";

	private const string PanelTitle = "人物皮肤";

	private const string PrevButtonName = "PrevButton";

	private const string NextButtonName = "NextButton";

	private const string CloseButtonName = "CloseButton";

	private const string PreviewViewportName = "PreviewViewport";

	private const string PreviewRootName = "PreviewRoot";

	private const string SkinNameLabelName = "SkinNameLabel";

	private static readonly AccessTools.FieldRef<NCharacterSelectScreen, Control> _bgContainerRef = AccessTools.FieldRefAccess<NCharacterSelectScreen, Control>("_bgContainer");

	private static CharacterModel? _lastCharacter;

	private static bool _dismissedByUser;

	public static Control GetBackgroundContainer(NCharacterSelectScreen screen)
	{
		return _bgContainerRef.Invoke(screen);
	}

	public static void EnsureInjected(NCharacterSelectScreen screen)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Expected O, but got Unknown
		if (((Node)screen).GetNodeOrNull<Control>("CharacterSkinManagerPanel") == null)
		{
			PanelContainer panel = new PanelContainer
			{
				Name = "CharacterSkinManagerPanel",
				Visible = false,
				MouseFilter = (MouseFilterEnum)0,
				FocusMode = (FocusModeEnum)0,
				ZIndex = 200
			};
			((Control)panel).SetAnchorsAndOffsetsPreset((LayoutPreset)3, (LayoutPresetMode)3, 0);
			((Control)panel).OffsetLeft = -560f;
			((Control)panel).OffsetTop = -304f;
			((Control)panel).OffsetRight = -214f;
			((Control)panel).OffsetBottom = -76f;
			((Control)panel).AddThemeStyleboxOverride("panel", (StyleBox)(object)CreatePanelStyle());
			VBoxContainer val = new VBoxContainer
			{
				Alignment = (AlignmentMode)1
			};
			Label val2 = new Label
			{
				Text = "人物皮肤",
				HorizontalAlignment = (HorizontalAlignment)1
			};
			((Control)val2).AddThemeFontSizeOverride("font_size", 18);
			HBoxContainer val3 = new HBoxContainer
			{
				Alignment = (AlignmentMode)1
			};
			Button val4 = CreateArrowButton("PrevButton", "<");
			Button val5 = CreateArrowButton("NextButton", ">");
			Button val6 = CreateCloseButton();
			val6.Text = "x";
			((Control)val6).CustomMinimumSize = new Vector2(30f, 30f);
			((Control)val6).Size = new Vector2(30f, 30f);
			PanelContainer val7 = new PanelContainer
			{
				CustomMinimumSize = new Vector2(218f, 198f)
			};
			((Control)val7).AddThemeStyleboxOverride("panel", (StyleBox)(object)CreatePreviewCardStyle());
			VBoxContainer val8 = new VBoxContainer
			{
				Alignment = (AlignmentMode)1
			};
			SubViewportContainer val9 = new SubViewportContainer
			{
				CustomMinimumSize = new Vector2(170f, 156f),
				Stretch = true
			};
			SubViewport val10 = new SubViewport
			{
				Name = "PreviewViewport",
				Size = new Vector2I(340, 312),
				TransparentBg = true,
				HandleInputLocally = false,
				RenderTargetUpdateMode = (UpdateMode)4
			};
			Node2D val11 = new Node2D
			{
				Name = "PreviewRoot"
			};
			((Node)val10).AddChild((Node)(object)val11, false, (InternalMode)0);
			((Node)val9).AddChild((Node)(object)val10, false, (InternalMode)0);
			Label val12 = new Label
			{
				Name = "SkinNameLabel",
				HorizontalAlignment = (HorizontalAlignment)1
			};
			((Control)val12).AddThemeFontSizeOverride("font_size", 16);
			((Node)val8).AddChild((Node)(object)val9, false, (InternalMode)0);
			((Node)val8).AddChild((Node)(object)val12, false, (InternalMode)0);
			((Node)val7).AddChild((Node)(object)val8, false, (InternalMode)0);
			((BaseButton)val4).Pressed += delegate
			{
				SelectRelative(screen, -1);
			};
			((BaseButton)val5).Pressed += delegate
			{
				SelectRelative(screen, 1);
			};
			((BaseButton)val6).Pressed += delegate
			{
				_dismissedByUser = true;
				((CanvasItem)panel).Visible = false;
			};
			((Node)val3).AddChild((Node)(object)val4, false, (InternalMode)0);
			((Node)val3).AddChild((Node)(object)val7, false, (InternalMode)0);
			((Node)val3).AddChild((Node)(object)val5, false, (InternalMode)0);
			((Node)val).AddChild((Node)(object)val2, false, (InternalMode)0);
			((Node)val).AddChild((Node)(object)val3, false, (InternalMode)0);
			((Node)panel).AddChild((Node)(object)val, false, (InternalMode)0);
			Control val13 = new Control
			{
				Name = "CloseOverlay",
				MouseFilter = (MouseFilterEnum)2
			};
			val13.SetAnchorsAndOffsetsPreset((LayoutPreset)15, (LayoutPresetMode)3, 0);
			((Node)panel).AddChild((Node)(object)val13, false, (InternalMode)0);
			((Control)val6).SetAnchorsAndOffsetsPreset((LayoutPreset)1, (LayoutPresetMode)3, 0);
			((Control)val6).OffsetLeft = -48f;
			((Control)val6).OffsetTop = -2f;
			((Control)val6).OffsetRight = -6f;
			((Control)val6).OffsetBottom = 20f;
			((Node)val13).AddChild((Node)(object)val6, false, (InternalMode)0);
			((Node)screen).AddChild((Node)(object)panel, false, (InternalMode)0);
			Log.Info("[CharacterSkinManager] Skin panel injected into character select screen.", 2);
		}
	}

	public static void Refresh(NCharacterSelectScreen screen, CharacterModel? character)
	{
		CharacterSkinJsonLoader.RegisterExternalJsonSkins();
		_lastCharacter = character;
		Control nodeOrNull = ((Node)screen).GetNodeOrNull<Control>("CharacterSkinManagerPanel");
		if (nodeOrNull == null || !(((CanvasItem)nodeOrNull).Visible = character != null && CharacterSkinRuntime.HasSkins(character) && !_dismissedByUser))
		{
			return;
		}
		CharacterSkinDefinition selectedOrDefault = CharacterSkinRegistry.GetSelectedOrDefault(((AbstractModel)character).Id.Entry);
		if (selectedOrDefault != null)
		{
			Node obj = ((Node)nodeOrNull).FindChild("SkinNameLabel", true, false);
			Label val = (Label)(object)((obj is Label) ? obj : null);
			if (val != null)
			{
				val.Text = selectedOrDefault.DisplayName;
			}
			RebuildMiniPreview(screen, nodeOrNull, selectedOrDefault);
		}
	}

	private static void RebuildMiniPreview(NCharacterSelectScreen screen, Control panel, CharacterSkinDefinition selected)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		Node obj = ((Node)panel).FindChild("PreviewViewport", true, false);
		Node obj2 = ((obj is SubViewport) ? obj : null);
		Node val = ((Node)panel).FindChild("PreviewRoot", true, false);
		if (obj2 == null || val == null)
		{
			return;
		}
		foreach (Node child in val.GetChildren(false))
		{
			child.QueueFree();
		}
		try
		{
			Node val2 = ((IEnumerable)((Node)GetBackgroundContainer(screen)).GetChildren(false)).Cast<Node>().LastOrDefault();
			if (val2 == null)
			{
				return;
			}
			Node2D val3 = CharacterSkinNodeResolver.FindSpineNodes(val2).FirstOrDefault();
			if (val3 == null)
			{
				Log.Warn($"[CharacterSkinManager] Mini preview failed: no SpineSprite found under '{val2.Name}'.", 2);
				return;
			}
			Node2D val4 = new Node2D
			{
				Name = "PreviewStage",
				Position = selected.Preview.Position,
				Scale = selected.Preview.Scale,
				ZIndex = 10
			};
			val.AddChild((Node)(object)val4, false, (InternalMode)0);
			Node obj3 = ((Node)val3).Duplicate(15);
			Node2D val5 = (Node2D)(object)((obj3 is Node2D) ? obj3 : null);
			if (val5 != null)
			{
				((Node)val5).Name = "PreviewSpine";
				((CanvasItem)val5).ZIndex = 20;
				val5.Position = Vector2.Zero;
				CanvasItem val6 = (CanvasItem)(object)val5;
				if (val6 != null)
				{
					val6.Visible = true;
					val6.Modulate = Colors.White;
				}
				((Node)val4).AddChild((Node)(object)val5, false, (InternalMode)0);
				CharacterSkinSceneKind kind = ((!selected.Preview.UseBattleSkeleton) ? CharacterSkinSceneKind.CharacterSelect : CharacterSkinSceneKind.Battle);
				if (CharacterSkinRuntime.TryApplySingleNodeLoop(val5, selected, kind, selected.Preview.Animation, randomizeTrackTime: true))
				{
					Log.Info($"[CharacterSkinManager] Mini preview applied successfully for {selected.CharacterId}/{selected.SkinId}.", 2);
				}
			}
		}
		catch (Exception value)
		{
			Log.Error($"[CharacterSkinManager] Failed to rebuild mini preview: {value}", 2);
		}
	}

	private static void SelectRelative(NCharacterSelectScreen screen, int direction)
	{
		CharacterModel lastCharacter = _lastCharacter;
		if (lastCharacter == null)
		{
			return;
		}
		IReadOnlyList<CharacterSkinDefinition> registeredSkins = CharacterSkinRegistry.GetRegisteredSkins(((AbstractModel)lastCharacter).Id.Entry);
		if (registeredSkins.Count <= 1)
		{
			return;
		}
		CharacterSkinDefinition characterSkinDefinition = CharacterSkinRegistry.GetSelectedOrDefault(((AbstractModel)lastCharacter).Id.Entry) ?? registeredSkins[0];
		int num = 0;
		for (int i = 0; i < registeredSkins.Count; i++)
		{
			if (string.Equals(registeredSkins[i].SkinId, characterSkinDefinition.SkinId, StringComparison.OrdinalIgnoreCase))
			{
				num = i;
				break;
			}
		}
		int num2 = (num + direction) % registeredSkins.Count;
		if (num2 < 0)
		{
			num2 += registeredSkins.Count;
		}
		CharacterSkinSelectionStore.SetSelectedSkinId(((AbstractModel)lastCharacter).Id.Entry, registeredSkins[num2].SkinId);
		if (CharacterSkinManagerCompatibility.RuntimeSwapEnabled)
		{
			CharacterSkinRuntime.TryApplyCharacterSelectPreview(screen, lastCharacter);
		}
		Refresh(screen, lastCharacter);
	}

	private static Button CreateArrowButton(string name, string text)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		Button val = new Button
		{
			Name = name,
			Text = text,
			CustomMinimumSize = new Vector2(44f, 44f),
			FocusMode = (FocusModeEnum)0
		};
		StyleBoxFlat val2 = new StyleBoxFlat
		{
			BgColor = new Color(0f, 0f, 0f, 0f),
			CornerRadiusBottomLeft = 12,
			CornerRadiusBottomRight = 12,
			CornerRadiusTopLeft = 12,
			CornerRadiusTopRight = 12,
			BorderColor = new Color(0f, 0f, 0f, 0f),
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1
		};
		StyleBoxFlat val3 = new StyleBoxFlat
		{
			BgColor = new Color(0.96f, 0.72f, 0.15f, 0.95f),
			CornerRadiusBottomLeft = 12,
			CornerRadiusBottomRight = 12,
			CornerRadiusTopLeft = 12,
			CornerRadiusTopRight = 12
		};
		((Control)val).AddThemeStyleboxOverride("normal", (StyleBox)(object)val2);
		((Control)val).AddThemeStyleboxOverride("hover", (StyleBox)(object)val3);
		((Control)val).AddThemeStyleboxOverride("pressed", (StyleBox)(object)val3);
		((Control)val).AddThemeColorOverride("font_color", Colors.White);
		((Control)val).AddThemeColorOverride("font_hover_color", Colors.Black);
		((Control)val).AddThemeColorOverride("font_pressed_color", Colors.Black);
		((Control)val).AddThemeFontSizeOverride("font_size", 20);
		return val;
	}

	private static Button CreateCloseButton()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		Button val = new Button
		{
			Name = "CloseButton",
			Text = "×",
			CustomMinimumSize = new Vector2(22f, 22f),
			Size = new Vector2(22f, 22f),
			FocusMode = (FocusModeEnum)0,
			Flat = false,
			MouseFilter = (MouseFilterEnum)0
		};
		StyleBoxFlat val2 = new StyleBoxFlat
		{
			BgColor = new Color(0f, 0f, 0f, 0f),
			CornerRadiusBottomLeft = 6,
			CornerRadiusBottomRight = 6,
			CornerRadiusTopLeft = 6,
			CornerRadiusTopRight = 6,
			BorderColor = new Color(0f, 0f, 0f, 0f),
			BorderWidthLeft = 0,
			BorderWidthTop = 0,
			BorderWidthRight = 0,
			BorderWidthBottom = 0,
			ContentMarginLeft = 0f,
			ContentMarginTop = 0f,
			ContentMarginRight = 0f,
			ContentMarginBottom = 0f
		};
		StyleBoxFlat val3 = new StyleBoxFlat
		{
			BgColor = new Color(0.84f, 0.16f, 0.16f, 0.95f),
			CornerRadiusBottomLeft = 6,
			CornerRadiusBottomRight = 6,
			CornerRadiusTopLeft = 6,
			CornerRadiusTopRight = 6
		};
		((Control)val).AddThemeStyleboxOverride("normal", (StyleBox)(object)val2);
		((Control)val).AddThemeStyleboxOverride("hover", (StyleBox)(object)val3);
		((Control)val).AddThemeStyleboxOverride("pressed", (StyleBox)(object)val3);
		((Control)val).AddThemeColorOverride("font_color", Colors.White);
		((Control)val).AddThemeColorOverride("font_hover_color", Colors.White);
		((Control)val).AddThemeColorOverride("font_pressed_color", Colors.White);
		((Control)val).AddThemeFontSizeOverride("font_size", 20);
		return val;
	}

	public static void NotifyCharacterSelected(CharacterModel? character)
	{
		if (character != null && CharacterSkinRuntime.HasSkins(character))
		{
			_dismissedByUser = false;
		}
	}

	private static StyleBoxFlat CreatePanelStyle()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		return new StyleBoxFlat
		{
			BgColor = new Color(0f, 0f, 0f, 0.65f),
			CornerRadiusBottomLeft = 18,
			CornerRadiusBottomRight = 18,
			CornerRadiusTopLeft = 18,
			CornerRadiusTopRight = 18,
			BorderColor = new Color(0f, 0f, 0f, 0f),
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1,
			ContentMarginLeft = 12f,
			ContentMarginTop = 10f,
			ContentMarginRight = 12f,
			ContentMarginBottom = 10f
		};
	}

	private static StyleBoxFlat CreatePreviewCardStyle()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		return new StyleBoxFlat
		{
			BgColor = new Color(0f, 0f, 0f, 0f),
			CornerRadiusBottomLeft = 14,
			CornerRadiusBottomRight = 14,
			CornerRadiusTopLeft = 14,
			CornerRadiusTopRight = 14,
			BorderColor = new Color(0f, 0f, 0f, 0f),
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1,
			ContentMarginLeft = 8f,
			ContentMarginTop = 8f,
			ContentMarginRight = 8f,
			ContentMarginBottom = 8f
		};
	}
}
