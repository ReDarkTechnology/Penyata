using System;
using System.Collections.Generic;
using SDL2;

namespace Penyata
{
	public static class Drawer
	{
		public static bool debugMode;
		public static List<DrawRequest> requests = new List<DrawRequest>();
		static int debugDelay = 30;
		/// <summary>
		/// Request to draw a square
		/// </summary>
		/// <param name="tr">Transform</param>
		/// <param name="fill">Is a filled square</param>
		/// <param name="color">The color</param>
		public static void DrawSquare(Transform tr, bool fill, Color color)
		{
			var r = new DrawRequest();
			r.layer = tr.gameObject.layer;
			r.original = tr.rect;
			r.OnDraw += (rn, ts) =>
			{
				SDL.SDL_SetRenderDrawColor(rn.renderer, color.byteR, color.byteG, color.byteB, color.byteA);
				var aa = (SDL.SDL_Rect)ts;
				if(fill)
					SDL.SDL_RenderFillRect(rn.renderer, ref aa);
				else
					SDL.SDL_RenderDrawRect(rn.renderer, ref aa);
			};
			requests.Add(r);
		}
		public static void ClearRequests()
		{
			requests.Clear();
		}
		/// <summary>
		/// Get the direction of a given angle
		/// </summary>
		/// <param name="angle">The angle</param>
		/// <returns>A vector2 direction</returns>
		public static Vector2 GetDirection(double angle)
		{
			angle = SubtractAngle(angle);
			float x = (float)Math.Cos(angle * Mathf.PI / 180);
			float y = (float)Math.Sin(angle * Mathf.PI / 180);
			return new Vector2(x, y);
		}
		static double SubtractAngle(double angle)
		{
			if (angle > 360) {
				angle = angle - 360;
			}
			if (angle > 360)
				angle = SubtractAngle(angle);
			return angle;
		}
		/// <summary>
		/// Rotates one point around another
		/// </summary>
		/// <param name="pointToRotate">The point to rotate.</param>
		/// <param name="centerPoint">The center point of rotation.</param>
		/// <param name="angleInDegrees">The rotation angle in degrees.</param>
		/// <returns>Rotated point</returns>
		public static SDL.SDL_Point RotatePoint(SDL.SDL_Point pointToRotate, SDL.SDL_Point centerPoint, double angleInDegrees)
		{
			double angleInRadians = angleInDegrees * (Math.PI / 180);
			double cosTheta = Math.Cos(angleInRadians);
			double sinTheta = Math.Sin(angleInRadians);
			return new SDL.SDL_Point {
				x =
		            (int)
		            (cosTheta * (pointToRotate.x - centerPoint.x) -
				sinTheta * (pointToRotate.y - centerPoint.y) + centerPoint.x),
				y =
		            (int)
		            (sinTheta * (pointToRotate.x - centerPoint.x) +
				cosTheta * (pointToRotate.y - centerPoint.y) + centerPoint.y)
			};
		}
	}
}
