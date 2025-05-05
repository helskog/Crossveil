using CrosshairChanger.Core;
using CrosshairChanger.Utils;

using ProjectM;
using UnityEngine;
using System.Collections.Generic;

namespace CrosshairChanger.Crosshair;

public static class CrosshairCache
{
	private static readonly Dictionary<(int collectionIndex, int crosshairIndex, float scale), Texture2D> _scaledCrosshairs = new();
	public static readonly Dictionary<CursorType, CursorData> OriginalCrosshairs = [];

	public static void ClearScaled() => _scaledCrosshairs.Clear();

	public static void ClearOriginal() => OriginalCrosshairs.Clear();

	public static Texture2D GetOrAddScaled(int collectionIndex, int crosshairIndex, Texture2D tex, float scale)
	{
		var key = (collectionIndex, crosshairIndex, scale);

		if (_scaledCrosshairs.TryGetValue(key, out var cached))
			return cached;

		var scaled = tex.ScaledCopy(scale);

		if (scaled == null)
		{
			Plugin.Log.LogError($"[Scaling] Failed to scale crosshair at {collectionIndex}:{crosshairIndex} with factor {scale}");
			return null;
		}

		_scaledCrosshairs[key] = scaled;
		return scaled;
	}

	/// <summary>
	/// Cache original CursorData
	/// </summary>

	public static void CacheOriginals(CursorData[] dataArray)
	{
		foreach (var cd in dataArray)
		{
			if (!OriginalCrosshairs.ContainsKey(cd.CursorType))
			{
				var copy = new CursorData
				{
					CursorType = cd.CursorType,
					Hotspot = cd.Hotspot,
					Texture = cd.Texture
				};

				OriginalCrosshairs[cd.CursorType] = copy;
			}
		}
		Plugin.Log.LogInfo("Original crosshairs cached.");
	}

	/// <summary>
	/// Retrieve cached CursorData for original crosshairs.
	/// </summary>

	public static CursorData RestoreOriginal(CursorType type)
	{
		if (OriginalCrosshairs.TryGetValue(type, out var original))
		{
			return original;
		}

		Plugin.Log.LogWarning($"Cache not found for CursorType: {type}");
		return null;
	}
}