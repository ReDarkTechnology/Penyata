using System;
using SDL2;

namespace Penyata
{
	public class BoxRenderer : Component
	{
		public Color color = Color.white;
		public bool fill;
		public override void OnDraw()
		{
			Drawer.DrawSquare(transform, fill, color);
		}
	}
}
