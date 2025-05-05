using CrossVeil.Core;
using CrossVeil.Core.Models;

using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace CrossVeil.Crosshair.Collections;

public class CollectionRegistry
{
	private readonly List<CustomCollection> _collections;

	public CollectionRegistry()
	{
		_collections = new List<CustomCollection>();
	}

	public Il2CppSystem.Collections.Generic.List<string> GetCrosshairOptions(int collectionIndex)
	{
		var collections = Plugin.Collections.GetList();
		var selected = collections[collectionIndex];

		var list = new Il2CppSystem.Collections.Generic.List<string>();
		foreach (var crosshair in selected.Crosshairs)
		{
			string fullName = $"{selected.Name}_{crosshair.Name}";
			list.Add($"<sprite name=\"{fullName}\">  {crosshair.Name}");
		}

		return list;
	}

	public List<CustomCollection> GetList()
	{
		return _collections;
	}

	public void Add(Texture2D texture, string collection)
	{
		bool collectionExists = _collections.Any(col => col.Name == collection);

		if (!collectionExists)
		{
			_collections.Add(
					new CustomCollection(collection, new List<CustomCrosshair>())
			);

			Plugin.Log.LogInfo($"Created collection with: name={collection}");
		}

		var collectionEntry = _collections.FirstOrDefault(c => c.Name.Equals(collection, StringComparison.OrdinalIgnoreCase));

		if (collectionEntry != null)
		{
			collectionEntry.Crosshairs.Add(
					new CustomCrosshair(texture)
			);

			Plugin.Log.LogInfo($"Added texture with name={texture.name}.png");
		}
	}
}