using System;
using System.Collections;
using System.Text;

namespace TangoDJ.Library
{
	public class Library
	{
		private System.Collections.Concurrent.ConcurrentBag<Genre> _genreList = new System.Collections.Concurrent.ConcurrentBag<Genre>();
		private SongList _masterList = new SongList();
		private SongList _noGenre = new SongList();
		private System.Threading.Thread _scanningThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Library.Scan));
		
		public Genre[] Genres{ get{ return _genreList.ToArray (); } }
		
		public event EventHandler ScanFinished;
		public event EventHandler ScanStarted;
		
		public Library ()
		{
			CreateGenres ();
		}

		public void AddSong(string f){
			if(f == null) return;

			bool foundGenre = false;

			try{
			SongInfo si = new SongInfo(f);
			_masterList.Add (si);
			foreach(Genre g in _genreList){
				foundGenre |= g.AddIfMemberOfGenre (si);
			}
			if(!foundGenre) _noGenre.Add (si);
			}catch(Exception ex){
				System.Console.WriteLine (ex.ToString ());
			}
		}
		public void CreateGenres(){
			//Everything that doesn't end up in a Genre ends up in "_noGenre"
			Newtonsoft.Json.Linq.JObject o = Newtonsoft.Json.Linq.JObject.Parse(System.IO.File.ReadAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "genres.json")));

			foreach(var go in o["genres"]){
				Genre g = new Genre(go["name"].ToString ());

				if(go.SelectToken ("artistcontains") != null) {
					System.Collections.Generic.List<string> artistContains = new System.Collections.Generic.List<string>();
					foreach(var ac in go["artistcontains"]){
						artistContains.Add (ac.ToString());
					}
					g.ArtistContains = artistContains.ToArray ();
				}
				if(go.SelectToken ("genrecontains") != null){
					System.Collections.Generic.List<string> genreContains = new System.Collections.Generic.List<string>();
					foreach(var gc in go["genrecontains"]){
						genreContains.Add (gc.ToString ());
					}
					g.GenreContains = genreContains.ToArray ();
				}
				if(go.SelectToken ("lasttanda") != null){
					g.LastTanda = bool.Parse (go["lasttanda"].ToString ());
				}
				if(go.SelectToken ("selectable") != null){
					g.Selectable=bool.Parse (go["selectable"].ToString ());
				}
				if(go.SelectToken ("titlecontains") != null){
					System.Collections.Generic.List<string> titleContains = new System.Collections.Generic.List<string>();
					foreach(var gc in go["titlecontains"]){
						titleContains.Add (gc.ToString ());
					}
					g.TitleContains = titleContains.ToArray ();
				}
				_genreList.Add (g);
			}

			//Genre cortina = new Genre("Cortina");
			//cortina.ArtistContains = new string[] {"cortina"};
			//_genreList.Add (cortina);
			
			/*Genre milonga = new Genre("Milonga");
			milonga.GenreContains = new string[] {"milonga"};
			_genreList.Add (milonga);
			
			Genre modern = new Genre("Modern");
			modern.ArtistContains = new string[] {"Piazzolla"};
			_genreList.Add (modern);
			
			Genre notTango = new Genre("Not Tango");
			notTango.GenreContains = new string[] {"audio book", "podcast"};
			_genreList.Add (notTango);
			
			Genre tango = new Genre("Tango");
			tango.GenreContains = new string[]{"tango"};
			_genreList.Add (tango);
			
			Genre vals = new Genre("Vals");
			vals.GenreContains = new string[] {"vals"};
			_genreList.Add (vals);*/
		}
		public Genre GetGenre(string name){
			foreach(Genre g in _genreList){
				if(g.Name.ToLower () == name.ToLower ()) return g;
			}
			return null;
		}
		/// <summary>
		/// Generates a tanda of the given genre
		/// </summary>
		/// <returns>
		/// The generated tanda.
		/// </returns>
		/// <param name='g'>
		/// The genre from which to generate the tanda
		/// </param>
		/// <exception cref='NotImplementedException'>
		/// Is thrown when a requested operation is not implemented for a given type.
		/// </exception>
		public Tanda GenerateTanda(Genre g){
			SongList songs = g.GetMatchedSongs (3);

			Genre gc = GetGenre ("cortina");
			SongList cortina;
			try{
				cortina = gc.GetMatchedSongs(1);
			}catch(Exception){
				throw new Exception("Failed to get a cortina!");
			}
			
			Tanda ret = new Tanda(g, songs,cortina.Items[0]);
			
			return ret;
		}
		/// <summary>
		/// Starts a scan of the computer for new songs for the library
		/// </summary>
		/// <exception cref='NotImplementedException'>
		/// Is thrown when a requested operation is not implemented for a given type.
		/// </exception>
		public void StartScan(){
			if(_scanningThread.ThreadState == System.Threading.ThreadState.Running) return;
			
			_scanningThread.Start (new ScanArgs(this, "/home/"));
		}
		
		private static void Scan(object scanArgs){
			if(scanArgs == null) throw new ArgumentNullException("scanArgs");
			if(!(scanArgs.GetType() == typeof(ScanArgs))) throw new ArgumentException("Expected argument of type scanArgs");
			
			ScanArgs sa = (ScanArgs)scanArgs;
			
			if(sa.Lib.ScanStarted != null) sa.Lib.ScanStarted(sa.Lib, new EventArgs());
			sa.Lib.Scan (sa.Path);
			sa.Lib.WriteNoGenre ();
			if(sa.Lib.ScanFinished != null) sa.Lib.ScanFinished(sa.Lib, new EventArgs());
		}
		private void Scan(string path){
			string[] files = System.IO.Directory.GetFiles (path, "*.mp3", System.IO.SearchOption.TopDirectoryOnly);
			foreach(string f in files){
				AddSong (f);
			}
			
			string[] directories = System.IO.Directory.GetDirectories (path);
			foreach(string d in directories){
				if(!System.IO.Path.GetFileName (d).StartsWith ("."))
					Scan (d);
			}
		}
		/// <summary>
		/// Outputs a text file containing a list of the songs that don't have a genre defined so 
		/// that the user can update the genres.json.  There should be a way to see these in the 
		/// UI
		/// </summary>
		private void WriteNoGenre(){
			System.IO.File.Delete (System.IO.Path.Combine (Environment.CurrentDirectory, "nogenre.csv"));
			System.IO.StreamWriter w = System.IO.File.CreateText (System.IO.Path.Combine (Environment.CurrentDirectory, "nogenre.csv"));
			w.WriteLine ("\"File\",\"Artist\",\"Genre\"");
			foreach(SongInfo si in _noGenre.Items){
				w.WriteLine (string.Format ("\"{0}\",\"{1}\",\"{2}\"", si.Path, si.Artist, si.Genre));
			}
			w.Flush ();
			w.Close ();
		}
		public void WriteNoGenreDetails(){
			Newtonsoft.Json.Linq.JObject o = new Newtonsoft.Json.Linq.JObject();

			if(System.IO.File.Exists (System.IO.Path.Combine(Environment.CurrentDirectory, "nogenredetails.json"))) System.IO.File.Delete (System.IO.Path.Combine(Environment.CurrentDirectory, "nogenredetails.json"));
			foreach(SongInfo si in _noGenre.Items){
				System.IO.File.AppendAllText (System.IO.Path.Combine(Environment.CurrentDirectory, "nogenredetails.json"), si.GetJSON ().ToString ());
				//o.Add (si.GetJSON ());
				//o.Add (si);
			}
			//System.IO.File.WriteAllText (System.IO.Path.Combine(Environment.CurrentDirectory, "nogenredetails.json"), o.ToString ());
		}
	}
}

