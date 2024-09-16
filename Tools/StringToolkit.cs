using System;

namespace Penyata
{
	public static class StringToolkit
	{
		public static bool IsTextContains(this string text, string contain)
		{
			if (string.IsNullOrEmpty(contain))
				return string.IsNullOrEmpty(text);
			if (!string.IsNullOrEmpty(text)) {
				int process = 0;
				char firstChar = contain[0];
				char nextChar = ' ';
				bool found = false;
				foreach (var str in text) {
					try {
						nextChar = contain[process];
					} catch {
					}
					if (str == firstChar) {
						if (found && firstChar == nextChar) {
							if (process < contain.Length - 1) {
								process++;
							} else {
								return true;
							}
						} else {
							found = true;
							process = 1;
						}
					} else {
						if (str == nextChar) {
							if (process < contain.Length - 1) {
								process++;
							} else {
								return true;
							}
						} else {
							found = false;
							process = 0;
						}
					}
				}
			} else {
				return string.IsNullOrEmpty(contain);
			}
			return false;
		}
		public static string Combine(string[] texts)
		{
			string text = " ";
			foreach (var a in texts) {
				text += a + "-";
			}
			if (!string.IsNullOrWhiteSpace(text)) {
				text = text.Remove(text.Length - 1);
			}
			return text;
		}
		public static string ReplaceText(string text, string find, string replace)
		{
			if (string.IsNullOrWhiteSpace(find))
				return text;
			if (!string.IsNullOrWhiteSpace(text)) {
				string result = text;
				int process = 0;
				int i = 0;
				int startChar = -1;
				char firstChar = find[0];
				char nextChar = ' ';
				bool found = false;
				foreach (var str in text) {
					try {
						nextChar = find[process];
					} catch {
						process = 10;
					}
					if (str == firstChar) {
						if (found && firstChar == nextChar) {
							if (process < find.Length - 1) {
								process++;
							} else {
								result = result.Remove(startChar, process + 1);
								result = result.Insert(startChar, replace);
								startChar = -1;
							}
						} else {
							startChar = i;
							found = true;
							process = 1;
						}
					} else {
						if (str == nextChar) {
							if (process < find.Length - 1) {
								process++;
							} else {
								result = result.Remove(startChar, process + 1);
								result = result.Insert(startChar, replace);
								startChar = -1;
							}
						} else {
							startChar = -1;
							found = false;
							process = 0;
						}
					}
					i++;
				}
				return result;
			} else {
				return text;
			}
		}
	}
}
