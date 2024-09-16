using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Penyata
{
	public static class WindowForm
	{
		public static void SaveWindowConfig(string name, Form form)
		{
			Configuration.SetString(name + "-Form:WindowConf", JsonConvert.SerializeObject(new FormConfig(form)));
		}
		public static FormConfig GetWindowConfig(string name)
		{
			var data = Configuration.GetString(name + "-Form:WindowConf");
			var config = new FormConfig();
			if(!string.IsNullOrEmpty(data))
			{
				config = JsonConvert.DeserializeObject<FormConfig>(data);
			}
			else
			{
				config.exist = false;
			}
			return config;
		}
		public static FormConfig ApplyWindowConfig(string name, Form form)
		{
			var data = Configuration.GetString(name + "-Form:WindowConf");
			var config = new FormConfig();
			if(!string.IsNullOrEmpty(data))
			{
				config = JsonConvert.DeserializeObject<FormConfig>(data);
				config.ApplyTo(form);
			}
			else
			{
				config.exist = false;
			}
			return config;
		}
	}
	
	public class FormConfig
	{
		public bool exist = true;
		
		public Point location = new Point(128, 128);
		public Size size = new Size(600, 400);
		public FormWindowState windowState = FormWindowState.Normal;
		
		public FormConfig()
		{
			
		}
		
		public FormConfig(Form form)
		{
			location = form.Location;
			size = form.Size;
			windowState = form.WindowState;
		}
		
		public void ApplyTo(Form form)
		{
			form.Location = location;
			form.Size = size;
			form.WindowState = windowState;
		}
	}
}
