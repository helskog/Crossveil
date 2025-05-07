using Crossveil.Crosshair;

using ProjectM;
using ProjectM.UI;

using HarmonyLib;
using Crossveil.Core;

namespace Crossveil.Patches;

[HarmonyPatch(typeof(SetCursorSystem), nameof(SetCursorSystem.OnUpdate))]
internal static class Patch_SetCursorSystem_OnUpdate
{
	private static bool _originalsCached;

	private static void Postfix(SetCursorSystem __instance)
	{
		var data = CursorController._CursorDatas;

		if (!_originalsCached)
		{
			CrosshairCache.CacheOriginals(data);
			_originalsCached = true;
		}

		CrosshairManager.ApplyCurrent();
	}
}