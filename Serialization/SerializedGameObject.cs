using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Object = Penyata.Object;
using Newtonsoft.Json;

namespace Penyata.Serialization
{
	[Serializable]
	public class SerializedGameObject
	{
		// Info
		public bool selfActive;
		public string name;
		public string tag;
		public int instanceId;
		public int parentInstanceId;
		public int layer;
	
		// Transform
		public Vector2 localPosition;
		public float localEulerAngle;
		public Vector2 localScale;

		// Childs
		public SerializedGameObject[] childs;

		// Components
		public List<SerializedComponent> components = new List<SerializedComponent>();
		
		/// <summary>
		/// Serializes a GameObject
		/// </summary>
		/// <param name="obj">The GameObject</param>
		/// <param name="recursive">Does it need to also serialize the childs?</param>
		/// <returns>The serialized properties as a class</returns>
		public static SerializedGameObject SerializeGameObject(GameObject obj, bool recursive = true)
		{
			var cls = new SerializedGameObject();
			cls.name = obj.name;
			cls.tag = obj.tag;
			cls.layer = obj.layer;

			cls.selfActive = obj.activeSelf;

			cls.localPosition = obj.transform.localPosition;
			cls.localEulerAngle = obj.transform.rotation;
			cls.localScale = obj.transform.localScale;
		
			cls.instanceId = obj.instanceID;
		
			if (obj.transform.parent != null) {
				cls.parentInstanceId = obj.transform.parent.gameObject.instanceID;
			}
			
			foreach (var a in obj.components) {
				if (a.GetType().Name != "ObjectIdentity")
					cls.components.Add(new SerializedComponent(a.GetType().FullName, JsonConvert.SerializeObject(a)));
			}
			if (recursive) {
				var objs = obj.GetComponentsInChildren<Transform>();
				var rootObjs = new List<SerializedGameObject>();
				foreach (var ob in objs) {
					if (ob.gameObject != obj) {
						rootObjs.Add(SerializeGameObject(ob.gameObject));
					}
				}
				cls.childs = rootObjs.ToArray();
			}
			return cls;
		}
		
		/// <summary>
		/// Spawns a GameObject with the serialized properties.
		/// </summary>
		/// <param name="scene">The target scene to put this on (optional)</param>
		/// <returns>The spawned GameObject</returns>
		public GameObject Spawn(Scene scene = null)
		{
			var obj = new GameObject(instanceId, scene);
			obj.name = name;
			obj.tag = tag;
			obj.layer = layer;
			obj.SetActive(selfActive);

			foreach (var comp in components) {
				var a = FetchComponent(comp);
				if (a != default(Component)) {
					obj.AddComponent(a);
				}
			}

			var spawned = new List<GameObject>();
			foreach (var child in childs) {
				var spawn = child.Spawn();
				if (child.parentInstanceId > 0)
					spawn.transform.SetParent(((GameObject)Object.instances[child.parentInstanceId]).transform);
				spawned.Add(spawn);
				InitializeTransform(spawn.transform);
			}
			InitializeTransform(obj.transform);
			return obj;
		}
		public void InitializeTransform(Transform to)
		{
			to.localPosition = localPosition;
			to.rotation = localEulerAngle;
			to.localScale = localScale;
		}

		public static SerializableComponent GetComponent(SerializedComponent component)
		{
			var types = Assembly.GetExecutingAssembly().GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(SerializedComponent)));
			foreach (Type mytype in types) {
				if (mytype.ToString() == component.name) {
					return (SerializableComponent)mytype;
				}
			}
			return null;
		}
	
		#region Adding Component
		public static Component FetchComponent(SerializedComponent comp)
		{
			var c = Scripting.Compiler.TryGetType(comp.name);
			var instance = JsonConvert.DeserializeObject(comp.data, c);
			return (Component)instance;
		}
		#endregion
	}
}