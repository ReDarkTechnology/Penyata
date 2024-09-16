using System;
using System.Collections.Generic;
using SDL2;

namespace Penyata
{
	public class Transform : Component
	{
		public Transform()
		{
			
		}
		public Transform(GameObject obj)
		{
			SetComponentOwner(obj);
		}
		public Action OnTransformChange;
		public Rect rect = new Rect(-0.5f, -0.5f, 1f, 1f);
		/*public SDL.SDL_Rect drawRect
		{
			get
			{
				var v = rect;
				var s = Main.mainCamera.actualSize;
				var n_rect = new SDL.SDL_Rect(){
					x = Convert.ToInt32(v.x * s + Main.aco.x),
					y = Convert.ToInt32(((-v.y + -v.h) * s) + Main.aco.y),
					w = Convert.ToInt32(v.w * s),
					h = Convert.ToInt32(v.h * s)
				};
				return n_rect;
			}
		}*/
		public Vector2 position
		{
			get
			{
				return new Vector2(rect.x + (rect.w/2), rect.y + (rect.h/2));
			}
			set
			{
				var d = value - position;
				rect.x = value.x - (localScale.x / 2);
				rect.y = value.y - (localScale.y / 2);
				if(childs.Count > 0)
				{
					foreach(var child in childs)
					{
						child.position += d;
					}
				}
				if(OnTransformChange != null) OnTransformChange.Invoke();
			}
		}
		public Vector2 localScale
		{
			get
			{
				return new Vector2(rect.w, rect.h);
			}
			set
			{
				var d = value - localScale;
				rect.x = rect.x - d.x / 2;
				rect.y = rect.y - d.y / 2;
				rect.w = value.x;
				rect.h = value.y;
				if(OnTransformChange != null) OnTransformChange.Invoke();
			}
		}
		public Vector2 localPosition
		{
			get
			{
				if(parent != null)
					return position - parent.position;
				else
					return position;
			}
			set
			{
				if(parent != null)
					position = value + parent.position;
				else
					position = value;
				if(OnTransformChange != null) OnTransformChange.Invoke();
			}
		}
		private float p_rotation;
		public float rotation
		{
			get
			{
				return p_rotation;
			}
			set
			{
				p_rotation = value;
				if(OnTransformChange != null) OnTransformChange.Invoke();
			}
		}
		public Vector2 forward
		{
			get
			{
				return Drawer.GetDirection(rotation);
			}
		}
		public Vector2 right
		{
			get
			{
				return Drawer.GetDirection(rotation + 90);
			}
		}
		public Transform parent {get; private set;}
		public List<Transform> childs = new List<Transform>();
		public void SetParent(Transform to)
		{
			if(parent != null) to.childs.Remove(this);
			if(SceneManager.activeScenes.Count > 0)
				if(SceneManager.activeScenes[0].rootObjects.Contains(gameObject))
					SceneManager.activeScenes[0].rootObjects.Remove(gameObject);
			to.childs.Add(this);
			parent = to;
			if(OnTransformChange != null) OnTransformChange.Invoke();
		}
	}
	public class Rect
	{
		/// <summary>
		/// The x origin of the rect
		/// </summary>
		public float x;
		/// <summary>
		/// The y origin of the rect
		/// </summary>
		public float y;
		/// <summary>
		/// Width of the rect
		/// </summary>
		public float w;
		/// <summary>
		/// Height of the rect
		/// </summary>
		public float h;
		/// <summary>
		/// Center point of the rect
		/// </summary>
		public Vector2 c
		{
			get
			{
				return new Vector2(x + (w / 2), y + (h / 2));
			}
		}
		/// <summary>
		/// Origin point of the rect
		/// </summary>
		public Vector2 o
		{
			get
			{
				return new Vector2(x, y);
			}
		}
		/// <summary>
		/// Scale of the rect
		/// </summary>
		public Vector2 s
		{
			get
			{
				return new Vector2(w, h);
			}
		}
		// Constructors
		public Rect()
		{
			
		}
		
		public Rect(float x, float y, float w, float h)
		{
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
		}
		
		public Rect(Vector2 position, Vector2 scale)
		{
			x = position.x;
			y = position.y;
			var d = scale - Vector2.one;
			x = x - d.x / 2;
			y = y - d.y / 2;
			w = scale.x;
			h = scale.y;
		}
		
		// Conversion between SDL_Rect and this RECT
		public static implicit operator SDL.SDL_Rect(Rect v) {
			return new SDL.SDL_Rect(){
				x = Convert.ToInt32(v.x),
				y = Convert.ToInt32(v.y),
				w = Convert.ToInt32(v.w),
				h = Convert.ToInt32(v.h)
			};
		}
		public static implicit operator Rect(SDL.SDL_Rect v) {
			return new Rect(){
				x = v.x,
				y = v.y,
				w = v.w,
				h = v.h
			};
		}
	}
}
