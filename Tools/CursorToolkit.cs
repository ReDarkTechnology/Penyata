using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Penyata
{
	public static class CursorToolkit
	{
		public static List<Cursor> cursorHistory = new List<Cursor>();
		public static void ChangeCursor (Cursor cursor)
		{
			Cursor.Current = cursor;
		}
		public static void UndoCursor ()
		{
			var c = cursorHistory.Count;
			if(c > 0)
			{
				Cursor.Current = cursorHistory[0];
				cursorHistory.RemoveAt(c - 1);
			}
		}
	}
}
