using Crossveil.Core;

using HarmonyLib;

using ProjectM.UI;

namespace Crossveil.Patch;

// Credit where its due to original ModernCamera mod for the patches.

[HarmonyPatch]
internal static class HUDMenu_Patch
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(HUDMenu), nameof(HUDMenu.OnEnable))]
	private static void OnEnable()
	{
		Plugin.inMenuState = true;
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(HUDMenu), nameof(HUDMenu.OnDisable))]
	private static void OnDisable()
	{
		Plugin.inMenuState = false;
	}
}