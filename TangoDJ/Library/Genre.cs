using System;
using System.Collections.Generic;
using System.Linq;

namespace TangoDJ.Library 
{
	/// <summary>
	/// Provides information and search criteria for the given Genre
	/// </summary>
	public class Genre
	{
		private static object _globalLock = new object();
		private static Random _rand = new Random();
		private SongList _songs = new SongList();
		
		public string[] ArtistContains{get;set;}
		public int		Count{ get { return _songs.Count; } }
		public string[] GenreContains{get;set;}
		public bool		LastTanda{get; set;}
		public string	Name{get;private set;}
		public bool		Selectable{get; set;}
		public string[] TitleContains{get;set;}

		public Genre (string name)
		{
			this.ArtistContains = new string[0];
			this.GenreContains = new string[0];
			this.LastTanda = false;
			this.Name = name;
			this.TitleContains = new string[0];
		}
		public bool		AddIfMemberOfGenre(SongInfo s){
			if(IsMemberOfGenre(s)){
				_songs.Add (s);
				return true;
			}else { return false; }
			//throw new NotImplementedException();
		}
		public SongList GetMatchedSongs(int numSongs){
			List<string> artists = new List<string>(_songs.Artists);
			List<SongInfo> songs = new List<SongInfo>();
			while(songs.Count < numSongs){
				if(artists.Count <= 0) throw new CouldNotGenerateSongListException(); //Exception("No groups could be found that contain " + numSongs + " songs!");
				int idx = RandNext (artists.Count);
				string artist = artists[idx]; //_songs.Artists[RandNext (_songs.Artists.Count)];
				artists.Remove (artist);
				songs = (from s in _songs.Items 
						where s.Artist == artist
						orderby Guid.NewGuid()
				         select s).Take(numSongs).ToList();

			}

			SongList ret = new SongList();
			ret.AddRange (songs);
			return ret;


			//throw new NotImplementedException();
		}
		public bool 	IsMemberOfGenre(SongInfo s){
			foreach(string ac in ArtistContains){
				if(s.Artist != null && s.Artist.ToLower ().Contains (ac.ToLower ())) return true;
			}
			
			foreach(string gc in GenreContains){
				if(s.ContentType != null && s.ContentType.ToLower ().Contains (gc.ToLower ())) return true;
			}

			foreach(string tc in TitleContains){
				if(s.Title != null && s.Title.ToLower ().Contains (tc.ToLower ())) return true;
			}
			
			return false;
		}
		private static int RandNext(){
			lock(_globalLock){
				return _rand.Next ();
			}
		}
		private static int RandNext(int maxValue){
			lock(_globalLock){
				return _rand.Next(maxValue);
			}
		}
		private static int RandNext(int minValue, int maxValue){
			lock(_globalLock){
				return _rand.Next (minValue, maxValue);
			}
		}

		public class CouldNotGenerateSongListException : Exception {

		}
	}
}

