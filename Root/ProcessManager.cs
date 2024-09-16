using System;
using System.Windows.Forms;
using Timer = System.Timers.Timer;
using SDL2;

namespace Penyata.Root
{
	public static class ProcessManager
	{
		public static Action OnInitialize;
		public static Action OnUpdate;
		public static Action OnDraw;
		public static Action OnPhysics;
		public static Action OnGizmos;
		public static Action OnDestroy;
		
		public static void InitializeProcess()
		{
			Init();
		}
		public static void InitializeProcess(IntPtr handle)
		{
			Init(handle);
		}
		
		static void Init(IntPtr handle = default(IntPtr))
		{
			if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0) {
				Console.WriteLine("SDL Failed to init.");
				return;
			}
			if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) < 0)
			{
				Console.WriteLine("Fail to initialize SDL_image");
			}
			if(handle == IntPtr.Zero)
				HandleManager.CreateWindow();
			else
				HandleManager.CreateFromHandle(handle);
			RendererManager.InitializeMain(HandleManager.mainHandle.handle);
			OnInitialize.TryInvoke();
			AddCamera();
		}
		
		public static void RunProcess()
		{
			ProcessManager.HandleWindowWithTimer();
			Application.Run();
		}
		
		static bool running = true;
		
		public static Timer timer;
		public static void HandleWindow()
		{
			while (running) {
				Handle();
			}
		}
		public static void HandleWindowWithTimer()
		{
			if(timer == null)
			{
				timer = new Timer();
				timer.Interval = 1;
				//timer.Tick += (e, a) => Handle();
				timer.Elapsed += (e, a) => Handle();
				timer.Start();
			}
		}
		public static void Handle()
		{
			SDL.SDL_Event sdlEvent;
			while (SDL.SDL_PollEvent(out sdlEvent) != 0) {
				if (sdlEvent.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN) {
					Input.AddMouseDown(sdlEvent.button.button);
				}
				if (sdlEvent.type == SDL.SDL_EventType.SDL_KEYDOWN) {
					Input.AddKeyDown(sdlEvent.key.keysym.sym);
				}
				if (sdlEvent.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP) {
					Input.AddMouseUp(sdlEvent.button.button);
				}
				if (sdlEvent.type == SDL.SDL_EventType.SDL_KEYUP) {
					Input.AddKeyUp(sdlEvent.key.keysym.sym);
				}
				if (sdlEvent.type == SDL.SDL_EventType.SDL_QUIT) {
					running = false;
				}
				if(sdlEvent.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
			    {
					Input.mouseWheel = new Vector2(sdlEvent.wheel.x, sdlEvent.wheel.y);
				}
			}
			OnUpdate.TryInvoke();
			OnDraw.TryInvoke();
			RendererManager.RenderAll();
			Drawer.ClearRequests();
			if(!running) Terminate();
		}
		public static void AddCamera()
		{
			var camera = new GameObject();
			camera.name = "MainCamera";
			camera.AddComponent<Camera>();
		}
		public static void TryInvoke(this Action v)
		{
			if(v != null) v.Invoke();
		}
		public static void Terminate()
		{
			OnDestroy.TryInvoke();
			Application.Exit();
		}
	}
}
