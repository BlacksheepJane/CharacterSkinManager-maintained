# Compatibility Notes

## Slay the Spire 2 v0.108.0

### Symptom

The game reaches character select, then selecting a character can log:

```text
System.MissingMethodException:
Method not found:
MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaTrackEntry
MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimationState.SetAnimation(System.String, Boolean, Int32)
```

### Cause

In `v0.108.0`, `MegaAnimationState.SetAnimation(string, bool, int)` is still present, but it is now a fire-and-forget method. The old mod binary expected it to return `MegaTrackEntry`.

### Fix

Use:

```csharp
MegaAnimationState animationState = megaSprite.GetAnimationState();
animationState.SetAnimation(animationName, true, 0);
MegaTrackEntry entry = animationState.GetCurrent(0);
```

instead of assigning the return value from `SetAnimation(...)`.
