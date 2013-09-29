using Gst;
using System;
using System.Linq;

namespace TangoDJ.Player
{
	sealed class Player
	{
		private Library.Library _library;
		private Playlist _nowPlaying;
    	private static Gst.BasePlugins.PlayBin2 _play;

		public Library.Tanda CurrentTanda{get{return _nowPlaying.CurrentTanda;}}
		public Library.SongInfo CurrentSong{get{return _nowPlaying.CurrentSong;}}

		public event EventHandler Playing;
		
		public Player (Library.Library lib)
		{
			_library = lib;
			_nowPlaying = new Playlist();
		}
		
		public void NextSong(){
			Stop ();
			Play ();
			//throw new NotImplementedException();
		}
		public void NextTanda(){
			throw new NotImplementedException();
		}
		public void Pause(){
			throw new NotImplementedException();
		}
		public void Play(){
			//Uri[] songs = new Uri[] {new Uri(PopNext ())};
			if(_play != null){
				Gst.State currentState;
				_play.GetState (out currentState, 1);
				if(currentState == State.Playing) return;
			}

			Advance ();


	    	Gst.Application.Init ();

    		_play = ElementFactory.Make ("playbin2", "play") as Gst.BasePlugins.PlayBin2;

    		if (_play == null) {
      			Console.WriteLine ("error creating a playbin gstreamer object");
	      		return;
    		}

			_play.Uri = new Uri(CurrentSong.Path).AbsoluteUri; //songs[0].AbsoluteUri;
	    	_play.Bus.AddWatch (new BusFunc (BusCb));
    		_play.SetState (Gst.State.Playing);
			if(Playing != null) Playing(this, new EventArgs());
		}
		private void Advance(){
			//throw new NotImplementedException();
			//Library.SongInfo next = _nowPlaying.Dequeue ();
			_nowPlaying.Advance ();
			if(CurrentSong == null){
				//For now, just pick a genre at random from the list.  Re-write this later
				Random rand = new Random();

				//System.Collections.Generic.List<Library.Genre> gl = new System.Collections.Generic.List<Library.Genre>(_library.Genres);
				System.Collections.Generic.List<Library.Genre> gl = 
					(from g in _library.Genres 
					where g.Selectable == true
						select g).ToList();

				Library.Tanda t = null;
				while(t == null) {
					if(gl.Count <= 0) throw new Exception("No genres could be found to generate a song list!");
					try{
						int idx = rand.Next (gl.Count);
						Library.Genre g = gl[idx];
						gl.Remove(g);
						t = _library.GenerateTanda (g);
					}catch(Library.Genre.CouldNotGenerateSongListException){}
				}

				_nowPlaying.AddTanda (t);
				//next = _nowPlaying.Dequeue ();
				//_nowPlaying.Advance ();
			}
			//CurrentSong = next;
			//CurrentTanda = 
			//return next;
		}
		public void Stop(){
			_play.SetState (Gst.State.Null);
			_play = null;
			//throw new NotImplementedException();
		}
		private bool BusCb (Bus bus, Message message) {
    		switch (message.Type) {
      		case Gst.MessageType.Error:
        			Enum err;
        			string msg;
        			message.ParseError (out err, out msg);
        			Console.WriteLine ("Gstreamer error: {0}", msg);
        			break;
      		case Gst.MessageType.Eos:
         			_play.SetState (Gst.State.Null);
					Play ();
        		break;
    		}

    		return true;
  		}
	}
}

