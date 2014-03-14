using System;
using Gtk;
namespace TangoDJ.UI{
public partial class MainWindow: Gtk.Window
{
		private const uint id = 1;
		private Library.Library lib;
		private readonly Player.Player player;
		private Statusbar sb;
		private SongInfo si;
		private Label LblStatus;
		
		public MainWindow (Library.Library l): base (Gtk.WindowType.Toplevel)
		{ 
			lib = l;
			this.player = new Player.Player(lib);

			this.Title = "Tango DJ";
			
			SetDefaultSize(450, 200);
        	SetPosition(WindowPosition.Center);
        	DeleteEvent += delegate { Application.Quit(); };

			VBox win = new VBox(false, 2);
			win.PackStart(BuildMenu(), false, false, 0);

			HBox cols = new HBox(false, 2);

			VBox vbox = new VBox(false, 2);
        	//vbox.PackStart(BuildMenu (), false, false, 0);
			Table player = BuildPlayer();
			vbox.Add(player);
			//Table si = BuildSongInfo ();
			//vbox.Add (si);
			si = new SongInfo();
			vbox.Add(si.BuildSongInfo());

			cols.Add(vbox);
			foreach(Library.Genre g in l.Genres){
				if(g.Selectable){
					UI.GenreInfo gi = new GenreInfo(g);
					cols.Add(gi.BuildGenreInfo());
				}
			}

			//vbox.Add (new SongInfo());
			win.Add(cols);

			sb = BuildStatusBar();
			win.Add(sb);

        	//Add(vbox);
			Add(win);

        	ShowAll();
			
			lib.ScanFinished+= Lib_ScanFinished;
			lib.ScanStarted += Lib_ScanStarted;
			this.player.Playing += Player_Playing;
			//Build ();

			this.lib.StartScan();
		}
		
		public MenuBar BuildMenu(){
			MenuBar mb = new MenuBar();

        	Menu filemenu = new Menu();
        	MenuItem file = new MenuItem("File");
        	file.Submenu = filemenu;
       
        	MenuItem exit = new MenuItem("Exit");
        	exit.Activated += OnExit;
        	filemenu.Append(exit);
			
			Menu editmenu = new Menu();
			MenuItem edit = new MenuItem("Library");
			edit.Submenu = editmenu;
			
			MenuItem scan = new MenuItem("Scan");
			scan.Activated += OnScan;
			editmenu.Append (scan);

			MenuItem noGenreDetails = new MenuItem("No Genre Details");
			noGenreDetails.Activated += OnNoGenreDetails;
			editmenu.Append (noGenreDetails);

        	mb.Append(file);
			mb.Append(edit);
			
			return mb;
		}
		public Table BuildPlayer(){
			Table ret = new Table(2, 4, false);

			LblStatus = new Label();
			LblStatus.Text  = "Not playing";
			ret.Attach (LblStatus, 0, 3, 0, 1);

			Button BtnPlay = new Button("Play");
			BtnPlay.Clicked += new EventHandler(BtnPlay_Clicked);
			ret.Attach (BtnPlay, 0,1,1,2);

			Button BtnPause = new Button("Pause");
			BtnPause.Clicked += new EventHandler(BtnPause_Clicked);
			ret.Attach (BtnPause, 1,2,1,2);

			Button BtnNextSong = new Button("Next Song");
			BtnNextSong.Clicked += new EventHandler(BtnNextSong_Clicked);
			ret.Attach (BtnNextSong, 2,3,1,2);

			Button BtnNextTanda = new Button("NextTanda");
			BtnNextTanda.Clicked += new EventHandler(BtnNextTanda_Clicked);
			ret.Attach (BtnNextTanda, 3,4,1,2);

			ret.ShowAll ();
			return ret;
		}
		public Table BuildSongInfo(){
			Table t = new Table(4,2,false);
			Label LblName;

			LblName = new Label();
			LblName.Text = "Name";
			t.Attach (LblName,0,1,0,1);
			
			//this.Add (t);
			//t.
			
			t.ShowAll ();
			//this.ShowAll ();
			return t;
		}
		public Statusbar BuildStatusBar(){
			
			Statusbar sb = new Statusbar ();
			
			sb.Push (id, "Welcome!");
			sb.HasResizeGrip = true;
			
			return sb;
		}

		protected void BtnNextSong_Clicked(object obj, EventArgs e){
			player.NextSong ();
		}
		protected void BtnNextTanda_Clicked(object obj, EventArgs e){
			player.NextTanda ();
		}
		protected void BtnPause_Clicked(object obj, EventArgs e){
			player.Pause ();
		}
		protected void BtnPlay_Clicked(object obj, EventArgs e){
			//lib.Play();
			player.Play();
		}
		protected void Lib_ScanFinished(object sender, EventArgs e){
			sb.Pop(id);
			sb.Push (id, "Scan finished");
		}
		protected void Lib_ScanStarted(object sender, EventArgs e){
			sb.Pop (id);
			sb.Push (id, "Scanning...");
		}
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
		protected void OnExit(object sender, EventArgs e){
			Application.Quit ();
		}
		protected void OnNoGenreDetails(object sender, EventArgs e){
			this.lib.WriteNoGenreDetails ();
		}
		protected void OnScan(object sender, EventArgs e){
			//sb.Pop (id);
			//sb.Push (id, "Scanning...");
			//Library.Library.Scan(new Library.ScanArgs(this.lib, "/~"));
			this.lib.StartScan();
		}
		protected void Player_Playing(object sender, EventArgs e){
			LblStatus.Text = player.CurrentTanda.G.Name + " - " + player.CurrentSong.Title;
			//LblStatus.Text = player.CurrentSong.Title;
			si.SetSongInfo (player.CurrentSong);
		}
	}
}
