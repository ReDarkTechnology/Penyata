using System;

namespace Penyata.Serialization
{
	public static class SerializerConverters
	{
		/// <summary>
		/// Serializes a GameObject
		/// </summary>
		/// <param name="obj">The GameObject</param>
		/// <param name="recursive">Does it need to also serialize the childs?</param>
		/// <returns>The serialized properties as a class</returns>
		public static SerializedGameObject Serialize(this GameObject obj, bool recursive)
		{
			var cls = SerializedGameObject.SerializeGameObject(obj, recursive);
			return cls;
		}
		
		public static string Serialize(this object obj, bool prettyPrint = false)
		{
			var f = Newtonsoft.Json.Formatting.None;
			if(prettyPrint) f = Newtonsoft.Json.Formatting.Indented;
			return Newtonsoft.Json.JsonConvert.SerializeObject(obj, f);
		}
		
		public static object Deserialize(this string serialized)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject(serialized);
		}
		
		public static T Deserialize<T>(this string serialized)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serialized);
		}
	}
}
