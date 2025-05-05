using CrossVeil.Core;
using CrossVeil.Utils;

using BepInEx;

using System.IO;
using UnityEngine;
using System.Reflection;

namespace CrossVeil.Crosshair.Collections;

public static class CollectionImport
{
	public static void Standard()
	{
		var asm = Assembly.GetExecutingAssembly();

		foreach (var res in asm.GetManifestResourceNames())
		{
			if (!res.StartsWith("CrossVeil.Crosshairs.") || !res.EndsWith(".png"))
				continue;

			using var stream = asm.GetManifestResourceStream(res);

			if (stream == null)
				continue;

			byte[] bytes;
			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				bytes = ms.ToArray();
			}

			// Create a new Texture2D from byte array
			var tex = TextureUtils.FromByteArray(bytes);

			tex.name = res.Replace("CrossVeil.Crosshairs.", "").Replace(".png", "");
			tex.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			Plugin.Collections.Add(tex, "Standard");
		}
	}

	public static void Custom()
	{
		var root = Path.Combine(Paths.ConfigPath, "CrossVeil");

		if (!Directory.Exists(root))
			Directory.CreateDirectory(root);

		foreach (var dir in Directory.GetDirectories(root))
		{
			var collectionName = Path.GetFileName(dir);

			foreach (var file in Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly))
			{
				var tex = TextureUtils.LoadFromFile(file);
				tex.name = Path.GetFileNameWithoutExtension(file);
				tex.hideFlags |= HideFlags.DontUnloadUnusedAsset;

				Plugin.Collections.Add(tex, collectionName);
			}
		}
	}
}