using UnityEngine;

namespace CrosshairChanger.Core.Models;

public class CustomCrosshair
{
	public Texture2D Texture { get; }
	public int Width { get; }
	public int Height { get; }
	public string Name { get; }

	public CustomCrosshair(Texture2D texture)
	{
		Texture = texture;
		Name = texture.name;
		Width = texture.width;
		Height = texture.height;
	}
}