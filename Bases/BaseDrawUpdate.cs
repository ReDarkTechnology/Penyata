using System;

namespace Penyata
{
	public class BaseDrawUpdate
	{
		public virtual void Start()
		{
			
		}
		public virtual void Update()
		{
			
		}
		public virtual void Draw()
		{
			
		}
		bool isStarted;
		public void ActualUpdate()
		{
			if(!isStarted)
			{
				Start();
				isStarted = true;
			}else{
				Update();
			}
		}
	}
}
