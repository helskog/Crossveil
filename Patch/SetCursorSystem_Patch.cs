using Crossveil.Core;
using Crossveil.Crosshair;
using HarmonyLib;
using ProjectM;
using ProjectM.UI;

namespace Crossveil.Patch;

[HarmonyPatch(typeof(SetCursorSystem), nameof(SetCursorSystem.OnUpdate))]
internal static class PatchSetCursorSystemOnUpdate
{
	private static bool _originalsCached;

	private static void Postfix(SetCursorSystem __instance)
	{
		var data = CursorController._CursorDatas;

		if (Plugin.ShouldCollectCache && !_originalsCached)
		{
			CrosshairCache.CacheOriginals(data);
			_originalsCached = true;
		}
	}
}