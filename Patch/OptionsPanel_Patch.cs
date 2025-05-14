using Crossveil.Core;
using Crossveil.Core.Gui.Components;
using Crossveil.Crosshair;
using HarmonyLib;
using ProjectM.UI;
using TMPro;

namespace Crossveil.Patch;

[HarmonyPatch(typeof(OptionsPanel_Interface), nameof(OptionsPanel_Interface.Start))]
internal static class PatchOptionsPanelInterface
{
	private static SettingsEntry_Checkbox _hideCrosshairToggle;
	private static SettingsEntry_Checkbox _useWindowsCursorToggle;
	private static SettingsEntry_Checkbox _changeInMenuToggle;
	private static SettingsEntry_Dropdown _crosshairDropdown;
	private static SettingsEntry_Dropdown _collectionDropdown;
	private static SettingsEntry_Dropdown _hotspotDropdown;
	private static SettingsEntry_Checkbox _enableScalingToggle;
	private static SettingsEntry_Checkbox _enableScalingInMenus;
	private static SettingsEntry_Slider _changeScalingSlider;

	[HarmonyPostfix]
	private static void Postfix(OptionsPanel_Interface __instance)
	{
		var mainHeader = new CustomHeader()
			.Panel(__instance)
			.Label("Crossveil")
			.Build();

		var enableModToggle = new CustomToggle()
			.Panel(__instance)
			.Label("Mod Enabled")
			.Tooltip("<align=\"center\">Enable or disable the mod.</align>")
			.DefaultValue(false)
			.InitialValue(Config.ModEnabled.Value)
			.OnValueChanged(OnModEnabledToggle)
			.Build();

		new CustomDivider().Panel(__instance).Label("General Options").Build();

		_hideCrosshairToggle = new CustomToggle()
			.Panel(__instance)
			.Label("Hide Crosshair")
			.Tooltip("<align=\"center\">Hide the game crosshair except inside menus.</align>")
			.DefaultValue(false)
			.InitialValue(Config.HideCrosshair.Value)
			.OnValueChanged(OnHideCrosshairToggle)
			.Build();

		_useWindowsCursorToggle = new CustomToggle()
			.Panel(__instance)
			.Label("Use Windows Cursor")
			.Tooltip("<align=\"center\">Uses your system crosshair as the default crosshair.</align>")
			.DefaultValue(false)
			.InitialValue(Config.UseWindows.Value)
			.OnValueChanged(OnUseWindowsCursor)
			.Build();

		_changeInMenuToggle = new CustomToggle()
			.Panel(__instance)
			.Label("Change In Menus")
			.Tooltip("<align=\"center\">Show the crosshair inside game menus.</align>")
			.DefaultValue(false)
			.InitialValue(Config.ChangeInMenus.Value)
			.OnValueChanged(OnChangeInMenusToggle)
			.Build();

		new CustomDivider().Panel(__instance).Label("Crosshair Options").Build();

		_collectionDropdown = new CustomDropdown()
			.Panel(__instance)
			.Type(CustomDropdown.DropdownType.COLLECTION)
			.Label("Collection")
			.Tooltip("<align=\"center\">Select a crosshair collection.\n\n\n" +
			         "<color=#ecdd1f><b>MANAGE COLLECTIONS</b></color>\n\n" +
			         "<size=75%>Go to the game install folder then open this path:\n" +
			         "<b><color=#e6e6e6>BepInEx\\config\\Crossveil</color></b>\n\n" +
			         "Each folder inside the directory containing <color=#FF0000>at least one <b>.png</b> image</color> " +
			         "will be imported as a unique collection at startup.\n\n" +
			         "For the best results use evenly sized images like <b>32x32</b>, <b>64x64</b> or <b>128x128</b> for example.</size></align>")
			.InitialValue(Config.CollectionIndex.Value)
			.DefaultValue(0)
			.OnValueChanged(OnCollectionChanged)
			.Build();

		_crosshairDropdown = new CustomDropdown()
			.Panel(__instance)
			.Type(CustomDropdown.DropdownType.CROSSHAIR)
			.Label("Crosshair")
			.Tooltip("<align=\"center\">Select the crosshair you would like to use.</size>")
			.InitialValue(Config.CrosshairIndex.Value)
			.DefaultValue(0)
			.OnValueChanged(OnCrosshairChanged)
			.Build();

		_hotspotDropdown = new CustomDropdown()
			.Panel(__instance)
			.Type(CustomDropdown.DropdownType.HOTSPOT)
			.Label("Hotspot")
			.Tooltip("<align=\"center\">Switch the crosshair hotspot.\n\n\n" +
			         "<color=#ecdd1f><b>INFORMATION</b></color>\n\n" +
			         "<size=75%>The crosshair hotspot defines the exact point used for targeting. " +
			         "For example if set to \"Center\" the clickable area of the loaded crosshair will be in the exact center of the image.\n\n" +
			         "You can try different hotspots for different crosshairs, as some textures such as ones pointing to a specific corner " +
			         "may work better with one of the other modes.</size></align>")
			.InitialValue(Config.HotspotIndex.Value)
			.DefaultValue(0)
			.OnValueChanged(OnHotspotChanged)
			.Build();

		new CustomDivider().Panel(__instance).Label("Scaling Options").Build();

		_enableScalingToggle = new CustomToggle()
			.Panel(__instance)
			.Label("Enable")
			.Tooltip("<align=\"center\">Scale the crosshair size up or down.\n\n\n" +
			         "<color=#FF0000><b>WARNING</b></color>\n\n" +
			         "<size=75%>This setting forces software-rendering for the crosshair. Which may or may not have a negative" +
			         "impact on performance, colouration of the image or hotspot locations.\n\n" +
			         "For best results only use imported images that are equal in dimensions and preferably ones that point to an obvious " +
			         "hotspot area like the middle of the image or the top left corner.</size></align>")
			.InitialValue(Config.ScaleEnabled.Value)
			.DefaultValue(false)
			.OnValueChanged(OnScalingEnabled)
			.Build();

		_enableScalingInMenus = new CustomToggle()
			.Panel(__instance)
			.Label("Scale In Menus")
			.Tooltip("<align=\"center\">Enable or disable crosshair scaling inside game menus.</size>")
			.InitialValue(Config.ScaleInMenus.Value)
			.DefaultValue(false)
			.OnValueChanged(OnScalingInMenus)
			.Build();

		_changeScalingSlider = new CustomSlider()
			.Panel(__instance)
			.Label("Scaling factor")
			.MinValue(0.1f)
			.MaxValue(5.0f)
			.InitialValue(Config.ScaleFactor.Value)
			.DefaultValue(1.0f)
			.OnValueChanged(OnScaleFactorChanged)
			.Build();
	}

	private static void OnModEnabledToggle(bool value)
	{
		Config.ModEnabled.Value = value;
		Plugin.Log.LogInfo($"[UI] Mod enabled changed to: {value}");

		CrosshairRuntime.Refresh();
	}

	private static void OnHideCrosshairToggle(bool value)
	{
		Config.HideCrosshair.Value = value;
		Plugin.Log.LogInfo($"[UI] Hide crosshair changed to: {value}");

		CrosshairRuntime.Refresh();
	}

	private static void OnUseWindowsCursor(bool value)
	{
		Config.UseWindows.Value = value;
		Plugin.Log.LogInfo($"[UI] Use windows crosshair changed to: {value}");

		CrosshairRuntime.Refresh();
	}

	private static void OnChangeInMenusToggle(bool value)
	{
		Config.ChangeInMenus.Value = value;
		Plugin.Log.LogInfo($"[UI] Change in menu changed to: {value}");

		CrosshairRuntime.Refresh();
	}

	private static void OnCollectionChanged(int index)
	{
		Config.CollectionIndex.Value = index;
		Config.CrosshairIndex.Value = 0;
		Plugin.Log.LogInfo($"[UI] Collection index changed to: {index}");

		var dd = _crosshairDropdown.GetComponentInChildren<TMP_Dropdown>(true);
		if (dd != null)
		{
			var crosshairOptions = Plugin.Collections.GetCrosshairOptions(Config.CollectionIndex.Value);
			dd.ClearOptions();
			dd.AddOptions(crosshairOptions);
			dd.RefreshShownValue();
		}

		CrosshairRuntime.Refresh();
	}

	private static void OnCrosshairChanged(int index)
	{
		Config.CrosshairIndex.Value = index;
		Plugin.Log.LogInfo($"[UI] Crosshair index changed to: {index}");

		CrosshairRuntime.Refresh();
	}

	private static void OnHotspotChanged(int index)
	{
		Config.HotspotIndex.Value = index;
		Plugin.Log.LogInfo($"[UI] Hotspot index changed to: {index}");

		CrosshairRuntime.Refresh();
	}

	private static void OnScalingEnabled(bool value)
	{
		Config.ScaleEnabled.Value = value;
		Plugin.Log.LogInfo($"[UI] Enable scaling changed to: {value}");

		CrosshairRuntime.Refresh();
	}

	private static void OnScalingInMenus(bool value)
	{
		Config.ScaleInMenus.Value = value;
		Plugin.Log.LogInfo($"[UI] Scale in menus changed to: {value}");

		CrosshairRuntime.Refresh();
	}

	private static void OnScaleFactorChanged(float value)
	{
		Config.ScaleFactor.Value = value;
		Plugin.Log.LogInfo($"[UI] Scaling factor changed to: {value}");

		CrosshairRuntime.Refresh();
	}
}