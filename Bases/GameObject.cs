using System;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Penyata
{
	public class GameObject : Object
	{
		[JsonIgnore]
		public bool activeSelf { get; private set; }
		public void SetActive(bool to)
		{
			activeSelf = to;
		}
		[JsonIgnore]
		public string tag = "Untagged";
		[JsonIgnore]
		public int layer;
		[JsonIgnore]
		public bool isDestroyed;
		[JsonIgnore]
		public Transform transform;
		[JsonIgnore]
		public int sceneIndex;
		
		
		public static List<GameObject> registeredObjects = new List<GameObject>();
		public GameObject()
		{
			Init();
			registeredObjects.Add(this);
			InitializeInstanceID(this);
		}
		public GameObject(int instanceID = -1, Scene scene = null)
		{
			Init(scene);
			if(instanceID > 0)
			{
				ChangeInstanceID(instanceID, this);
			}
			registeredObjects.Add(this);
		}
		private void Init(Scene scene = null)
		{
			SetActive(true);
			transform = new Transform(this);
			Root.ProcessManager.OnDraw += OnDraw;
			Root.ProcessManager.OnUpdate += OnUpdate;
			if (scene != null) {
				scene.rootObjects.Add(this);
			} else {
				if (SceneManager.activeScenes.Count > 0) {
					scene = SceneManager.activeScenes[0];
					scene.rootObjects.Add(this);
				}
			}
		}
		public void OnDraw()
		{
			if (activeSelf) {
				int total = components.Count;
				try {
					for (int i = 0; i < total; i++) {
						Component comp = ((Component)components[i]);
						if (comp.enabled)
							comp.OnDraw();
					}
				} catch {
					Debug.LogError(this, "Some component isn't available on draw");
				}
			}
		}
		public void OnUpdate()
		{
			if (activeSelf) {
				int total = components.Count;
				try {
					for (int i = 0; i < total; i++) {
						Component comp = ((Component)components[i]);
						if (comp.enabled)
							comp.ActualUpdate();
					}
				} catch {
					Debug.LogError(this, "Some component isn't available on update");
				}
			}
		}
		public void OnDestroy()
		{
			int total = components.Count;
			for (int i = 0; i < total; i++) {
				Component comp = ((Component)components[i]);
				if (comp.enabled)
					comp.OnDestroy();
			}
			try {
				foreach (var ch in transform.childs) {
					Component.Destroy(ch.gameObject);
				}
			} catch (Exception e) {
				Debug.LogError("ChildDestroyer", e.Message + e.StackTrace);
			}
			registeredObjects.Remove(this);
			isDestroyed = true;
		}
		public void OnGizmoUpdate(bool overrideHide = false)
		{
			int total = components.Count;
			try {
				if (activeSelf) {
					for (int i = 0; i < total; i++) {
						Component comp = ((Component)components[i]);
						if (comp.enabled)
							comp.GizmoUpdate(overrideHide);
					}
				}
				foreach (var obj in transform.childs) {
					obj.gameObject.OnGizmoUpdate(overrideHide);
				}
			} catch {
				Debug.LogError(this, "Some component isn't available on gizmo update");
			}
		}
		public void OnPhysicsUpdate()
		{
			int total = components.Count;
			try {
				if (activeSelf) {
					for (int i = 0; i < total; i++) {
						Component comp = ((Component)components[i]);
						if (comp.enabled)
							comp.CallPhysics();
					}
				}
				foreach (var obj in transform.childs) {
					obj.gameObject.OnPhysicsUpdate();
				}
			} catch {
				Debug.LogError(this, "Some component isn't available on gizmo update");
			}
		}
		
		[JsonIgnore]
		public List<object> components = new List<object>();
		/// <summary>
		/// Adds a component to the GameObject
		/// </summary>
		/// <returns>The spawned component</returns>
		public T AddComponent<T>()
		{
			if (typeof(Component).IsAssignableFrom(typeof(T))) {
				var instance = Activator.CreateInstance(typeof(T));
				AddComponent(instance);
				return (T)instance; 
			}
			return default(T);
		}
		/// <summary>
		/// Get a component in the gameObject
		/// </summary>
		/// <param name="includeInactive">Does it include inactive components?</param>
		/// <returns>The component</returns>
		public T GetComponent<T>(bool includeInactive = false)
		{
			foreach (var obj in components) {
				if (typeof(T).Name == "Transform")
					return ((T)(object)transform);
				if (((Component)obj).enabled || !includeInactive)
				if (obj.GetType().Name == typeof(T).Name)
					return (T)obj;
			}
			return default(T);
		}
		/// <summary>
		/// Get a component in the gameObject
		/// </summary>
		/// <param name="includeInactive">Does it include inactive components?</param>
		/// <returns>The component</returns>
		public T[] GetComponentsInChildren<T>(bool includeInactive = false)
		{
			var cs = new List<T>();
			foreach (var obj in transform.childs) {
				var t = obj.GetComponent<T>();
				try {
					var na = t.GetType().Name;
					cs.Add(t);
				} catch (Exception e) {
					Debug.LogError("ComponentGetter:InChildren", e.Message);
				}
				var cps = obj.gameObject.GetComponentsInChildren<T>(includeInactive);
				foreach (var cp in cps) {
					cs.Add(cp);
				}
			}
			return cs.ToArray();
		}
		/// <summary>
		/// Adds a component into the GameObject
		/// </summary>
		/// <param name="comp">The component</param>
		public void AddComponent(object comp)
		{
			InitializeComponent(comp);
			components.Add(comp);
		}
		public void InitializeComponent(object comp)
		{
			var component = (Component)comp;
			if(comp.GetType().IsSubclassOf(typeof(MonoBehaviour)))
				AddMonoBehaviour(comp);
			component.SetComponentOwner(this);
		}
		/// <summary>
		/// Add mono behaviour to the registry
		/// </summary>
		public static void AddMonoBehaviour(object o)
		{
			MethodInfo[] infos = o.GetType().GetMethods();
			foreach(var a in infos)
			{
				if(a.Name == "Start") ((Component)o).OnStartInvoked += () => a.Invoke(o, null);
				if(a.Name == "Update") ((Component)o).OnUpdateInvoked += () => a.Invoke(o, null);
			}
		}
		/// <summary>
		/// Adds a component into the GameObject with the type's full name and the json data
		/// </summary>
		public void AddComponent(string name, string data)
		{
			var t = Type.GetType(name);
			var component = (Component)JsonConvert.DeserializeObject(data, t);
			components.Add(component);
			component.SetComponentOwner(this);
		}
		/// <summary>
		/// Creates a primitive GameObject
		/// </summary>
		/// <param name="type">The primitive type</param>
		/// <returns>The primitive GameObject</returns>
		public static GameObject CreatePrimitive(PrimitiveType type)
		{
			var obj = new GameObject();
			switch (type) {
				case PrimitiveType.Box:
					obj.AddComponent<BoxRenderer>();
					break;
				case PrimitiveType.FilledBox:
					obj.AddComponent<BoxRenderer>().fill = true;
					break;
				case PrimitiveType.Line:
					obj.AddComponent<LineRenderer>();
					break;
			}
			return obj;
		}
		/// <summary>
		/// Get and if not found, add component type T to the GameObject
		/// </summary>
		/// <returns>A GameObject type T or null if invalid</returns>
		public T GetOrAddComponent<T>()
		{
			var g = GetComponent<T>(true);
			if(EqualityComparer<T>.Default.Equals(g)){
				return AddComponent<T>();
			}
			return g;
		}
		
		/// <summary>
		/// Find a GameObject of type T in the GameObject registry
		/// </summary>
		/// <returns>A GameObject of type T or null</returns>
		public static T FindObjectOfType<T>()
		{
			foreach (var obj in registeredObjects) {
				var comp = obj.GetComponent<T>(true);
				if(!EqualityComparer<T>.Default.Equals(comp)) return comp;
			}
			return default(T);
		}
	}
	public enum PrimitiveType
	{
		Box,
		FilledBox,
		Line
	}
}
