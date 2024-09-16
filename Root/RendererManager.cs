using System;
using System.Collections.Generic;
using SDL2;

namespace Penyata.Root
{
	public static class RendererManager
	{
		private static Renderer _rend;
		public static Renderer mainRenderer
		{
			get
			{
				return _rend ?? FindLowestRenderer();
			}
		}
		public static Renderer FindLowestRenderer()
		{
			try
			{
				_rend = renderers[0];
				return renderers[0];
			}
			catch
			{
				return null;
			}
		}
		public static List<Renderer> renderers = new List<Renderer>();
		
		public static void InitializeMain(IntPtr handle)
		{
			var r = new Renderer();
			r.Initialize(handle);
			renderers.Add(r);
		}
		
		public static void RenderAll()
		{
			foreach(var a in renderers)
			{
				a.Render();
			}
		}
	}
}
