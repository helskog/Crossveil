using System;
using Il2CppSystem.Collections.Generic;

using TMPro;
using ProjectM.UI;

using Stunlock.Core;
using Stunlock.Localization;
using UnityEngine;

namespace CrosshairChanger.Core.UI.Components;

internal class CustomDropdown
{
	private OptionsPanel_Interface _panel;
	private DropdownType _type;
	private string _label;
	private string _tooltip;
	private List<string> _options = new();
	private int _defaultValue;
	private int _initialValue;
	private Action<int> _onValueChanged;

	public enum DropdownType
	{
		COLLECTION,
		CROSSHAIR,
		HOTSPOT
	}

	public CustomDropdown Panel(OptionsPanel_Interface panel)
	{
		_panel = panel;
		return this;
	}

	public CustomDropdown Type(DropdownType type)
	{
		_type = type;
		return this;
	}

	public CustomDropdown Label(string label)
	{
		_label = label;
		return this;
	}

	public CustomDropdown Tooltip(string tooltip)
	{
		_tooltip = tooltip;
		return this;
	}

	public CustomDropdown DefaultValue(int defaultValue)
	{
		_defaultValue = defaultValue;
		return this;
	}

	public CustomDropdown InitialValue(int initialValue)
	{
		_initialValue = initialValue;
		return this;
	}

	public CustomDropdown OnValueChanged(Action<int> callback)
	{
		_onValueChanged = callback;
		return this;
	}

	public SettingsEntry_Dropdown Build()
	{
		var headerGuid = AssetGuid.NewGuid();
		var headerKey = new LocalizationKey(headerGuid);

		var tooltipGuid = AssetGuid.NewGuid();
		var tooltipKey = new LocalizationKey(tooltipGuid);

		if (_label != null)
			Localization.LocalizedStrings.Add(headerGuid, _label);

		if (_tooltip != null)
			Localization.LocalizedStrings.Add(tooltipGuid, _tooltip);

		PopulateOptions(_type);

		var entry = OptionsHelper.AddDropdown(
				_panel.ContentNode,
				_panel.DropdownPrefab,
				isGamepad: false,
				headerKey,
				tooltipKey,
				_options,
				_defaultValue,
				_initialValue,
				_onValueChanged
		);

		var tmp_DropdownChildComponent = entry.GetComponentInChildren<TMP_Dropdown>(true);
		if (tmp_DropdownChildComponent == null) return null;

		var dropdownRect = tmp_DropdownChildComponent.GetComponent<RectTransform>();
		dropdownRect.sizeDelta = new Vector2(-300f, dropdownRect.sizeDelta.y);

		tmp_DropdownChildComponent.captionText.richText = true;
		tmp_DropdownChildComponent.captionText.spriteAsset = Plugin.SpriteAsset.Atlas;
		tmp_DropdownChildComponent.captionText.alignment = TextAlignmentOptions.Left;

		tmp_DropdownChildComponent.itemText.richText = true;
		tmp_DropdownChildComponent.itemText.spriteAsset = Plugin.SpriteAsset.Atlas;
		tmp_DropdownChildComponent.itemText.alignment = TextAlignmentOptions.Center;

		// Make reactive for ToolTips to render properly
		_panel.EntriesSelectionGroup.Entries.Add(entry);

		return entry;
	}

	private void PopulateOptions(DropdownType type)
	{
		var collections = Plugin.Collections.GetList();
		var crosshairOptions = Plugin.Collections.GetCrosshairOptions(Config.CollectionIndex.Value);

		switch (type)
		{
			case DropdownType.COLLECTION:

				foreach (var collection in collections)
				{
					_options.Add(collection.Name);
				}

				break;

			case DropdownType.CROSSHAIR:

				foreach (var crosshair in crosshairOptions)
				{
					_options.Add(crosshair);
				}

				break;

			case DropdownType.HOTSPOT:
				_options.Add("Center");
				_options.Add("Top Left");
				_options.Add("Top Right");
				_options.Add("Bottom Left");
				_options.Add("Bottom Right");
				break;
		}
	}
}