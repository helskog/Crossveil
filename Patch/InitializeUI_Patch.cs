using Crossveil.Core;
using Crossveil.Crosshair;
using HarmonyLib;
using ProjectM.UI;

namespace Crossveil.Patch;

[HarmonyPatch]
internal static class InitializeUIPatch
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(InitializeUI), nameof(InitializeUI.Start))]
	private static void initialize_ui_patch()
	{
		Plugin.ShouldCollectCache = true;
		CrosshairRuntime.Refresh();
	}
}