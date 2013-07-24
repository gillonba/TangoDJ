using System;

namespace TangoDJ
{
	public class Playlist
	{
		private int _currentSongIndex = 0;
		private int _currentTandaIndex = 0;
		private System.Collections.Concurrent.ConcurrentDictionary<int, Library.Tanda> _tandaList = new System.Collections.Concurrent.ConcurrentDictionary<int, Library.Tanda>();
		
		public Library.SongInfo CurrentSong{ get{
				if(CurrentTanda == null) return null;
				if(_currentSongIndex >= CurrentTanda.Songs.Count) return CurrentTanda.Cortina;
				else return CurrentTanda.Songs[_currentSongIndex];
			}}
		public Library.Tanda CurrentTanda{ 
			get{
				if(_currentTandaIndex >= _tandaList.Count) return null;
				return _tandaList[_currentTandaIndex]; 
			} }
		
		public Playlist ()
		{
		}
		
		public void AddNextTanda(Library.Tanda t){
			throw new NotImplementedException();
		}
		public void AddTanda(Library.Tanda t){
			_tandaList.TryAdd (_tandaList.Count, t);
			//throw new NotImplementedException();
		}
		public void AdvanceSong(){
			throw new NotImplementedException();
		}
		public void AdvanceTanda(){
			throw new NotImplementedException();
		}
		public void Advance(){
			if(CurrentTanda == null) return;
			_currentSongIndex++;
			if(_currentSongIndex > CurrentTanda.Songs.Count){
				_currentSongIndex = 0;
				_currentTandaIndex++;
			}
		}
		/*public Library.SongInfo Peek(){
			Library.Tanda t;

			if(_currentTandaIndex >= _tandaList.Count) return null;	//This should never actually happen...
			t = _tandaList[_currentTandaIndex];
			return t.Songs[_currentSongIndex];
		}*/
	}
}

