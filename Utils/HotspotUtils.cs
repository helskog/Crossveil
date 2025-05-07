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
			0 => new Vector2(w * 0.5f, h * 0.5f),       // Center
			1 => Vector2.zero,                          // Top-left
			2 => new Vector2(w - 1f, 0f),               // Top-right
			3 => new Vector2(0f, h - 1f),               // Bottom-left
			4 => new Vector2(w - 1f, h - 1f),           // Bottom-right
			5 => new Vector2(0f, h * 0.5f),             // Center Left
			6 => new Vector2(w - 1f, h * 0.5f),         // Center Right
			7 => new Vector2(w * 0.5f, 0f),             // Top Center
			8 => new Vector2(w * 0.5f, h - 1f),         // Bottom Center
			_ => new Vector2(w * 0.5f, h * 0.5f)        // Default to Center
		};
	}
}