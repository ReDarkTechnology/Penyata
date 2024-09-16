using System;
using System.Collections.Generic;

namespace Penyata
{
	// disable once ConvertToStaticType
	public class Collision : BaseDrawUpdate
	{
		public static List<Collider> colliders = new List<Collider>();
		static List<Collide> EnterCollides = new List<Collide>();
		public static void RegisterCollider(Collider col)
		{
			if(!colliders.Contains(col)) colliders.Add(col);
		}
		public static void DeleteCollider(Collider col)
		{
			if(colliders.Contains(col)) colliders.Remove(col);
		}
		public static bool IsColliding(Collider rhs, Collider lhs)
		{
			Rect a = rhs.collideRect;
			Rect b = lhs.collideRect;
			if (a.x + a.w > b.x && 
			    a.x < b.x + b.w && 
			    a.y + a.h > b.y && 
			    a.y < b.y + b.h)
				return true;
			
		    return false;
		}
		public override void Update()
		{
			
		}
		public class Collide
		{
			public bool isEntering = true;
			public bool isColliding;
			public bool isExiting;
			
			public float collideTime = 1;
			
			public Collider a;
			public Collider b;
			
			public Collide()
			{
				
			}
			public Collide(Collider rhs, Collider lhs)
			{
				a = rhs;
				b = lhs;
			}
			public bool CheckCollision()
			{
				bool result = Collision.IsColliding(a, b);
				
				if(collideTime > 0) {
					collideTime--;
				}
				return result;
			}
		}
	}
}
