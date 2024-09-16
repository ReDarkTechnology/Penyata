using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Penyata
{
	public class Object : IDisposable
	{
		[JsonProperty("instanceId")]
		public int instanceID {get; private set;}
		
		[JsonIgnore]
		private string p_name = "Object";
		
		[JsonIgnore]
		public string name {
			get
			{
				return p_name;
			}
			set
			{
				p_name = value;
			}
		}
		
		public void InitializeInstanceID(object sender)
		{
			if(instanceID < 1) ChangeInstanceID(GetInstanceID(), sender);
		}
		public void ChangeInstanceID(int to, object sender)
		{
			if(!instances.ContainsKey(to)){
				if(instanceID > 0) instances.Remove(to);
				instanceID = to;
				instances.Add(instanceID, sender);
			}
		}
		
		public static Dictionary<int, object> instances = new Dictionary<int, object>();
		public static int maxInstance;
		public static int GetInstanceID()
		{
			maxInstance++;
			return maxInstance;
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
