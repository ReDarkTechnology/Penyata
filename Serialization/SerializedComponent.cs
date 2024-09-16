using System;

namespace Penyata.Serialization
{
	[Serializable]
	public class SerializedComponent
	{
		public string name;
		public string data;
		public SerializedComponent()
		{
		
		}
		public SerializedComponent(string n, string d)
		{
			name = n;
			data = d;
		}
	}

	public interface SerializableComponent
	{
		string GetName();
		void ApplyTo(GameObject obj);
		SerializedComponent Serialize();
	}
}
