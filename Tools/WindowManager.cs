using System;
using System.Runtime.InteropServices;

namespace Penyata.Win
{
	public static class WindowManager
	{
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
		
		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
		private const int SW_MAXIMIZE = 3;
		private const int SW_MINIMIZE = 6;

		[DllImport("user32.dll", EntryPoint = "FindWindow")]
		public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

		private const int SW_SHOWNORMAL = 1;
		private const int SW_SHOWMINIMIZED = 2;
		private const int SW_SHOWMAXIMIZED = 3;
		
		public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
		public static readonly IntPtr HWND_NORMAL = new IntPtr(-2);
		public const UInt32 SWP_NOSIZE = 0x0001;
		public const UInt32 SWP_NOMOVE = 0x0002;
		public const UInt32 SWP_SHOWWINDOW = 0x0040;

		[DllImport("user32.dll")]
		public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
		public static bool MoveWindowPositon(string name, int x, int y)
		{
			try {
				var win = GetWindow(name);
				win.position = new Vector2(x, y);
				return true;
			} catch {
				return false;
			}
		}
		public static Window GetWindow(string name, bool showWindow = true)
		{
			var win = FindWindowByCaption(IntPtr.Zero, name);
			if (showWindow)
				ShowWindowAsync(win, SW_SHOWNORMAL);
			Window w = null;
			if (win != default(IntPtr)) {
				w = new Window(win);
				w.name = name;
			}
			GetWindowRect(win, ref w.rect);
			return w;
		}
	}
}