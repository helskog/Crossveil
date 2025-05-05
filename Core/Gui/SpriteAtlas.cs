using Crossveil.Utils;
using Crossveil.Core.Models;

using TMPro;
using System.Linq;

using UnityEngine;
using UnityEngine.TextCore;

namespace Crossveil.Core.UI;

public sealed class SpriteAtlas
{
	private TMP_SpriteAsset _atlas;
	public TMP_SpriteAsset Atlas => _atlas;

	public SpriteAtlas()
	{
		// Clone and initialize a fresh sprite asset
		var defaultAsset = TMP_Settings.defaultSpriteAsset;
		_atlas = Object.Instantiate(defaultAsset);
		_atlas.name = "CrosshairPreviews";
		_atlas.hideFlags |= HideFlags.DontUnloadUnusedAsset;

		// Clear any existing data
		_atlas.spriteInfoList.Clear();
		_atlas.spriteGlyphTable.Clear();
		_atlas.spriteCharacterTable.Clear();
		_atlas.spriteCharacterLookupTable.Clear();
		_atlas.fallbackSpriteAssets.Clear();

		BuildSpriteAsset();
	}

	private void BuildSpriteAsset()
	{
		// Get textures and their collection names
		var entries = Plugin.Collections.GetList()
						.Where(col => col != null)
						.SelectMany(col => (col.Crosshairs ?? Enumerable.Empty<CustomCrosshair>())
						.Where(ch => ch?.Texture != null)
						.Select(ch => new { Collection = col.Name, ch.Texture }))
						.ToArray();

		if (entries.Length == 0)
			return;

		// Pack textures into an atlas
		var atlasTex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
		Rect[] rects = atlasTex.PackTextures(entries.Select(e => e.Texture).ToArray(), 2, 2048);
		atlasTex.Apply();

		// Assign atlas to TMP asset
		_atlas.spriteSheet = atlasTex;
		var mat = new Material(Shader.Find("TextMeshPro/Sprite"))
		{
			mainTexture = atlasTex
		};

		_atlas.material = mat;
		_atlas.materialHashCode = mat.GetInstanceID();

		// Private Use Area
		uint baseUnicode = 0xE000;

		// Populate all tables
		for (int i = 0; i < entries.Length; i++)
		{
			var entry = entries[i];
			var tex = entry.Texture;
			var uv = rects[i];

			// Calculate pixel rect
			var pixelRectVar = new Rect(
					uv.x * atlasTex.width,
					uv.y * atlasTex.height,
					uv.width * atlasTex.width,
					uv.height * atlasTex.height
			);

			var glyphRectVar = new GlyphRect(
					(int)pixelRectVar.x,
					(int)pixelRectVar.y,
					(int)pixelRectVar.width,
					(int)pixelRectVar.height
			);

			// Create Unity Sprite
			var sprite = atlasTex.ToSprite(pixelRectVar);
			sprite.name = $"{entry.Collection}_{tex.name}";

			var glyph = new TMP_SpriteGlyph
			{
				index = (uint)i,
				glyphRect = glyphRectVar,
				scale = 1.0f,
				sprite = sprite
			};

			glyph.metrics = new GlyphMetrics(
							(int)pixelRectVar.width,
							(int)pixelRectVar.height,
							0,
							(int)pixelRectVar.height * 0.8f, // Looks like this is vertical placement
							(int)pixelRectVar.width
			);

			_atlas.spriteGlyphTable.Add(glyph);

			var character = new TMP_SpriteCharacter((uint)(baseUnicode + i), glyph)
			{
				name = sprite.name,
				scale = 1f
			};

			_atlas.spriteCharacterTable.Add(character);

			_atlas.spriteInfoList.Add(new TMP_Sprite
			{
				name = sprite.name,
				unicode = (int)(baseUnicode + i),
				hashCode = TMP_TextUtilities.GetSimpleHashCode(sprite.name),
				pivot = sprite.pivot,
				sprite = sprite
			});
		}

		// Rebuild lookup tables and sort
		_atlas.UpdateLookupTables();
		_atlas.SortGlyphTable();
		_atlas.SortCharacterTable();

		Plugin.Log.LogInfo($"Built sprite atlas with {entries.Length} imported textures");
		for (int i = 0; i < _atlas.spriteInfoList.Count; i++)
		{
			var info = _atlas.spriteInfoList[i];
			Plugin.Log.LogInfo($"[{i}] name=\"{info.name}\"  unicode=0x{info.unicode:X}  hash={info.hashCode}");
		}
	}
}