using System;
using Newtonsoft.Json;

namespace Penyata
{
	public class Collider : Component
	{
		public Vector2 size = Vector2.one;
		public bool drawColliderRect;
		[JsonIgnore]
		public Rect collideRect 
		{
			get
			{
				return new Rect(transform.position, transform.localScale * size);
			}
		}
		public override void OnStart()
		{
			Collision.RegisterCollider(this);
		}
	}
}
