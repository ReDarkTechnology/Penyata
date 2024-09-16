using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CSharp;

namespace Penyata.Scripting
{
	public static class Compiler
	{
		#region Main
		public static Assembly compiledAssembly;
		public static Type[] assemblyTypes;
		public static Dictionary<string, Type> assemblyDictionary = new Dictionary<string, Type>();
		public static List<Type> componentTypes = new List<Type>();
		public static Assembly Compile(string directory, Project project)
		{
			var provider = new CSharpCodeProvider();
			var parameters = new CompilerParameters();

			// Reference to libraries
			foreach (var refer in project.ItemGroup[0].Reference) {
				if (string.IsNullOrWhiteSpace(refer.HintPath)) {
					parameters.ReferencedAssemblies.Add(refer.Include + ".dll");
				} else {
					string path = Path.Combine(directory, refer.HintPath);
					if (File.Exists(path))
						parameters.ReferencedAssemblies.Add(path);
					else {
						path = refer.HintPath;
						if (File.Exists(path))
							parameters.ReferencedAssemblies.Add(path);
						else {
							path = Path.GetFileName(refer.HintPath);
							if (File.Exists(path))
								parameters.ReferencedAssemblies.Add(path);
						}
					}
				}
			}
			// True - memory generation, false - external file generation
			parameters.GenerateInMemory = false;
			// True - exe file generation, false - dll file generation
			parameters.GenerateExecutable = false;
			string dir = Path.Combine(directory, "Library");
			var t = GetFreeAssemblyPath(dir, project.PropertyGroup[0].AssemblyName, 0);
			parameters.OutputAssembly = t;
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
            
			var codes = new List<string>();
			var paths = new List<string>();
			var items = project.ItemGroup[1];
           	
			if (items.Compile != null) {
				foreach (Compile item in items.Compile) {
					string path = Path.Combine(directory, item.Include);
					if (File.Exists(path)) {
						if (!string.IsNullOrWhiteSpace(item.DependentUpon)) {
							int index = LookForPath(paths, item.DependentUpon);
							if (index > -1)
								codes[index] += File.ReadAllText(path);
						} else {
							string build = File.ReadAllText(path);
							codes.Add(build);
							paths.Add(build);
						}
					}
				}
			}
			compiledAssembly = null;
			componentTypes.Clear();
			assemblyDictionary.Clear();
			CompilerResults results = provider.CompileAssemblyFromSource(parameters, codes.ToArray());
			provider.Dispose();
			if (results.Errors.HasErrors) {
				foreach (CompilerError error in results.Errors) {
					Debug.LogError("Compiler:Compiling", String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
				}
			} else {
				Assembly assembly = results.CompiledAssembly;
				compiledAssembly = assembly;
				AnalyzeAssembly();
				return assembly;
			}
			return null;
		}
		public static Type TryGetType(string name)
		{
			try 
			{
				var t = Type.GetType(name);
				if(t != null)
				{
					return t;
				}
				else
				{
					if(assemblyDictionary.ContainsKey(name)) return assemblyDictionary[name];
				}
			}
			catch
			{
				if(assemblyDictionary.ContainsKey(name)) return assemblyDictionary[name];
			}
			return null;
		}
		public static int LookForPath(List<string> list, string fileName)
		{
			for (int i = 0; i < list.Count; i++) {
				if (Path.GetFileName(list[i]) == fileName)
					return i;
			}
			return -1;
		}
		static Type[] owo = {
			typeof(BoxCollider),
			typeof(BoxRenderer),
			typeof(SpriteRenderer),
			typeof(LineRenderer),
			typeof(TextRenderer),
			typeof(DragableObject)
		};
		public static void AnalyzeAssembly()
		{
			componentTypes.Clear();
			assemblyDictionary.Clear();
			assemblyTypes = compiledAssembly.GetTypes();
			foreach (var a in assemblyTypes) {
				assemblyDictionary.Add(a.FullName, a);
				if (a.IsSubclassOf(typeof(MonoBehaviour)))
					componentTypes.Add(a);
			}
			foreach (var t in owo) {
				componentTypes.Add(t);
			}
		}
		#endregion
		#region Testing
		public static void TestMethods()
		{
			MethodInfo function = CreateFunction("x + 2 * y");
			var betterFunction = (Func<double, double, double>)Delegate.CreateDelegate(typeof(Func<double, double, double>), function);
			Func<double, double, double> lambda = (x, y) => x + 2 * y;

			DateTime start;
			DateTime stop;
			double result;
			const int repetitions = 5000000;

			start = DateTime.Now;
			for (int i = 0; i < repetitions; i++) {
				result = OriginalFunction(2, 3);
			}
			stop = DateTime.Now;
			Console.WriteLine("Original - time: {0} ms", (stop - start).TotalMilliseconds);

			start = DateTime.Now;
			for (int i = 0; i < repetitions; i++) {
				result = (double)function.Invoke(null, new object[] { 2, 3 });
			}
			stop = DateTime.Now;
			Console.WriteLine("Reflection - time: {0} ms", (stop - start).TotalMilliseconds);

			start = DateTime.Now;
			for (int i = 0; i < repetitions; i++) {
				result = betterFunction(2, 3);
			}
			stop = DateTime.Now;
			Console.WriteLine("Delegate - time: {0} ms", (stop - start).TotalMilliseconds);

			start = DateTime.Now;
			for (int i = 0; i < repetitions; i++) {
				result = lambda(2, 3);
			}
			stop = DateTime.Now;
			Console.WriteLine("Lambda - time: {0} ms", (stop - start).TotalMilliseconds);
		}

		public static double OriginalFunction(double x, double y)
		{
			return x + 2 * y;
		}

		public static MethodInfo CreateFunction(string function)
		{
			const string code = @"
                using System;
            
                namespace UserFunctions
                {                
                    public class BinaryFunction
                    {                
                        public static double Function(double x, double y)
                        {
                            return func_xy;
                        }
                    }
                }
            ";

			string finalCode = code.Replace("func_xy", function);

			var provider = new CSharpCodeProvider();
			CompilerResults results = provider.CompileAssemblyFromSource(new CompilerParameters(), finalCode);

			Type binaryFunction = results.CompiledAssembly.GetType("UserFunctions.BinaryFunction");
			return binaryFunction.GetMethod("Function");
		}
		public static bool IsFileLocked(FileInfo file)
		{
		    try
		    {
		        using(FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
		        {
		            stream.Close();
		        }
		    }
		    catch (IOException)
		    {
		        return true;
		    }
		    return false;
		}
		public static string GetFreeAssemblyPath(string dir, string name, int attempt)
		{
			string t = null;
			if(attempt > 0){
				t = Path.Combine(dir, name + attempt.ToString() + ".dll");
				if(File.Exists(t)) 
					if (IsFileLocked(new FileInfo(t)))
					    t = GetFreeAssemblyPath(dir, name, attempt + 1);
			}else{
				t = Path.Combine(dir, name + ".dll");
				if(File.Exists(t))
					if (IsFileLocked(new FileInfo(t)))
						t = GetFreeAssemblyPath(dir, name, attempt + 1);
			}
			return t;
		}
		#endregion
	}
}
