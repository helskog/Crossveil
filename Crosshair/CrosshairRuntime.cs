using Crossveil.Core;
using ProjectM;
using ProjectM.UI;
using UnityEngine;

namespace Crossveil.Crosshair;

public static class CrosshairRuntime
{
	public static void Refresh()
	{
		// If no collections are loaded yet
		if (Plugin.Collections == null || Plugin.Collections.GetList().Count == 0) return;

		var isMenuContext = ViewManager.IsInMenu() || Plugin.InMenuState;

		var ctx = new CrosshairContext
		{
			ModEnabled = Config.ModEnabled.Value,
			Hide = Config.HideCrosshair.Value,
			UseWindows = Config.UseWindows.Value,
			ChangeInMenus = Config.ChangeInMenus.Value,
			ScaleEnabled = Config.ScaleEnabled.Value,
			ScaleInMenus = Config.ScaleInMenus.Value,
			ScaleFactor = Config.ScaleFactor.Value,
			RequestedType = isMenuContext ? CursorType.Menu_Normal : CursorType.Game_Normal,
			IsMenuContext = isMenuContext
		};

		var decision = CrosshairRules.Decide(ctx);

		// Clear scaled cache
		if (!ctx.ScaleEnabled || ctx is { IsMenuContext: true, ScaleInMenus: false })
			CrosshairCache.ClearScaled();

		Cursor.SetCursor(decision.Texture, decision.Hotspot, decision.Mode);

		// Only call .visible if hiding is enabled, otherwise crosshair sticks to screen on rotate camera
		if (decision.Visible == false) Cursor.visible = false;
	}
}