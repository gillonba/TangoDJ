using System;
using System.Collections.ObjectModel;

namespace TangoDJ.Library
{
	public class Tanda
	{
		private SongInfo _cortina;
		private SongList _songs;

		public SongInfo Cortina{get{return _cortina;}}
		public Genre G{get;private set;}
		
		public ReadOnlyCollection<SongInfo> Songs{ get{return _songs.Items;}}
		
		public Tanda (Genre g, SongList songs, SongInfo cortina)
		{
			G = g;
			_songs = songs;
			_cortina = cortina;
		}
	}
}

