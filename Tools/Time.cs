using System;
using SDL2;

namespace Penyata
{
	public class Time : BaseDrawUpdate
	{
		public static float time;
		public static float timeScale = 1;
		public static float deltaTime = 0.1f;
		public static float unscaledDeltaTime = 0.1f;
		public static float fixedDeltaTime = 0.01f;
		public static int frameCount = 1;
		public static int targetFPS = 60;
		public static DateTime time1 = DateTime.Now;
		public static DateTime time2 = DateTime.Now;
		public static int framesSinceStart;
		public override void Update()
		{
			time2 = DateTime.Now;
			unscaledDeltaTime = (time2.Ticks - time1.Ticks) / 10000000f;
			deltaTime = unscaledDeltaTime * timeScale;
			time += unscaledDeltaTime;
			time1 = time2;
		}
	}
}