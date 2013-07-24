using System;
using Gtk;

namespace TangoDJ
{
	class MainClass
	{		
		public static void Main (string[] args)
		{
			Library.Library l = new Library.Library();
			
			Application.Init ();
			UI.MainWindow win = new UI.MainWindow (l);
			win.Show ();
			Application.Run ();
		}
	}
}
