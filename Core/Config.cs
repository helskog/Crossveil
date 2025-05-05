using BepInEx.Configuration;

namespace Crossveil.Core;

public class Config
{
	internal static ConfigEntry<bool> ModEnabled;
	internal static ConfigEntry<bool> ChangeInMenus;
	internal static ConfigEntry<int> CollectionIndex;
	internal static ConfigEntry<int> CrosshairIndex;
	internal static ConfigEntry<int> HotspotIndex;
	internal static ConfigEntry<bool> ScaleEnabled;
	internal static ConfigEntry<float> ScaleFactor;

	internal static void Initialize(ConfigFile config)
	{
		config.SaveOnConfigSet = true;

		ModEnabled = config.Bind("Options", "EnableCustomCrosshair", false, "Selected option to enable the mod.");
		ChangeInMenus = config.Bind("Options", "ChangeInMenus", false, "Selected option to change menu crosshairs.");
		ScaleEnabled = config.Bind("Options", "ScaleEnabled", false, "Selected option to change menu crosshairs.");

		CollectionIndex = config.Bind("Variables", "CrosshairCollection", 0, "Selected collection.");
		CrosshairIndex = config.Bind("Variables", "CrosshairStyle", 0, "Selected crosshair.");
		HotspotIndex = config.Bind("Variables", "HotspotIndex", 0, "Selected hotspot.");
		ScaleFactor = config.Bind("Variables", "ScaleFactor", 1.0f, "Scale factor");

		Plugin.Log.LogInfo($"Initialized configuration file");
	}
}