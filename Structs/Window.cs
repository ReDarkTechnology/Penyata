using System;
using System.Runtime.InteropServices;

namespace Penyata.Win
{
	public class Window
	{
		public IntPtr window;
		public string name;
		public WindowManager.RECT rect;
		public Vector2 size {
			get {
				WindowManager.GetWindowRect(window, ref rect);
				int width = rect.Right - rect.Left;
				int height = rect.Bottom - rect.Top;
				return new Vector2(width, height);
			}
			set {
				WindowManager.GetWindowRect(window, ref rect);
				WindowManager.MoveWindow(window, Convert.ToInt32(position.x), Convert.ToInt32(position.y), Convert.ToInt32(value.x), Convert.ToInt32(value.y), true);
			}
		}
		public Vector2 position {
			get {
				WindowManager.GetWindowRect(window, ref rect);
				return new Vector2(rect.Left, rect.Top);
			}
			set {
				WindowManager.GetWindowRect(window, ref rect);
				WindowManager.MoveWindow(window, Convert.ToInt32(value.x), Convert.ToInt32(value.y), Convert.ToInt32(size.x), Convert.ToInt32(size.y), true);
			}
		}
		public bool topMost
		{
			get
			{
				return IsWindowTopMost(window);
			}
			set
			{
				var p = position;
				var s = size;
				IntPtr mode = IntPtr.Zero;
				if(value)
					mode = WindowManager.HWND_TOPMOST;
				else
					mode = WindowManager.HWND_NORMAL;
				WindowManager.SetWindowPos (
					window, 
					mode, Convert.ToInt32(p.x), Convert.ToInt32(p.y), Convert.ToInt32(s.x), Convert.ToInt32(s.y),
					WindowManager.SWP_NOMOVE | WindowManager.SWP_NOSIZE
				);
			}
		}
		
		public Window()
		{
			
		}
		public Window(IntPtr window)
		{
			this.window = window;
		}
		public void RefreshRect()
		{
			WindowManager.GetWindowRect(window, ref rect);
		}
		
		// Extension
		[DllImport("user32.dll", SetLastError=true)]
		static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		
		const int GWL_EXSTYLE = -20;
		const int WS_EX_TOPMOST = 0x0008;
		
		public static bool IsWindowTopMost(IntPtr hWnd)
		{
		    int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
		    return (exStyle & WS_EX_TOPMOST) == WS_EX_TOPMOST;
		}
	}
}