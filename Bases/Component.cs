using System;
using Newtonsoft.Json;

namespace Penyata
{
	public class Component
	{
		public bool enabled = true;
		[JsonIgnore]
		public string name {
			get {
				return gameObject.name;
			}
			set {
				gameObject.name = value;
			}
		}
		[JsonIgnore]
		public GameObject gameObject { get; private set; }
		[JsonIgnore]
		public Transform transform {
			get {
				return gameObject.transform;
			}
		}
		/// <summary>
		/// This will set the gameObject, usually called by GameObject.AddComponent
		/// </summary>
		/// <param name="to">The component's gameObject</param>
		public void SetComponentOwner(GameObject to)
		{
			gameObject = to;
		}
		[JsonIgnore]
		public Action OnStartInvoked;
		[JsonIgnore]
		public Action OnUpdateInvoked;
		/// <summary>
		/// Called before OnUpdate
		/// </summary>
		public virtual void OnStart()
		{
		}
		/// <summary>
		/// Called after every OnUpdate's update
		/// </summary>
		public virtual void OnPhysics()
		{
			
		}
		/// <summary>
		/// Called every frame
		/// </summary>
		public virtual void OnUpdate()
		{
			
		}
		/// <summary>
		/// Called after OnUpdate
		/// </summary>
		public virtual void OnDraw()
		{
			
		}
		/// <summary>
		/// Called if the component is being destroyed
		/// </summary>
		public virtual void OnDestroy()
		{
			
		}
		/// <summary>
		/// Gizmo update makes everything is drawn on top of OnDraw
		/// </summary>
		public virtual void GizmoUpdate(bool overrideHide = false)
		{
			
		}
		[JsonIgnore]
		bool doneStart;
		/// <summary>
		/// The GameObject will run this in order to run the component
		/// </summary>
		public void ActualUpdate()
		{
			if (!doneStart) {
				OnStart();
				if(OnStartInvoked != null) OnStartInvoked.Invoke();
				doneStart = true;
			} else {
				OnUpdate();
				if(OnUpdateInvoked != null) OnUpdateInvoked.Invoke();
			}
		}
		public void CallPhysics()
		{
			if (doneStart)
				OnPhysics();
		}
		/// <summary>
		/// Get a component in the component's gameObject
		/// </summary>
		/// <param name="includeInactive">Does it include inactive components?</param>
		/// <returns>A GameObject of type T or null</returns>
		public T GetComponent<T>(bool includeInactive = false)
		{
			return gameObject.GetComponent<T>(includeInactive);
		}
		/// <summary>
		/// Find a GameObject of type T in the GameObject registry
		/// </summary>
		/// <returns>A GameObject of type T or null</returns>
		public static T FindObjectOfType<T>()
		{
			return GameObject.FindObjectOfType<T>();
		}
		/// <summary>
		/// Destroy an object
		/// </summary>
		/// <param name="obj">The object you want to destroy</param>
		public static void Destroy(object obj)
		{
			if (obj.HaveAnyInheritanceWith(typeof(GameObject))) {
				var go = ((GameObject)obj);
				go.SetActive(false);
				go.OnDestroy();
				go.components.Clear();
				go.transform = null;
				GameObject.registeredObjects.Remove(go);
				go.name = null;
			} else {
				if (obj.HaveAnyInheritanceWith(typeof(Component))) {
					var c = (Component)obj;
					c.gameObject.components.Remove(obj);
				}
			}
			GC.SuppressFinalize(obj);
		}
	}
}
