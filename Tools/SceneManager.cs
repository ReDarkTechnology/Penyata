using System;
using System.IO;
using System.Collections.Generic;
using Penyata.Serialization;
using Newtonsoft.Json;

namespace Penyata
{
	public static class SceneManager
	{
		public static List<Scene> activeScenes = new List<Scene>();
		public static Scene dontDestroyOnLoad;
		public static List<SerializedScene> scenes = new List<SerializedScene>();
		
		public static void LoadScene(int sceneIndex)
		{
			SerializedScene scene = null;
			foreach (var a in scenes) {
				if (a.index == sceneIndex)
					scene = a;
			}
			if (scene != null) {
				LoadUnpreparedScene(scene, sceneIndex);
			} else {
				Debug.LogError("SceneLoader", "Scene can't be loaded");
			}
		}
		public static void LoadUnpreparedScene(SerializedScene scene, int sceneIndex = -1)
		{
			if (scene != null) {
				if (activeScenes.Count > 0) {
					foreach (var sc in activeScenes) {
						DestroyScene(sc);
					}
					activeScenes.Clear();
				}
				var spScene = new Scene();
				activeScenes.Add(spScene);
				spScene.buildIndex = sceneIndex;
				spScene.name = scene.name;
				foreach (SerializedGameObject obj in scene.rootObjects) {
					spScene.rootObjects.Add(
						obj.Spawn(spScene)
					);
				}
				//Main.mainCamera.transform.position = Vector2.zero;
			} else {
				Debug.LogError("SceneLoader", "Scene can't be loaded");
			}
		}
		public static void DestroyScene(Scene scene)
		{
			foreach (var a in scene.rootObjects) {
				try {
					Component.Destroy(a);
				} catch (Exception e) {
					Debug.LogError("SceneDestroyer", e.Message + e.StackTrace);
				}
			}
			GC.SuppressFinalize(scene);
		}
		public static void PhysicsProcess()
		{
			//Main.mainCamera.gameObject.OnPhysicsUpdate();
			if (activeScenes.Count > 0) {
				foreach (var a in activeScenes) {
					foreach (var obj in a.rootObjects) {
						if (obj.activeSelf) {
							obj.OnPhysicsUpdate();
						}
					}
				}
			}
		}
		public static void CallGizmoUpdate(bool overrideHide = false)
		{
			//Main.mainCamera.gameObject.OnGizmoUpdate(overrideHide);
			if (activeScenes.Count > 0) {
				foreach (var a in activeScenes) {
					foreach (var obj in a.rootObjects) {
						if (obj.activeSelf) {
							obj.OnGizmoUpdate(overrideHide);
						}
					}
				}
			}
		}
	}
}
