using System;
using System.IO;
using Crossveil.Core;
using UnityEngine;

namespace Crossveil.Utils;

public static class TextureUtils
{
	/// <summary>
	///  Load textures from file path
	/// </summary>
	public static Texture2D LoadFromFile(string path, bool linear = true)
	{
		return LoadAndLinearise(File.ReadAllBytes(path), linear);
	}

	/// <summary>
	///  Load default textures bundled within the .dll file
	/// </summary>
	public static Texture2D FromByteArray(this byte[] bytes)
	{
		return LoadAndLinearise(bytes, true);
	}

	/// <summary>
	///  Extension for converting texture to sprite
	/// </summary>
	public static Sprite ToSprite(this Texture2D tex, Rect pixelrect)
	{
		return Sprite.Create(
			tex,
			pixelrect,
			new Vector2(0.5f, 0.5f),
			100f
		);
	}

	/// <summary>
	///  Extension for making scaled copy of Texture2d
	/// </summary>
	public static Texture2D ScaledCopy(this Texture2D src, float factor)
	{
		try
		{
			if (!src)
			{
				Plugin.Log.LogError("[ScaledCopy] Source texture is null.");
				return null;
			}

			var w = Mathf.Max(1, Mathf.RoundToInt(src.width * factor));
			var h = Mathf.Max(1, Mathf.RoundToInt(src.height * factor));

			var rt = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			if (!rt)
			{
				Plugin.Log.LogError("[ScaledCopy] Failed to get temporary RenderTexture.");
				return null;
			}

			Graphics.Blit(src, rt);

			var prev = RenderTexture.active;
			RenderTexture.active = rt;

			var dst = new Texture2D(w, h, TextureFormat.RGBA32, false, true);
			dst.ReadPixels(new Rect(0, 0, w, h), 0, 0);
			dst.Apply();

			RenderTexture.active = prev;
			RenderTexture.ReleaseTemporary(rt);

			dst.name = $"{src.name}_scaled";
			dst.filterMode = src.filterMode;
			dst.anisoLevel = src.anisoLevel;
			dst.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			dst.Apply(false, false);
			return dst;
		}
		catch (Exception ex)
		{
			Plugin.Log.LogError($"[ScaledCopy] Exception during scaling: {ex}");
			return null;
		}
	}

	/// <summary>
	///  Enforcing requirements for all cursors, to avoid mismatch in coloration when switching between rendering modes.
	/// </summary>
	private static Texture2D LoadAndLinearise(byte[] data, bool linear)
	{
		var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false, linear);

		tex.LoadImage(data);

		if (linear) AdjustBrightnessInPlace(tex, 1.25f);

		return tex;
	}

	/// <summary>
	///  Temporary to brighten up the imported textures
	/// </summary>
	private static void AdjustBrightnessInPlace(Texture2D tex, float factor)
	{
		Color[] pixels = tex.GetPixels();
		for (var i = 0; i < pixels.Length; ++i)
		{
			var c = pixels[i];
			c.r = Mathf.Clamp01(c.r * factor);
			c.g = Mathf.Clamp01(c.g * factor);
			c.b = Mathf.Clamp01(c.b * factor);
			pixels[i] = c;
		}

		tex.SetPixels(pixels);
		tex.Apply();
	}
}