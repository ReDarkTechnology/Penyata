using System;
using SDL2;

namespace Penyata
{
	public struct Color
	{
		[Newtonsoft.Json.JsonIgnore]
		public byte byteR {
			get {
				return GetByte(r);
			}
			set {
				r = value / 255;
			}
		}
		[Newtonsoft.Json.JsonIgnore]
		public byte byteG {
			get {
				return GetByte(g);
			}
			set {
				g = value / 255;
			}
		}
		[Newtonsoft.Json.JsonIgnore]
		public byte byteB {
			get {
				return GetByte(b);
			}
			set {
				b = value / 255;
			}
		}
		[Newtonsoft.Json.JsonIgnore]
		public byte byteA {
			get {
				return GetByte(a);
			}
			set {
				a = value / 255;
			}
		}
		public float r;
		public float g;
		public float b;
		public float a;
		public Color(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		public Color(float r, float g, float b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 1f;
		}
		
		public Color(byte b_r, byte b_g, byte b_b, byte b_a)
		{
			this.r = (float)b_r / 255;
			this.g = (float)b_g / 255;
			this.b = (float)b_b / 255;
			this.a = (float)b_a / 255;
		}
		public Color Reverse()
		{
			return new Color(1 - r, 1 - g, 1 - b, 1 - a);
		}
		public byte GetByte(float f)
		{
			Normalize();
			byte o = Convert.ToByte(Convert.ToInt32(f * 255));
			return o;
		}
		public void Normalize()
		{
			r = Mathf.Clamp(r, 0, 1);
			g = Mathf.Clamp(g, 0, 1);
			b = Mathf.Clamp(b, 0, 1);
			a = Mathf.Clamp(a, 0, 1);
		}
		public static Color red {
			get {
				return new Color(1f, 0f, 0f, 1f);
			}
		}

		public static Color green {
			get {
				return new Color(0f, 1f, 0f, 1f);
			}
		}

		public static Color blue {
			get {
				return new Color(0f, 0f, 1f, 1f);
			}
		}

		public static Color white {
			get {
				return new Color(1f, 1f, 1f, 1f);
			}
		}

		public static Color black {
			get {
				return new Color(0f, 0f, 0f, 1f);
			}
		}

		public static Color yellow {
			get {
				return new Color(1f, 0.921568632f, 0.0156862754f, 1f);
			}
		}

		public static Color cyan {
			get {
				return new Color(0f, 1f, 1f, 1f);
			}
		}

		public static Color magenta {
			get {
				return new Color(1f, 0f, 1f, 1f);
			}
		}

		public static Color gray {
			get {
				return new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}

		public static Color grey {
			get {
				return new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}

		public static Color clear {
			get {
				return new Color(0f, 0f, 0f, 0f);
			}
		}
		public static Color operator +(Color a, Color b)
		{
			return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
		}

		public static Color operator -(Color a, Color b)
		{
			return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		public static Color operator *(Color a, Color b)
		{
			return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
		}

		public static Color operator *(Color a, float b)
		{
			return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		public static Color operator *(float b, Color a)
		{
			return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		public static Color operator /(Color a, float b)
		{
			return new Color(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		public static Color Lerp(Color a, Color b, float t)
		{
			t = Mathf.Clamp01(t);
			return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
		}
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			return (obj is Color) && Equals((Color)obj);
		}

		public bool Equals(Color other)
		{
			return object.Equals(this.r, other.r) && object.Equals(this.g, other.g) && object.Equals(this.b, other.b) && object.Equals(this.a, other.a);
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				// disable NonReadonlyReferencedInGetHashCode
				hashCode += 1000000007 * r.GetHashCode();
				hashCode += 1000000009 * g.GetHashCode();
				hashCode += 1000000021 * b.GetHashCode();
				hashCode += 1000000033 * a.GetHashCode();
			}
			return hashCode;
		}
		public static bool operator ==(Color lhs, Color rhs)
		{
			// disable CompareOfFloatsByEqualityOperator
			return (lhs.r == rhs.r && lhs.g == rhs.g && lhs.b == rhs.b && lhs.a == rhs.a);
		}

		public static bool operator !=(Color lhs, Color rhs)
		{
			return !(lhs == rhs);
		}

		#endregion

		public static Color LerpUnclamped(Color a, Color b, float t)
		{
			return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
		}

		internal Color RGBMultiplied(float multiplier)
		{
			return new Color(this.r * multiplier, this.g * multiplier, this.b * multiplier, this.a);
		}

		internal Color AlphaMultiplied(float multiplier)
		{
			return new Color(this.r, this.g, this.b, this.a * multiplier);
		}
		public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
		{
			if (rgbColor.b > rgbColor.g && rgbColor.b > rgbColor.r) {
				Color.RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V);
			} else {
				if (rgbColor.g > rgbColor.r) {
					Color.RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V);
				} else {
					Color.RGBToHSVHelper(0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V);
				}
			}
		}

		private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
		{
			V = dominantcolor;
			if (V != 0f) {
				float num;
				if (colorone > colortwo) {
					num = colortwo;
				} else {
					num = colorone;
				}
				float num2 = V - num;
				if (num2 != 0f) {
					S = num2 / V;
					H = offset + (colorone - colortwo) / num2;
				} else {
					S = 0f;
					H = offset + (colorone - colortwo);
				}
				H /= 6f;
				if (H < 0f) {
					H += 1f;
				}
			} else {
				S = 0f;
				H = 0f;
			}
		}

		public static Color HSVToRGB(float H, float S, float V)
		{
			return Color.HSVToRGB(H, S, V, true);
		}

		public static Color HSVToRGB(float H, float S, float V, bool hdr)
		{
			Color w = Color.white;
			if (S == 0f) {
				w.r = V;
				w.g = V;
				w.b = V;
			} else {
				if (V == 0f) {
					w.r = 0f;
					w.g = 0f;
					w.b = 0f;
				} else {
					w.r = 0f;
					w.g = 0f;
					w.b = 0f;
					float num = H * 6f;
					int num2 = (int)Mathf.Floor(num);
					float num3 = num - (float)num2;
					float num4 = V * (1f - S);
					float num5 = V * (1f - S * num3);
					float num6 = V * (1f - S * (1f - num3));
					switch (num2 + 1) {
						case 0:
							w.r = V;
							w.g = num4;
							w.b = num5;
							break;
						case 1:
							w.r = V;
							w.g = num6;
							w.b = num4;
							break;
						case 2:
							w.r = num5;
							w.g = V;
							w.b = num4;
							break;
						case 3:
							w.r = num4;
							w.g = V;
							w.b = num6;
							break;
						case 4:
							w.r = num4;
							w.g = num5;
							w.b = V;
							break;
						case 5:
							w.r = num6;
							w.g = num4;
							w.b = V;
							break;
						case 6:
							w.r = V;
							w.g = num4;
							w.b = num5;
							break;
						case 7:
							w.r = V;
							w.g = num6;
							w.b = num4;
							break;
					}
					if (!hdr) {
						w.r = Mathf.Clamp(w.r, 0f, 1f);
						w.g = Mathf.Clamp(w.g, 0f, 1f);
						w.b = Mathf.Clamp(w.b, 0f, 1f);
					}
				}
			}
			return w;
		}
		public static implicit operator SDL.SDL_Color(Color v)
		{
			return new SDL.SDL_Color() {
				r = v.byteR,
				g = v.byteG,
				b = v.byteB,
				a = v.byteA
			};
		}
		public static implicit operator Color(SDL.SDL_Color v)
		{
			return new Color(v.r, v.g, v.b, v.a);
		}
	}
}
