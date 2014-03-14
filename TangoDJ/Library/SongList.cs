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
		/// <summary>
		/// An index of all the artists in the song list
		/// </summary>
		private readonly IList<string> _idxArtists = new List<string>();

		/// <summary>
		/// The locking variable used for non thread-safe functions
		/// </summary>
		private readonly object _lock = new object();

		/// <summary>
		/// The _readonly artists.
		/// </summary>
		private ReadOnlyCollection<string> _readonlyArtists;

		/// <summary>
		/// The _readonly songs.
		/// </summary>
		private ReadOnlyCollection<SongInfo> _readonlySongs;

		/// <summary>
		/// The backing variable containing all the songs
		/// </summary>
		private readonly IList<SongInfo> _songs = new List<SongInfo>();

		/// <summary>
		/// Gets the artists.
		/// </summary>
		/// <value>
		/// The artists.
		/// </value>
		public ReadOnlyCollection<string> Artists
		{
			get
			{
				if(this._readonlyArtists == null) { _readonlyArtists = new ReadOnlyCollection<string>(_idxArtists);}
				return this._readonlyArtists;
			}
		}

		/// <summary>
		/// Gets the number of songs in the list
		/// </summary>
		/// <value>
		/// The count.
		/// </value>
		public int Count
		{ 
			get { return this._songs.Count; } 
		}

		public ReadOnlyCollection<SongInfo> Items
		{
			get
			{
				if(this._readonlySongs == null) { this._readonlySongs = new ReadOnlyCollection<SongInfo>(_songs); }
				return this._readonlySongs;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TangoDJ.Library.SongList"/> class.
		/// </summary>
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
		public void Add(SongInfo s)
		{
			lock(_lock)
			{
				_songs.Add (s);
				if(!_idxArtists.Contains (s.Artist)){	_idxArtists.Add (s.Artist);	}
			}
		}

		public void AddRange(IEnumerable<SongInfo> songs)
		{
			lock(_lock)
			{
				foreach(SongInfo s in songs) { this.Add(s); }
			}
		}

		private void ClearIndexes()
		{
			throw new NotImplementedException();
		}

		public SongInfo[] GetByArtist(string artist)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Rebuilds the index of the artist.
		/// </summary>
		private void RebuildArtistIndex()
		{
			throw new NotImplementedException();
		}
	}
}