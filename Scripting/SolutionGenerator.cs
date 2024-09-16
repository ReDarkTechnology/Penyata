using System;
using System.IO;
using Penyata.Tools;
using Penyata.Serialization;

namespace Penyata.Scripting
{
	public static class SolutionGenerator
	{
		public static string format = "Microsoft Visual Studio Solution File, Format Version 12.00\n" +
		"# Visual Studio 2012\n" +
		"# SharpDevelop 5.1\n" +
		"VisualStudioVersion = 12.0.20827.3\n" +
		"MinimumVisualStudioVersion = 10.0.40219.1\n" +
		"Project(\"{0}\") = \"{2}\", \"{2}.csproj\", \"{1}\"\n" +
		"EndProject\n" +
		"Global\n" +
		"	GlobalSection(SolutionConfigurationPlatforms) = preSolution\n" +
		"		Debug|Any CPU = Debug|Any CPU\n" +
		"		Release|Any CPU = Release|Any CPU\n" +
		"	EndGlobalSection\n" +
		"	GlobalSection(ProjectConfigurationPlatforms) = postSolution\n" +
		"		{1}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\n" +
		"		{1}.Debug|Any CPU.Build.0 = Debug|Any CPU\n" +
		"		{1}.Release|Any CPU.ActiveCfg = Release|Any CPU\n" +
		"		{1}.Release|Any CPU.Build.0 = Release|Any CPU\n" +
		"	EndGlobalSection\n" +
		"EndGlobal";
		
		public static string assemblyInfo = "#region Using directives\n" +
		"using System;\n" +
		"using System.Reflection;\n" +
		"using System.Runtime.InteropServices;\n" +
		"#endregion\n" +
		"// General Information about an assembly is controlled through the following\n"+
		"// set of attributes. Change these attribute values to modify the information\n"+
		"// associated with an assembly.\n" +
		"[assembly: AssemblyTitle(\"{0}\")]\n" +
		"[assembly: AssemblyDescription(\"\")]\n" +
		"[assembly: AssemblyConfiguration(\"\")]\n" +
		"[assembly: AssemblyCompany(\"\")]\n" +
		"[assembly: AssemblyProduct(\"{0}\")]\n" +
		"[assembly: AssemblyCopyright(\"Copyright 2021\")]\n" +
		"[assembly: AssemblyTrademark(\"\")]\n" +
		"[assembly: AssemblyCulture(\"\")]\n" +
		"// This sets the default COM visibility of types in the assembly to invisible.\n" +
		"// If you need to expose a type to COM, use [ComVisible(true)] on that type.\n" +
		"[assembly: ComVisible(false)]\n" +
		"[assembly: AssemblyVersion(\"{1}\")]";
		
		public static void Generate (string directory, Project project)
		{
			try {
				if(!Directory.Exists(directory)) Directory.CreateDirectory(directory);
				string[] paths = 
				{
					"Assets",
					"Properties",
					"Library",
					"Temp",
					"ProjectSettings"
				};
				foreach(var path in paths)
				{
					string p = Path.Combine(directory, path);
					if(!Directory.Exists(p)) Directory.CreateDirectory(p);
				}
				
				// Solution
				PropertyGroup projectInfo = project.PropertyGroup[0];
				string fileData = string.Format(format, projectInfo.ProjectTypeGuids, projectInfo.ProjectGuid, "Assembly-CSharp");
				File.WriteAllText(Path.Combine(directory, projectInfo.RootNamespace + ".sln"), fileData);
				
				// Info
				File.WriteAllText(Path.Combine(directory, "Properties", "AssemblyInfo.cs"), string.Format(assemblyInfo, projectInfo.RootNamespace, "1.0.*"));
				
				// CSProj
				project.ItemGroup[0].Reference.Add(new Reference() {
				                                   	HintPath = Path.Combine(Application.executableDirectory, "SDL2-CS.dll"), 
				                                   	Include = "SDL2-CS"
				                                   });
				project.ItemGroup[0].Reference.Add(new Reference() {
				                                   	HintPath = Path.Combine(Application.executableDirectory, "Penyata.dll"), 
				                                   	Include = "Penyata"
				                                   });
				project.ItemGroup[0].Reference.Add(new Reference() {
				                                   	HintPath = Path.Combine(Application.executableDirectory, "NAudio.dll"), 
				                                   	Include = "NAudio"
				                                   });
				project.ItemGroup[0].Reference.Add(new Reference() {
				                                   	HintPath = Path.Combine(Application.executableDirectory, "Newtonsoft.Json.dll"), 
				                                   	Include = "Newtonsoft.Json"
				                                   });
				string projectData = project.ToXmlString();
				projectData = projectData.Replace("utf-16", "utf-8");
				File.WriteAllText(Path.Combine(directory, "Assembly-CSharp" + ".csproj"), projectData, System.Text.Encoding.UTF8);
				
				// Info
				var infpath = Path.Combine(directory, "ProjectSettings", "ProjectInfo.json");
				var pinf = new ProjectInfo();
				pinf.name = projectInfo.RootNamespace;
				pinf.directory = directory;
				pinf.owner = Environment.UserName;
				File.WriteAllText(infpath, pinf.Serialize());
			}
			catch (Exception e)
			{
				Debug.LogError("SolutionGenerator:Generating", e.Message + " => " + e.Source + " => " + e.StackTrace);
			}
		}
	}
}
