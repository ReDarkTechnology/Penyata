using System;
using System.Linq;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Penyata
{
	public class GUIContainer : GUI
	{
		public object host {get; private set;}
		public Control.ControlCollection controls 
		{
			get 
			{
				if(isForms) return ((Control)host).Controls;
				return null;
			}
		}
		bool init;
		private bool _isForms;
		public bool isForms
		{
			get
			{
				if(!init) {
					_isForms = host.GetType().IsAssignableFrom(typeof(Control));
					init = true;
				}
				return _isForms;
			}
		}
		public Padding padding = new Padding(1);
		static object[] CToO(Control.ControlCollection col)
		{
			var objs = new List<object>((IEnumerable<Control>)col);
			return objs.ToArray();
		}
		public GUIContainer()
		{
			
		}
		
		public GUIContainer(Panel panel)
		{
			host = panel;
			_isForms = host.GetType().IsAssignableFrom(typeof(Control));
			init = true;
		}
		
		public GUIContainer(GUI gui)
		{
			host = gui;
			_isForms = host.GetType().IsAssignableFrom(typeof(Control));
			init = true;
		}
	}
}
