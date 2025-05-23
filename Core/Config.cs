﻿using BepInEx.Configuration;

namespace Crossveil.Core;

public class Config
{
	internal static ConfigEntry<bool> ModEnabled;
	internal static ConfigEntry<bool> HideCrosshair;
	internal static ConfigEntry<bool> UseWindows;
	internal static ConfigEntry<bool> ChangeInMenus;
	internal static ConfigEntry<int> CollectionIndex;
	internal static ConfigEntry<int> CrosshairIndex;
	internal static ConfigEntry<int> HotspotIndex;
	internal static ConfigEntry<bool> ScaleEnabled;
	internal static ConfigEntry<bool> ScaleInMenus;
	internal static ConfigEntry<float> ScaleFactor;

	internal static void Initialize(ConfigFile config)
	{
		config.SaveOnConfigSet = true;

		ModEnabled = config.Bind("Options", "EnableCustomCrosshair", false, "Selected option to enable the mod.");
		HideCrosshair = config.Bind("Options", "HideCrosshair", false, "Hides the game crosshair.");
		UseWindows = config.Bind("Options", "UseWindows", false, "Uses the windows cursor");
		ChangeInMenus = config.Bind("Options", "ChangeInMenus", false, "Selected option to change menu crosshairs.");
		ScaleEnabled = config.Bind("Options", "ScaleEnabled", false, "Selected option to change menu crosshairs.");
		ScaleInMenus = config.Bind("Options", "ScaleInMenus", false, "Scale crosshair in menus.");

		CollectionIndex = config.Bind("Variables", "CrosshairCollection", 0, "Selected collection.");
		CrosshairIndex = config.Bind("Variables", "CrosshairStyle", 0, "Selected crosshair.");
		HotspotIndex = config.Bind("Variables", "HotspotIndex", 0, "Selected hotspot.");
		ScaleFactor = config.Bind("Variables", "ScaleFactor", 1.0f, "Scale factor");

		Plugin.Log.LogInfo($"Initialized configuration file");
	}
}