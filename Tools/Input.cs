using System;
using SDL2;
using Keys = SDL2.SDL.SDL_Keycode;
using System.Collections.Generic;

namespace Penyata
{
	public class Input : BaseDrawUpdate
	{
		#region InputKeys
		// KeyPresses
		public static List<Keys> keyDowns = new List<Keys>();
		public static Action<Keys> OnKeyDown;
		public List<Keys> keyDownsRemove = new List<Keys>();
		public static List<Keys> keyUps = new List<Keys>();
		public static Action<Keys> OnKeyUp;
		public List<Keys> keyUpsRemove = new List<Keys>();
		public static List<Keys> keyPressed = new List<Keys>();
		public List<Keys> keyPressedRemove = new List<Keys>();
		// MousePresses
		public static List<byte> mouseDowns = new List<byte>();
		public static Action<byte> OnMouseDown;
		public List<byte> mouseDownsRemove = new List<byte>();
		public static List<byte> mouseUps = new List<byte>();
		public static Action<byte> OnMouseUp;
		public List<byte> mouseUpsRemove = new List<byte>();
		public static List<byte> mousePressed = new List<byte>();
		public List<byte> mousePressedRemove = new List<byte>();
		#endregion
    
		#region CheckAdding
		public static List<KeyStatement> statements = new List<KeyStatement>();
		public static List<MouseStatement> mouseStatements = new List<MouseStatement>();
		public static bool LogInputs = true;
		public override void Update()
		{
			// Keys
			keyDowns = new List<Keys>();
			keyUps = new List<Keys>();
			var keyDestroyQueue = new List<KeyStatement>();
			foreach (KeyStatement stat in statements) {
				if (stat.keyTime > 0) {
					stat.keyTime--;
				} else {
					stat.isDoneDown = true;
				}
				if (!stat.isDoneDown) {
					keyDowns.Add(stat.keyCode);
					keyPressed.Add(stat.keyCode);
				}
				if (stat.isDoneDown) {
					if (!stat.isPressed) {
						if (!stat.isUp) {
							keyUps.Add(stat.keyCode);
							keyPressed.Remove(stat.keyCode);
							stat.isUp = true;
						} else {
							keyDestroyQueue.Add(stat);
						}
					}
				}
			}
			foreach (KeyStatement stat in keyDestroyQueue) {
				statements.Remove(stat);
			}
			// Mouse
			mouseDowns = new List<byte>();
			mouseUps = new List<byte>();
			var mouseDestroyQueue = new List<MouseStatement>();
			foreach (MouseStatement stat in mouseStatements) {
				if (stat.keyTime > 0) {
					stat.keyTime--;
				} else {
					stat.isDoneDown = true;
				}
				if (!stat.isDoneDown) {
					mouseDowns.Add(stat.keyCode);
					mousePressed.Add(stat.keyCode);
				}
				if (stat.isDoneDown) {
					if (!stat.isPressed) {
						if (!stat.isUp) {
							mouseUps.Add(stat.keyCode);
							mousePressed.Remove(stat.keyCode);
							stat.isUp = true;
						} else {
							mouseDestroyQueue.Add(stat);
						}
					}
				}
			}
			foreach (MouseStatement stat in mouseDestroyQueue) {
				mouseStatements.Remove(stat);
			}
			mouseWheel = tempScroll;
			tempScroll = Vector2.zero;
		}
		#endregion
    
		#region InputResults
		public static bool GetKeyDown(Keys keyCode)
		{
			return keyDowns.Contains(keyCode);
		}
		public static bool GetKeyDown(KeyCode key)
		{
			return GetKeyDown(key.ToSDLKeyCode());
		}
		public static bool GetKeyUp(Keys keyCode)
		{
			return keyUps.Contains(keyCode);
		}
		public static bool GetKeyUp(KeyCode key)
		{
			return GetKeyUp(key.ToSDLKeyCode());
		}
		public static bool GetKey(Keys keyCode)
		{
			return keyPressed.Contains(keyCode);
		}
		public static bool GetKey(KeyCode key)
		{
			return GetKey(key.ToSDLKeyCode());
		}
		public static bool GetMouseButton(byte button)
		{
			return mousePressed.Contains(button);
		}
		public static bool GetMouseButtonDown(byte button)
		{
			return mouseDowns.Contains(button);
		}
		public static bool GetMouseButtonUp(byte button)
		{
			return mouseUps.Contains(button);
		}
		public static bool GetMouseButton(MouseCode button)
		{
			return GetMouseButton((byte)button.ToUint());
		}
		public static bool GetMouseButtonDown(MouseCode button)
		{
			return GetMouseButtonDown((byte)button.ToUint());
		}
		public static bool GetMouseButtonUp(MouseCode button)
		{
			return GetMouseButtonUp((byte)button.ToUint());
		}
		public static bool IsStatementsHasKey(Keys keyCode)
		{
			bool result = false;
			foreach (KeyStatement stat in statements) {
				if (stat.keyCode.ToString() == keyCode.ToString())
					result = true;
			}
			return result;
		}
		public static bool IsStatementsHasKey(byte keyCode)
		{
			bool result = false;
			foreach (MouseStatement stat in mouseStatements) {
				if (stat.keyCode.ToString() == keyCode.ToString())
					result = true;
			}
			return result;
		}
		public static KeyResult GetKeyResult(Keys keyCode)
		{
			bool result = false;
			int index = 0;
			var res = new KeyResult();
			foreach (KeyStatement stat in statements) {
				if (stat.keyCode.ToString() == keyCode.ToString()) {
					result = true;
					res.statement = stat;
					res.index = index;
				}
				index++;
			}
			res.isExist = result;
			return res;
		}
		public static MouseResult GetMouseResult(byte button)
		{
			bool result = false;
			int index = 0;
			var res = new MouseResult();
			foreach (MouseStatement stat in mouseStatements) {
				if (stat.keyCode.ToString() == button.ToString()) {
					result = true;
					res.statement = stat;
					res.index = index;
				}
				index++;
			}
			res.isExist = result;
			return res;
		}
		#endregion
	
		#region AddInput
		static Vector2 tempScroll;
		public static void AddScroll(Vector2 s)
		{
			tempScroll = s;
		}
		public static void AddKeyDown(Keys val)
		{
			if (!IsStatementsHasKey(val)) {
				KeyStatement stat = new KeyStatement();
				if (OnKeyDown != null)
					OnKeyDown.Invoke(val);
				stat.keyCode = val;
				stat.keyTime = 1;
				stat.isPressed = true;
				statements.Add(stat);
			}
		}
		public static void AddMouseDown(byte button)
		{
			if (!IsStatementsHasKey(button)) {
				MouseStatement stat = new MouseStatement();
				if (OnMouseDown != null)
					OnMouseDown.Invoke(button);
				stat.keyCode = button;
				stat.keyTime = 1;
				stat.isPressed = true;
				mouseStatements.Add(stat);
			}
		}
		public static void AddKeyUp(Keys val)
		{
			KeyResult keyRes = GetKeyResult(val);
			if (OnKeyUp != null)
				OnKeyUp.Invoke(val);
			if (keyRes.isExist) {
				keyRes.statement.isPressed = false;
			}
		}
		public static void AddMouseUp(byte button)
		{
			MouseResult keyRes = GetMouseResult(button);
			if (OnMouseUp != null)
				OnMouseUp.Invoke(button);
			if (keyRes.isExist) {
				keyRes.statement.isPressed = false;
			}
		}
		#endregion
	
		#region Mouse
		public static Vector2 mousePosition {
			get {
				int x, y;
				SDL.SDL_GetMouseState(out x, out y);
				return new Vector2(x, y);
			}
		}
		public static Vector2 mouseWheel;
		public static bool IsMouseHovering(Transform v)
		{
			Rect a = v.rect;
			var b = new Rect(mousePosition, Vector2.one);
			if (a.x + a.w > b.x &&
			   a.x < b.x + b.w &&
			   a.y + a.h > b.y &&
			   a.y < b.y + b.h)
				return true;
		
			return false;
		}
		#endregion
	}
	[Serializable]
	public class KeyStatement
	{
		public Keys keyCode;
		public int keyTime;
		public bool isPressed;
		public bool isDoneDown;
		public bool isUp;
	}
	[Serializable]
	public class MouseStatement
	{
		public byte keyCode;
		public int keyTime;
		public bool isPressed;
		public bool isDoneDown;
		public bool isUp;
	}
	[Serializable]
	public class KeyResult
	{
		public bool isExist;
		public KeyStatement statement;
		public int index;
	}
	[Serializable]
	public class MouseResult
	{
		public bool isExist;
		public MouseStatement statement;
		public int index;
	}
}