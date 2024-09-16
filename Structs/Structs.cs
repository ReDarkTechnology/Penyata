using System;
using Penyata.Root;
using SDL2;

namespace Penyata
{
	public class Renderer
	{
		public IntPtr renderer;
		public SDL.SDL_RendererInfo info;
		public int index = -1;
		public RendererMode mode = RendererMode.Accelerated;
		
		public Vector2 viewportOffset;
		public float viewportRotation;
		public float viewportSize = 1;
		public Color backgroundColor = Color.black;
		
		public Action OnDraw;
		
		public void Initialize (IntPtr handle)
		{
			renderer = SDL.SDL_CreateRenderer(handle, index, mode.ToSDLFlag());
			if (renderer == IntPtr.Zero) Console.WriteLine("SDL can't create a valid renderer");
			else SDL.SDL_GetRendererInfo(renderer, out info);
		}
		public void Render()
		{
			SDL.SDL_RenderClear(renderer);
			
			Color reverse = backgroundColor.Reverse();
			SDL.SDL_SetRenderDrawColor(renderer, reverse.byteR, reverse.byteG, reverse.byteB, reverse.byteA);
			OnDraw.TryInvoke();
			SDL.SDL_SetRenderDrawColor(renderer, 
			                           backgroundColor.byteR, 
			                           backgroundColor.byteG, 
			                           backgroundColor.byteB, 
			                           backgroundColor.byteA);
			SDL.SDL_RenderPresent(renderer);
		}
	}
	
	public class DrawRequest
	{
		public int layer;
		public Rect original;
		public Action<Renderer, Rect> OnDraw;
		
		public void Draw(Renderer rend, Rect translated)
		{
			if(OnDraw != null) OnDraw.Invoke(rend, translated);
		}
	}
}
