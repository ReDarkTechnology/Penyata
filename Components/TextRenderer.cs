using System;
using System.IO;
using SDL2;

namespace Penyata
{
	public class TextRenderer : Component
	{
		public static bool isInit;
		public string text = "Text";
		public IntPtr font;
		public Color color = Color.white;
		
		public void LoadFont(string path, int size = 12)
		{
			font = SDL_ttf.TTF_OpenFont(path, size);
		}
		/*public override void OnDraw()
		{
			if(!isInit) 
			{
				SDL_ttf.TTF_Init();
				isInit = true;
			}
			if(font != IntPtr.Zero)
			{
				IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(font, text, color);
				Rect r = transform.drawRect;
				SDL.SDL_Point p = r.c;
				var t = transform.drawRect;
				var d = default(SDL.SDL_Rect);
				
				// now you can convert it into a texture
				IntPtr Message = SDL.SDL_CreateTextureFromSurface(Main.renderer, surfaceMessage);
				SDL.SDL_RenderCopyEx(Main.renderer, Message, ref d, ref t, transform.rotation, ref p, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
				
				// Don't forget to free your surface and texture
				SDL.SDL_FreeSurface(surfaceMessage);
				SDL.SDL_DestroyTexture(Message);
			}
		}*/
	}
}
