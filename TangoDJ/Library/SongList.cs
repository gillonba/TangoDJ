using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TangoDJ.Library 
{
	/// <summary>
	/// Represents a list of songs.  This is a utility class
	/// </summary>
	public class SongList
	{
		
		private readonly IList<string> _idxArtists = new List<string>();
		//private System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentBag<SongInfo>> _idxArtistSongs = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentBag<SongInfo>>();
		private readonly object _lock = new object();
		private ReadOnlyCollection<string> _readonlyArtists;
		private ReadOnlyCollection<SongInfo> _readonlySongs;
		private readonly IList<SongInfo> _songs = new List<SongInfo>();

		public ReadOnlyCollection<string> Artists{
			get{
				if(_readonlyArtists == null) { _readonlyArtists = new ReadOnlyCollection<string>(_idxArtists);}
				return _readonlyArtists;
			}
		}
		public int Count{get{return _songs.Count;}}
		public ReadOnlyCollection<SongInfo> Items{
			get{
				if(_readonlySongs == null) { _readonlySongs = new ReadOnlyCollection<SongInfo>(_songs);	}
				return _readonlySongs;
			}
		}
		
		public SongList ()
		{
		}
		
		/// <summary>
		/// Add the specified SongInfo object to the end of the list.
		/// </summary>
		/// <param name='s'>
		/// The song to be added to the list.
		/// </param>
		/// <exception cref='NotImplementedException'>
		/// Is thrown when a requested operation is not implemented for a given type.
		/// </exception>
		public void Add(SongInfo s){
			//throw new NotImplementedException();
			lock(_lock){
				_songs.Add (s);
				if(!_idxArtists.Contains (s.Artist)){	_idxArtists.Add (s.Artist);	}

				//ClearIndexes ();
			}
		}
		public void AddRange(IEnumerable<SongInfo> songs){
			lock(_lock){
				foreach(SongInfo s in songs){Add (s);}
			}
		}
		private void ClearIndexes(){
			throw new NotImplementedException();
			//_idxArtistSongs = null;
		}
		public SongInfo[] GetByArtist(string artist){
			throw new NotImplementedException();
			//if(_idxArtistSongs == null) RebuildArtistIndex ();
			//return _idxArtistSongs[artist].ToArray ();
		}
		private void RebuildArtistIndex(){
			throw new NotImplementedException();
			//foreach(SongInfo si in _songs){
			//	if(!_idxArtistSongs.ContainsKey (si.Artist)){
			//		_idxArtistSongs.AddOrUpdate (si.Artist, new System.Collections.Concurrent.ConcurrentBag<SongInfo>);
			//	}
			//}
		}
	}

}

