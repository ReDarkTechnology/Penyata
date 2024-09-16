using System;
using System.Collections;

namespace Penyata
{
	public class MonoBehaviour : Component
	{
		public void StartCoroutine(IEnumerator method)
		{
			CoroutineProcess.StartCoroutine(method);
		}
	}
}