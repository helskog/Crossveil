using System.Linq;
using System.Collections.Generic;

namespace Crossveil.Core.Models;

public class CustomCollection
{
	public string Name { get; }
	public List<CustomCrosshair> Crosshairs { get; }

	public CustomCollection(string name, List<CustomCrosshair> items)
	{
		Name = name;
		Crosshairs = items;
	}
}