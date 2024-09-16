using System;
using SDL2;
using Newtonsoft.Json;

namespace Penyata
{
	public class SpriteRenderer : Component
	{
		private string _path;
		public string stringPath
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
				LoadTextureFromFile(_path);
			}
		}
		[JsonIgnore]
		public IntPtr texture;
		[JsonIgnore]
		public Vector2 textureScale;
		public Color color = Color.white;
		public override void OnDraw()
		{
			Request();
		}
		public DrawRequest Request()
		{
			var r = new DrawRequest();
			r.layer = gameObject.layer;
			r.original = transform.rect;
			r.OnDraw += (rn, ts) =>
			{
				if (texture != default(IntPtr)) {
			    	SDL.SDL_SetRenderDrawColor(rn.renderer, color.byteR, color.byteG, color.byteB, color.byteA);
					var rect = new SDL.SDL_Rect() {
						x = Convert.ToInt32(ts.x),
						y = Convert.ToInt32(ts.y),
						w = Convert.ToInt32(ts.w), //* (int)textureScale.x),
						h = Convert.ToInt32(ts.h) //* (int)textureScale.y)
					};
					SDL.SDL_Rect srcRect = rect;
					var point = new SDL.SDL_Point();
					point.x = rect.x + (rect.w / 2);
					point.y = rect.y + (rect.h / 2);
					SDL.SDL_RenderCopyEx(rn.renderer, texture, ref srcRect, ref rect, transform.rotation, ref point, SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL);
				}
			};
			Drawer.requests.Add(r);
			return r;
		}
		public void LoadTextureFromFile(string file)
		{
			stringPath = file;
			texture = SDL_image.IMG_LoadTexture(Root.RendererManager.mainRenderer.renderer, file);
			uint format = default(uint);
			int access = 0;
			int w = 0;
			int h = 0;
			SDL.SDL_QueryTexture(texture, out format, out access, out w, out h);
			textureScale = new Vector2(w, h);
		}
	}
}
