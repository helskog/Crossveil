using Crossveil.Core;
using Crossveil.Crosshair;
using HarmonyLib;

using ProjectM.UI;

namespace Crossveil.Patch;

// Credit where its due to original ModernCamera mod for the patches.

[HarmonyPatch]
internal static class InitializeUI_Patch
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(InitializeUI), nameof(InitializeUI.Start))]
	private static void initialize_ui_patch()
	{
		Plugin.ShouldCollectCache = true;
		
		if (Config.ModEnabled.Value && Config.ScaleEnabled.Value)
		{
			Crosshair.CrosshairManager.ApplyCurrent();
		}
	}
}