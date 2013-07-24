using Gtk;
using System;

namespace TangoDJ
{
	public class SongInfo : Gtk.Bin
	{
		Label LArtist	= new Label();
		Label LGenre	= new Label();
		Label LLeadPerformer	= new Label();
		Label LName		= new Label();

		public SongInfo () : base()
		{

		}

		public Table BuildSongInfo(){
			Table t = new Table(4,2,false);
			//Label LA;
			//Label LN;

			Label LN = new Label();
			LN.Text = "Name";
			t.Attach (LN,0,1,0,1);

			t.Attach (LName,1,2,0,1);

			Label LA = new Label();
			LA.Text = "Artist";
			t.Attach (LA,0,1,1,2);

			t.Attach (LArtist, 1, 2, 1, 2);

			Label LLP = new Label();
			LLP.Text = "Lead Performer";
			t.Attach (LLP,0,1,2,3);

			t.Attach (LLeadPerformer, 1,2,2,3);

			Label LG = new Label();
			LG.Text = "Genre";
			t.Attach (LG,0,1,3,4);

			t.Attach (LGenre,1,2,3,4);

			//this.Add (t);
			//t.
			
			t.ShowAll ();
			//this.ShowAll ();
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

		public void SetSongInfo(TangoDJ.Library.SongInfo si){
			LArtist.Text 		= si.Artist;
			LName.Text 			= si.Title;
			LLeadPerformer.Text = si.LeadPerformer;
			LGenre.Text 		= si.Genre;
		}
	}
}

