using System;
using System.Collections.Generic;

namespace Penyata
{
	public class GUI
	{
		public GUIContainer container;
		public Rect rect = new Rect(-0.5f, -0.5f, 1, 1);
		public Color color = Color.white;
		
		public GUI parent;
		public GUI[] childs;
		
		public object[] childsObj
		{
			get
			{
				var c = new List<object>(childs);
				return c.ToArray();
			}
		}
		public virtual void Draw()
		{
			if(container.isForms)
			{
				
			}
		}
	}
}
