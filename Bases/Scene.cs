using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Penyata.Serialization;

namespace Penyata
{
	public class Scene
	{
		public int buildIndex = -1;
		public string name;
		public List<GameObject> rootObjects = new List<GameObject>();
		
		public string SerializeScene()
		{
			return Utility.Encrypt(SerializeToSC().Serialize());
		}
		
		public SerializedScene SerializeToSC()
		{
			var cls = new SerializedScene();
			cls.index = buildIndex;
			cls.name = name;
			foreach(var a in rootObjects)
			{
				cls.rootObjects.Add(a.Serialize(true));
			}
			return cls;
		}
		
		public T FindObjectOfType<T>()
		{
			foreach (var obj in rootObjects) {
				var comp = obj.GetComponent<T>(true);
				if(!EqualityComparer<T>.Default.Equals(comp)) return comp;
			}
			return default(T);
		}
		
		public static Scene Deserialize(string data, int buildIndex)
		{
			var scene = new Scene();
			SerializedScene s_scene = Deserialize(data);
			scene.name = s_scene.name;
			scene.buildIndex = buildIndex;
			foreach(var obj in s_scene.rootObjects)
			{
				scene.rootObjects.Add(obj.Spawn());
			}
			return scene;
		}
		public static Scene Deserialize(SerializedScene s_scene, int buildIndex)
		{
			var scene = new Scene();
			scene.name = s_scene.name;
			scene.buildIndex = buildIndex;
			foreach(var obj in s_scene.rootObjects)
			{
				scene.rootObjects.Add(obj.Spawn());
			}
			return scene;
		}
		
		public static SerializedScene Deserialize(string data)
		{
			SerializedScene s_scene = Utility.Decrypt(data).Deserialize<SerializedScene>();
			return s_scene;
		}
	}
	public class SerializedScene
	{
		public int index;
		public string name;
		public List<SerializedGameObject> rootObjects = new List<SerializedGameObject>();
	}
}
