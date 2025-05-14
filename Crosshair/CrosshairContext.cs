using System.Linq;
using Crossveil.Core;
using Crossveil.Utils;
using ProjectM;
using UnityEngine;

namespace Crossveil.Crosshair;

public readonly record struct CrosshairContext
{
	public bool ModEnabled { get; init; }
	public bool Hide { get; init; }
	public bool UseWindows { get; init; }
	public bool ChangeInMenus { get; init; }
	public bool ScaleEnabled { get; init; }
	public bool ScaleInMenus { get; init; }
	public float ScaleFactor { get; init; }
	public CursorType RequestedType { get; init; }
	public bool IsMenuContext { get; init; }
}

public readonly record struct CursorDecision(Texture2D Texture, Vector2 Hotspot, CursorMode Mode, bool Visible);

public static class CrosshairRules
{
	public static CursorDecision Decide(CrosshairContext ctx)
	{
		// Restore original
		if (!ctx.ModEnabled)
		{
			var original = CrosshairCache.RestoreOriginal(ctx.RequestedType);
			return new CursorDecision(original?.Texture, original?.Hotspot ?? Vector2.zero, CursorMode.Auto, true);
		}

		bool visible;

		// Use windows is toggled on
		if (ctx.UseWindows)
		{
			visible = !(ctx is { Hide: true, IsMenuContext: false });
			return new CursorDecision(null, Vector2.zero, CursorMode.Auto, visible);
		}

		// In menu & change in menus is toggled off
		if (ctx is { IsMenuContext: true, ChangeInMenus: false })
		{
			var original = CrosshairCache.RestoreOriginal(CursorType.Menu_Normal);
			return new CursorDecision(original?.Texture, original?.Hotspot ?? Vector2.zero, CursorMode.Auto, true);
		}

		var collection = Plugin.Collections.GetList().ElementAtOrDefault(Config.CollectionIndex.Value);

		if (collection == null || collection.Crosshairs.Count == 0)
		{
			Plugin.Log.LogWarning($"Invalid collection index {Config.CollectionIndex.Value}");

			goto RestoreFallback;
		}

		var crosshair = collection.Crosshairs.ElementAtOrDefault(Config.CrosshairIndex.Value);

		if (crosshair == null || !crosshair.Texture)
		{
			Plugin.Log.LogWarning(
				$"Invalid crosshair index {Config.CrosshairIndex.Value} in collection {collection.Name}");

			goto RestoreFallback;
		}

		var tex = crosshair.Texture;

		var scaleAllowed = ctx.ScaleEnabled && (!ctx.IsMenuContext || ctx.ScaleInMenus);

		if (scaleAllowed && !Mathf.Approximately(ctx.ScaleFactor, 1f))
			tex = CrosshairCache.GetOrAddScaled(
				Config.CollectionIndex.Value,
				Config.CrosshairIndex.Value,
				tex,
				ctx.ScaleFactor) ?? tex;

		// If texture is null, restore fallback
		if (!tex) goto RestoreFallback;

		var hotspot = HotspotUtils.GetHotspot(Config.HotspotIndex.Value, tex);
		var mode = scaleAllowed ? CursorMode.ForceSoftware : CursorMode.Auto;
		visible = !(ctx is { Hide: true, IsMenuContext: false });

		return new CursorDecision(tex, hotspot, mode, visible);

		RestoreFallback:
		var fallback = CrosshairCache.RestoreOriginal(ctx.RequestedType);
		return new CursorDecision(fallback?.Texture, fallback?.Hotspot ?? Vector2.zero, CursorMode.Auto, true);
	}
}