using System.Linq;
using ProjectM.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Crossveil.Core.Gui.Components;

/// <summary>
///  Credits to mitch (mfoltz) for his divider implementation in RetroCamera:
///  https://github.com/mfoltz/RetroCamera/blob/main/Patches/OptionsMenuPatches.cs#L239
/// </summary>
internal class CustomDivider
{
	private string _label;
	private OptionsPanel_Interface _panel;

	public CustomDivider Panel(OptionsPanel_Interface panel)
	{
		_panel = panel;
		return this;
	}

	public CustomDivider Label(string label)
	{
		_label = label;
		return this;
	}

	public GameObject Build()
	{
		GameObject dividerGameObject = new("Divider");

		var dividerTransform = dividerGameObject.AddComponent<RectTransform>();
		dividerTransform.SetParent(_panel.ContentNode);
		dividerTransform.localScale = Vector3.one;
		dividerTransform.sizeDelta = new Vector2(0f, 28f);

		var dividerLayout = dividerGameObject.AddComponent<LayoutElement>();
		dividerLayout.preferredHeight = 75f;

		GameObject dividerTextGameObject = new("Text");
		var dividerTextTransform = dividerTextGameObject.AddComponent<RectTransform>();
		dividerTextTransform.SetParent(dividerGameObject.transform);
		dividerTextTransform.localScale = Vector3.one;

		var textMeshDivider = dividerTextGameObject.AddComponent<TextMeshProUGUI>();
		var textMeshArray = _panel.ContentNode.GetComponentsInChildren<TextMeshProUGUI>();
		var fontAsset = textMeshArray.First().font;

		textMeshDivider.alignment = TextAlignmentOptions.Center;
		textMeshDivider.fontStyle = FontStyles.SmallCaps;
		textMeshDivider.font = fontAsset;
		textMeshDivider.fontSize = 20f;
		textMeshDivider.color = Color.gray;
		textMeshDivider.SetText(_label);

		dividerGameObject.SetActive(true);
		return dividerGameObject;
	}
}