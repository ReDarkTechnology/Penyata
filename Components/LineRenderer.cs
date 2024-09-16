using System;
using SDL2;

namespace Penyata
{
	public class LineRenderer : Component
	{
		public Color color = Color.white;
		public float x1;
		public float y1;
		public float x2;
		public float y2;
		/*public override void OnDraw()
		{
		    SDL.SDL_SetRenderDrawColor(Main.renderer, color.byteR, color.byteG, color.byteB, color.byteA);
			SDL.SDL_RenderDrawLine(Main.renderer, 
			                       Convert.ToInt32(x1),
			                       Convert.ToInt32(y1),
			                       Convert.ToInt32(x2),
			                       Convert.ToInt32(y2)
			                      );
		}*/
	}
}
