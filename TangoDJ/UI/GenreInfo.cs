using Gtk;
using System;

namespace TangoDJ.UI
{
	public class GenreInfo : Gtk.Bin
	{
		private Library.Genre _g;

		public GenreInfo(Library.Genre g) : base()
		{
			_g = g;
		}

		public Table BuildGenreInfo(){
			Table t = new Table(4,2,false);

			Label GN = new Label();
			GN.Text = _g.Name;
			t.Attach(GN,0,1,0,1);

			return t;
		}

		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
		{
			if (this.Child != null)
			{
				this.Child.Allocation = allocation;
			}
		}
		
		protected override void OnSizeRequested (ref Requisition requisition)
		{
			if (this.Child != null)
			{
				requisition = this.Child.SizeRequest ();
			}
		}
	}
}

