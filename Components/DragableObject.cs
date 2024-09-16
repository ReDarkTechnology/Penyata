using System;
using SDL2;

namespace Penyata
{
	public class DragableObject : Component
	{
		Vector2 prPos = Vector2.zero;
		BoxRenderer r_rend;
		BoxRenderer rend
		{
			get
			{
				if(r_rend == null) r_rend = GetComponent<BoxRenderer>();
				return r_rend;
			}
		}
		Color originColor = Color.white;
		Color hoverColor = new Color((byte)137, (byte)190, (byte)255, (byte)255);
		bool isMoving;
		public override void OnUpdate()
		{
			if(Input.IsMouseHovering(transform))
			{
				if(rend != null) rend.color = hoverColor;
				if(Input.GetMouseButtonDown((byte)SDL.SDL_BUTTON_LEFT))
				{
					prPos = inputSized;
					isMoving = true;
				}
			}
			else
			{
				if(rend != null) rend.color = originColor;
			}
			if(Input.GetMouseButtonUp((byte)SDL.SDL_BUTTON_LEFT))
			{
				isMoving = false;
			}
			if(isMoving)
			{
				var m = inputSized - prPos;
				transform.position += new Vector2(m.x, -m.y);
				prPos = inputSized;
			}
		}
		public static Vector2 inputSized
		{
			get
			{
				return Input.mousePosition;
			}
		}
	}
}
