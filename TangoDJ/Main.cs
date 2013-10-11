/// <summary>
/// Main class.
/// </summary>
namespace TangoDJ
{
	using System;
	using Gtk;

	/// <summary>
	/// Main class.  This class contains the entry point of the application
	/// </summary>
	public class MainClass
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main(string[] args)
		{
			Library.Library l = new Library.Library();
			
			Application.Init();
			UI.MainWindow win = new UI.MainWindow(l);
			win.Show();
			Application.Run();
		}
	}
}
