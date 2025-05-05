using UnityEngine;

namespace Crossveil.Utils;

public static class HotspotUtils
{
	/// <summary>
	/// Picks the hotspot coordinate based on texture size / rendering mode.
	/// </summary>

	public static Vector2 GetHotspot(int index, Texture2D tex)
	{
		float w = tex.width;
		float h = tex.height;

		return index switch
		{
			0 => new Vector2(w * 0.5f, h * 0.5f),
			1 => Vector2.zero,
			2 => new Vector2(w - 1f, 0f),
			3 => new Vector2(0f, h - 1f),
			4 => new Vector2(w - 1f, h - 1f),
			_ => new Vector2(w * 0.5f, h * 0.5f)
		};
	}
}