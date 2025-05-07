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
		bool isInOtherMenu = Plugin.inMenuState;
		isMenuContext = isInMainMenu || isInOtherMenu;

		if (!Config.ModEnabled.Value)
		{
			if (isMenuContext)
			{
				RestoreOriginal(CursorType.Menu_Normal);
			}
			else
			{
				RestoreOriginal(CursorType.Game_Normal);
			}

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
		if (crosshair == null || crosshair.Texture == null)
			return null;

		Texture2D texture = crosshair.Texture;
		if (scaleEnabled && scale != 1f && !isMenuContext)
		{
			texture = CrosshairCache.GetOrAddScaled(collectionIndex, crosshairIndex, crosshair.Texture, scale);
			if (texture == null)
				return null;
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

		Cursor.SetCursor(finalTex, cursor.Hotspot, mode);

		if (Config.ModEnabled.Value && Config.HideCrosshair.Value && !isMenuContext)
		{
			Cursor.visible = false;
		}
	}
}