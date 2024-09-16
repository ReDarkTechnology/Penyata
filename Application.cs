
using System;

namespace Penyata
{
	public static class Application
	{
		public static string productName = "Penyata";
		public static string productCompany = "ReDark Technology";
		public static string executablePath 
		{
			get
			{
				return System.Windows.Forms.Application.ExecutablePath;
			}
		}
		public static string executableDirectory
		{
			get
			{
				return Configuration.GetRidOfLastPath(executablePath);
			}
		}
		
		public static void Run()
		{
			System.Windows.Forms.Application.Run();
		}
		public static void Run(System.Windows.Forms.Form form)
		{
			System.Windows.Forms.Application.Run(form);
		}
		public static void Run(System.Windows.Forms.ApplicationContext context)
		{
			System.Windows.Forms.Application.Run(context);
		}
		public static void Exit()
		{
			System.Windows.Forms.Application.Exit();
		}
	}
}
