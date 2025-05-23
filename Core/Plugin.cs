﻿using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Crossveil.Core.Gui;
using Crossveil.Crosshair.Collections;
using HarmonyLib;

namespace Crossveil.Core;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
	private static Harmony _harmony;

	public static ManualLogSource Log { get; private set; }

	public static CollectionRegistry Collections { get; set; }
	public static SpriteAtlas SpriteAsset { get; set; }

	public static bool InMenuState { get; set; }
	public static bool ShouldCollectCache { get; set; }

	public override void Load()
	{
		Log = base.Log;

		ShouldCollectCache = false;

		// Initialize configuration file
		Core.Config.Initialize(Config);

		// Initialize collection registry.
		Collections = new CollectionRegistry();

		// Import crosshair collections.
		CollectionImport.Standard();
		CollectionImport.Custom();

		// Initialize and build SpriteAsset from imported crosshairs (for UI preview).
		SpriteAsset = new SpriteAtlas();

		_harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
		_harmony.PatchAll(Assembly.GetExecutingAssembly());

		Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
	}

	public override bool Unload()
	{
		_harmony?.UnpatchSelf();
		return true;
	}
}