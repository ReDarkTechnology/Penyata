using System;
using SDL2;

namespace Penyata.Root
{
	public static class HandleManager
	{
		public static Handle mainHandle;
		public static PlaymodeState playmodeState = PlaymodeState.Playing;
		
		public static Window CreateWindow ()
		{
			var w = new Window();
			w.Initialize();
			mainHandle = w;
			return w;
		}
		public static Handle CreateFromHandle (IntPtr handle)
		{
			var w = new Handle();
			w.handle = handle;
			mainHandle = w;
			return w;
		}
	}
	public class Handle 
	{
		public IntPtr handle;
	}
	public class Window : Handle
	{
		private string _title = "New Window";
		public string title
		{
			get
			{
				return _title;
			}
			set
			{
				if(handle != IntPtr.Zero)
				{
					SDL.SDL_SetWindowTitle(handle, value);
				}
				_title = value;
			}
		}
		public WindowMode mode = WindowMode.Shown;
		private Vector2 p_winPos = new Vector2(128, 128); 
		public Vector2 position 
		{
			get {
				if (handle == IntPtr.Zero)
					return p_winPos;
				else {
					int x, y;
					SDL.SDL_GetWindowPosition(handle, out x, out y);
					return new Vector2(x, y);
				}
			}
			set
			{
				if(handle == IntPtr.Zero)
					p_winPos = value;
				else
					SDL.SDL_SetWindowPosition(handle, value.x.ToInt(), value.y.ToInt());
			}
		}
		private Vector2 p_winSize = new Vector2(640, 360);
		public Vector2 size {
			get
			{
				if(handle == IntPtr.Zero)
					return p_winSize;
				else
				{	
					int w, h;
					SDL.SDL_GetWindowSize(handle, out w, out h);
					return new Vector2(w, h);
				}
			}
			set
			{
				if(handle == IntPtr.Zero)
					p_winSize = value;
				else
					SDL.SDL_SetWindowSize(handle, value.x.ToInt(), value.y.ToInt());
			}
		}
		
		public void Initialize()
		{
			handle = SDL.SDL_CreateWindow(title, position.x.ToInt(), position.y.ToInt(), size.x.ToInt(), size.y.ToInt(), mode.ToSDLFlag());
			if (handle == IntPtr.Zero) Console.WriteLine("SDL can't create a window");
		}
	}
}
