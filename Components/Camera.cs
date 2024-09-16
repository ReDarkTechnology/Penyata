using System;
using System.Collections.Generic;
using SDL2;
using Penyata.Root;

namespace Penyata
{
	public class Camera : Component
	{
		public Renderer targetRenderer;
		Renderer rend
		{
			get
			{
				if(targetRenderer == null) return RendererManager.mainRenderer;
				return targetRenderer;
			}
		}
		public List<int> layers = new List<int>(new int[]{-1});
		public float size = 1;
		
		public override void OnPhysics()
		{
			if(rend != null)
			{
				rend.OnDraw += () =>
				{
					foreach(var r in Drawer.requests)
					{
						r.Draw(rend, Translate(r.original));
					}
				};
			}
		}
		
		public Rect Translate(Rect rect)
		{
			var v = rect;
			var s = size;
			var n_rect = new Rect(){
				x = v.x * s + transform.position.x,
				y = ((-v.y + -v.h) * s) + transform.position.y,
				w = v.w * s,
				h = v.h * s
			};
			return n_rect;
		}
	}
}
