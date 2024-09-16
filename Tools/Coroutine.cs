using System;
using System.Collections;
using System.Collections.Generic;

namespace Penyata
{ 
	public sealed class Coroutine : CustomYieldInstruction
    {
        private readonly IEnumerator routine;
 
        public Coroutine(IEnumerator routine)
        {
            this.routine = routine;
        }
 
        public override bool GetKeepWaiting()
        {
            return routine.MoveNext();
        }
    }
 
    public class WaitForMouseDown : CustomYieldInstruction
    {
        private readonly MouseCode mouseCode;
        public WaitForMouseDown(MouseCode mouseCode)
        {
            this.mouseCode = mouseCode;
        }
 
        public override bool GetKeepWaiting()
        {
            return !Input.GetMouseButtonDown(mouseCode);
        }
    }
    public class WaitForSeconds : CustomYieldInstruction
    {
        private readonly float finishTime;
        public WaitForSeconds(float seconds)
        {
            finishTime = Time.time + seconds;
        }
 
        public override bool GetKeepWaiting()
        {
            return finishTime > Time.time;
        }
    }
 
    public abstract class CustomYieldInstruction : Object, IEnumerator
    {
    	private object _cur = null;
    	public object Current
    	{
    		get
    		{
    			return _cur;
    		}
    	}
 
        public abstract bool GetKeepWaiting();
        public bool MoveNext()
        {
            return GetKeepWaiting();
        }
 
        public void Reset()
        {
        }
        
    }
	public class CoroutineProcess : BaseDrawUpdate {
	
		private static List<IEnumerator> coroutines = new List<IEnumerator>();	
		
		public override void Update()
		{
			HandleCoroutines();
		}
		
		private void HandleCoroutines()
		{
			for (int i = 0; i < coroutines.Count; i++)
			{
				var cur = coroutines[i].Current;
				bool yielded = (cur is CustomYieldInstruction);
				if(yielded)
				{
					var c = cur as CustomYieldInstruction;
					yielded = c.MoveNext();
					if (yielded)
					{
						continue;
					}
				}
				coroutines.RemoveAt(i);
				i--;
			}
		}
		
		public static Coroutine StartCoroutine(IEnumerator method)
		{
			coroutines.Add(method);
			return new Coroutine(method);
		}
	}
}
