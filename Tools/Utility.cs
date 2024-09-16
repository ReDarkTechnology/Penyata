using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using Color = Penyata.Color;
using System.Collections.Generic;

namespace Penyata
{
	public static class Utility
	{
		static char[] randomMD5 = "abcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();
		public static List<string> knownMD5 = new List<string>();
		public static string GetMD5()
		{
			var rand = new Random();
			string build = null;
			for (int i = 0; i < 20; i++) {
				build += randomMD5[rand.Next(0, randomMD5.Length - 1)];
			}
			if (knownMD5.Contains(build)) {
				build = GetMD5();
			}
			knownMD5.Add(build);
			return build;
		}
		public static int ToInt(this float v)
		{
			return int.Parse(v.ToString("0"));
		}
		public static string GetValidSender(object sender)
		{
			if(sender.GetType().IsAssignableFrom(typeof(Type)))
				return ((Type)sender).FullName;
			return sender.GetType().IsAssignableFrom(typeof(String)) ? sender.ToString() : sender.GetType().FullName;
		}
		public static Color ColorFromString(string str)
		{
			str = str.Remove(0, 1);
			str = str.Remove(str.Length - 1, 1);
			var values = str.Split(",".ToCharArray());
			return new Color(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
		}
		public static bool HaveAnyInheritanceWith(this object obj, Type type)
		{
			var t = obj.GetType();
			return (t.IsSubclassOf(type) || t.IsAssignableFrom(type));
		}
		public static string ColorToString(Color color)
		{
			string build = "(";
			build += color.r;
			build += ",";
			build += color.g;
			build += ",";
			build += color.b;
			build += ",";
			build += color.a;
			build += ")";
			return build;
		}
		public static Vector2 Vector2FromString(string str)
		{
			str = str.Remove(0);
			str = str.Remove(str.Length - 1);
			var values = str.Split(",".ToCharArray());
			return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
		}
		public static string Vector2ToString(Vector2 vector)
		{
			string build = "(";
			build += vector.x;
			build += ",";
			build += vector.y;
			build += ")";
			return build;
		}
		public static Vector2 GetScreenSize(bool squarize = true)
		{
			var screen = new Vector2(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
			if (squarize) {
				screen = SquarizeVector(screen);
			}
			return screen;
		}
		public static Vector2 LocalCanvas = new Vector2(1f, 1f);
		public static Vector2 ConvertWorldToLocal(Vector2 world)
		{
			var screen = GetScreenSize();
			var originalScreen = GetScreenSize(false);
			var _add = (originalScreen / screen) / 4;
			var ratio = GetRatio(LocalCanvas, screen);
			return world * (float)ratio;
		}
		public static Vector2 ConvertLocalToWorld(Vector2 local)
		{
			var screen = GetScreenSize();
			var originalScreen = GetScreenSize(false);
			var _add = (originalScreen / screen) / 4;
			var ratio = GetRatio(LocalCanvas, screen);
			var scale = ResizeScreen(LocalCanvas, GetScreenSize());
			return local / (float)ratio;
		}
		public static Vector2 SquarizeVector(Vector2 vector)
		{
			// disable once CompareOfFloatsByEqualityOperator
			if (vector.x < vector.y) {
				return new Vector2(vector.x, vector.x);
			}
			if (vector.x > vector.y) {
				return new Vector2(vector.y, vector.y);
			}
			return vector;
		}
		public static Vector2 ResizeScreen(Vector2 canvas, Vector2 original)
		{
			double ratio = GetRatio(canvas, original);

			// now we can get the new height and width
			float newHeight = original.y * (float)ratio;
			float newWidth = original.x * (float)ratio;
        
			return new Vector2(newWidth, newHeight);
		}
		public static Vector2 ResizeScreen(Vector2 original, double ratio)
		{
			// now we can get the new height and width
			float newHeight = original.y * (float)ratio;
			float newWidth = original.x * (float)ratio;
        
			return new Vector2(newWidth, newHeight);
		}
		public static double GetRatio(Vector2 canvas, Vector2 original)
		{
			// Figure out the ratio
			double ratioX = (double)canvas.x / (double)original.x;
			double ratioY = (double)canvas.y / (double)original.y;
			// use whichever multiplier is smaller
			return ratioX < ratioY ? ratioX : ratioY;
		}
		public static string FloatToString(float fr)
		{
			var result = fr.ToString();
			try {
				result = result.Replace((",").ToCharArray()[0], (".").ToCharArray()[0]);
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}
			return result;
		}
		public static float StringToFloat(string fr)
		{
			fr = fr.Replace(".", ",");
			return float.Parse(fr);
		}
		public static int GetClampedIntFromTextBox(TextBox box, int defaultValue = 0, int min = 20, int max = 1000)
		{
			try {
				int i = int.Parse(box.Text);
				if (i < min)
					i = min;
				if (i > max)
					i = max;
				box.Text = i.ToString();
				return i;
			} catch {
				box.Text = defaultValue.ToString();
				return defaultValue;
			}
		}

		public static ListViewItem[] GetSelectedItems(ListView view)
		{
			var items = new List<ListViewItem>();
			if (view.SelectedItems.Count > 0) {
				foreach (ListViewItem item in view.SelectedItems) {
					if (item != null)
						items.Add(item);
				}
			}
			return items.ToArray();
		}

		public static Control[] GetControls(Control control, bool deep = true)
		{
			var controll = new List<Control>();
			foreach (Control pb in control.Controls) {
				controll.Add(pb);
			}
			if (deep) {
				foreach (Control ctrl in control.Controls) {
					var controls = GetControls(ctrl);
					foreach (var butt in controls) {
						controll.Add(butt);
					}
				}
			}
			return controll.ToArray();
		}

		public static T[] GetControlsWithType<T>(Control control)
		{
			var controll = new List<T>();
			foreach (object pb in control.Controls) {
				string a = pb.GetType().ToString();
				string b = typeof(T).ToString();
				Console.WriteLine("1 : " + a + " -- " + "2 : " + b);
				if (a == b) {
					Console.WriteLine("Match!");
					controll.Add((T)pb);
				} 
			}
			foreach (Control ctrl in control.Controls) {
				var controls = GetControlsWithType<T>(ctrl);
				foreach (object butt in controls) {
					controll.Add((T)butt);
				}
			}
			return controll.ToArray();
		}
		#region Encryption
		// This constant is used to determine the keysize of the encryption algorithm in bits.
		// We divide this by 8 within the code below to get the equivalent number of bytes.
		static int Keysize = 256;

		// This constant determines the number of iterations for the password bytes generation function.
		static int DerivationIterations = 1000;

		public static string laPhrase = "scenePass";

		public static string Encrypt(string plainText, string passPhrase = null)
		{
			if (string.IsNullOrEmpty(passPhrase)) {
				passPhrase = laPhrase;
			}
			// Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
			// so that the same Salt and IV values can be used when decrypting.  
			var saltStringBytes = Generate256BitsOfRandomEntropy();
			var ivStringBytes = Generate256BitsOfRandomEntropy();
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations)) {
				var keyBytes = password.GetBytes(Keysize / 8);
				using (var symmetricKey = new RijndaelManaged()) {
					symmetricKey.BlockSize = 256;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes)) {
						using (var memoryStream = new MemoryStream()) {
							using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
								cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
								cryptoStream.FlushFinalBlock();
								// Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
								var cipherTextBytes = saltStringBytes;
								cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
								cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
								memoryStream.Close();
								cryptoStream.Close();
								return Convert.ToBase64String(cipherTextBytes);
							}
						}
					}
				}
			}
		}

		public static string Decrypt(string cipherText, string passPhrase = null)
		{	
			if (string.IsNullOrEmpty(passPhrase)) {
				passPhrase = laPhrase;
			}
			// Get the complete stream of bytes that represent:
			// [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
			var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
			// Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
			var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
			// Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
			var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
			// Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
			var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

			using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations)) {
				var keyBytes = password.GetBytes(Keysize / 8);
				using (var symmetricKey = new RijndaelManaged()) {
					symmetricKey.BlockSize = 256;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes)) {
						using (var memoryStream = new MemoryStream(cipherTextBytes)) {
							using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)) {
								var plainTextBytes = new byte[cipherTextBytes.Length];
								var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
								memoryStream.Close();
								cryptoStream.Close();
								return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
							}
						}
					}
				}
			}
		}
	
		private static byte[] Generate256BitsOfRandomEntropy()
		{
			var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
			using (var rngCsp = new RNGCryptoServiceProvider()) {
				// Fill the array with cryptographically secure random bytes.
				rngCsp.GetBytes(randomBytes);
			}
			return randomBytes;
		}
		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
		#endregion
		public static byte[] Compress(byte[] data)
		{
			MemoryStream output = new MemoryStream();
			using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal)) {
				dstream.Write(data, 0, data.Length);
			}
			return output.ToArray();
		}
	
		public static byte[] Decompress(byte[] data)
		{
			MemoryStream input = new MemoryStream(data);
			MemoryStream output = new MemoryStream();
			using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress)) {
				dstream.CopyTo(output);
			}
			return output.ToArray();
		}
	
		#region Moveable Window
	
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
	
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		public static void MoveOnMouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{     
			if (e.Button == MouseButtons.Left) {
				Control control = sender as Control;
				ReleaseCapture();
				SendMessage(control.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}
	
		public class MoveableSerialized
		{
			public bool enabled = true;
		
			public Control root;
			public Control[] childs = { };
		
			public MoveableSerialized(Control root = null, bool recursive = false)
			{
				if (root != null) {
					this.root = root;
					if (recursive) {
						childs = Utility.GetControls(root);
					}
					InjectMovingFunction(recursive);
				}
			}
		
			public void InjectMovingFunction(bool isRecursive)
			{
				if (root != null) {
					root.MouseDown += (u, i) => {
						if (enabled)
							Utility.MoveOnMouseClick(u, i);
					};
					if (!isRecursive)
						return;
					if (childs.Length > 0) {
						foreach (var a in childs) {
							if (a != root) {
								a.MouseDown += (aa, ee) => {
									if (enabled)
										Utility.MoveOnMouseClick(root, ee);
								};
							}
						}
					}
				}
			}
		}
		public static MoveableSerialized SetControlToMoveRoot(Control root, bool recursive = false)
		{
			return new MoveableSerialized(root, recursive);
		}
		#endregion
	}
	public enum Space
	{
		World,
		Self
	}
	public enum SendMessageOptions
	{
		RequireReceiver,
		DontRequireReceiver
	}
	internal enum RotationOrder
	{
		OrderXYZ,
		OrderXZY,
		OrderYZX,
		OrderYXZ,
		OrderZXY,
		OrderZYX
	}
}
