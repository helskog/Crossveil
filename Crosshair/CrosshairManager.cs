using Crossveil.Core;
using Crossveil.Utils;

using ProjectM;
using UnityEngine;
using System.Linq;
using ProjectM.UI;
using static UnityEngine.UI.Image;

namespace Crossveil.Crosshair;

public static class CrosshairManager
{
	public static bool isMenuContext;

	public static void ApplyCurrent()
	{
		bool isInMainMenu = ViewManager.IsInMenu();
		bool isInOtherMenu = Plugin.InMenuState;
		isMenuContext = isInMainMenu || isInOtherMenu;

		if (Config.ModEnabled.Value && Config.ScaleEnabled.Value)
		{
			EnsureScalingInitialized();
		}
		
		if (!Config.ModEnabled.Value)
		{
			RestoreOriginal(isMenuContext ? CursorType.Menu_Normal : CursorType.Game_Normal);

			return;
		}

		var type = isMenuContext ? CursorType.Menu_Normal : CursorType.Game_Normal;
		bool allowInMenu = Config.ChangeInMenus.Value;

		if (isMenuContext && !allowInMenu)
		{
			RestoreOriginal(CursorType.Menu_Normal);
			return;
		}

		var cursor = GetCustomCrosshair(type);

		if (cursor != null)
			ApplyCursor(cursor);
	}

	private static void EnsureScalingInitialized()
	{
		CrosshairCache.ClearScaled();
		Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
	}
	
	private static CursorData GetCustomCrosshair(CursorType type)
	{
		int collectionIndex = Config.CollectionIndex.Value;
		int crosshairIndex = Config.CrosshairIndex.Value;
		float scale = Config.ScaleFactor.Value;
		bool scaleEnabled = Config.ScaleEnabled.Value;

		var collection = Plugin.Collections.GetList().ElementAtOrDefault(collectionIndex);
		if (collection == null || collection.Crosshairs.Count == 0)
			return null;

		var crosshair = collection.Crosshairs.ElementAtOrDefault(crosshairIndex);
		if (crosshair == null || !crosshair.Texture)
			return null;

		Texture2D texture = crosshair.Texture;
		
		bool shouldApplyScaling = scaleEnabled && !Mathf.Approximately(scale, 1f);
    
		if (shouldApplyScaling && !Config.UseWindows.Value && !Config.HideCrosshair.Value)
		{
			texture = CrosshairCache.GetOrAddScaled(collectionIndex, crosshairIndex, crosshair.Texture, scale);
			if (!texture)
			{
				Plugin.Log.LogWarning("Failed to get or add scaled texture, falling back to unscaled");
				texture = crosshair.Texture; // Fallback to original texture
			}
		}

		Vector2 hotspot = HotspotUtils.GetHotspot(Config.HotspotIndex.Value, texture);

		return new CursorData
		{
			CursorType = type,
			Texture = texture,
			Hotspot = hotspot
		};
	}

	private static void RestoreOriginal(CursorType type)
	{
		var original = CrosshairCache.RestoreOriginal(type);

		if (original != null)
		{
			ApplyCursor(original, CursorMode.Auto);
		}
	}

	private static void ApplyCursor(CursorData cursor, CursorMode? overrideMode = null)
	{
		if (cursor == null || cursor.Texture == null)
			return;

		var finalTex = cursor.Texture;
		var type = cursor.CursorType;
		
		var mode = overrideMode ?? (Config.ScaleEnabled.Value ? CursorMode.ForceSoftware : CursorMode.Auto);
		
		if (Config.ModEnabled.Value)
		{
			finalTex = type.Equals(CursorType.Menu_Normal) && Config.ChangeInMenus.Value && Config.UseWindows.Value ? null : finalTex;
			finalTex = type.Equals(CursorType.Game_Normal) && Config.UseWindows.Value ? null : finalTex;
		}
		
		if (Config.ScaleEnabled.Value && mode != CursorMode.ForceSoftware)
		{
			mode = CursorMode.ForceSoftware;
		}

		Cursor.SetCursor(finalTex, cursor.Hotspot, mode);

		if (Config.ModEnabled.Value && Config.HideCrosshair.Value && !isMenuContext)
		{
			Cursor.visible = false;
		}
	}
}