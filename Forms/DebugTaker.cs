using System;
using System.Drawing;
using System.Windows.Forms;

namespace Penyata
{
	public partial class DebugTaker : Form
	{
		public DebugTaker()
		{
			InitializeComponent();
			Debug.OnError += Log;
			Debug.OnWarning += Log;
			Debug.OnInfo += Log;
		}
		int logTotal;
		public string target;
		public bool pauseLog;
		public void Log(DebugInfo info)
		{
			if(logTotal > 20){
				target = "";
				logTotal = 0;
			}
			string say = info.sender.ToString();
			if(info.sender.GetType().Name.ToLower() != "string") say = info.sender.GetType().Name;
			target = info.type.ToString("G") + "[" + say + "]"+ ": " + info.arguments.ToString() + Environment.NewLine + target;
			if(!pauseLog) textBox1.Text = target;
			logTotal++;
		}
		void Button1Click(object sender, EventArgs e)
		{
			textBox1.Text = "";
		}
		void DebugTakerFormClosing(object sender, FormClosingEventArgs e)
		{
			WindowForm.SaveWindowConfig(Text, this);
			Debug.OnError -= Log;
			Debug.OnWarning -= Log;
			Debug.OnInfo -= Log;
		}
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			pauseLog = checkBox1.Checked;
		}
		void DebugTakerLoad(object sender, EventArgs e)
		{
			WindowForm.ApplyWindowConfig(Text, this);
		}
	}
}
