using System;

namespace TangoDJ.Library
{
	/// <summary>
	/// Represents a song on the hard drive.  Additionally, exposes common properties that are useful for categorizing songs
	/// </summary>
	public class SongInfo
	{
		private System.Collections.Generic.Dictionary<string, string> _fields = new System.Collections.Generic.Dictionary<string, string>();

		public string Album		{get;private set;}
		public string Artist	{get;private set;}
		public string BandOrchestraAccompaniment	{get;private set;}
		public string BPM	{get;private set;}
		public string Composer	{get;private set;}
		public string ContentGroupDescription	{get;private set;}
		public string CommercialInformation	{get;private set;}
		public string ContentType	{get;private set;}
		public string CopyrightMessage	{get;private set;}
		public string Date	{get;private set;}
		public string EncodedBy	{get;private set;}
		public string FileType{get; private set;}
		public string Genre		{get;private set;}
		public string InitialKey	{get;private set;}
		public string Language	{get;private set;}
		public string LeadPerformer	{get;private set;}
		public string Length	{get;private set;}
		public string MediaType	{get;private set;}
		public string OfficialArtistPerformerWebpage	{get;private set;}
		public string OfficialAudioFileWebpage		{get;private set;}
		public string OfficialAudioSourceWebpage	{get;private set;}
		public string OriginalArtistPerformer	{get;private set;}
		public string Path	{get;private set;}
		public string Payment	{get;private set;}
		public string SoftwareHardwareAndSettingsUsedForEncoding	{get;private set;}
		public string SubtitleDescriptionRefinement	{get;private set;}
		public string Time	{get;private set;}
		public string Title		{get;private set;}
		public string PartOfSet	{get;private set;}
		public string PublishersWebPage	{get;private set;}
		public string Track	{get;private set;}
		public string Year	{get;private set;}
		
		public SongInfo (string path)
		{
			Path = path;

			try{
				ID3.ID3Info i = new ID3.ID3Info(path, true);

				Album = i.ID3v1Info.Album;
				Artist = i.ID3v1Info.Artist;
				//Genre = i.ID3v1Info.Genre;
				Title = i.ID3v1Info.Title;
				
				
				foreach(ID3.ID3v2Frames.TextFrames.TextFrame tf in i.ID3v2Info.TextFrames){
					if(tf.FrameID == "TALB") Album = tf.Text;
					else if(tf.FrameID == "TBPM") BPM	= tf.Text;
					else if(tf.FrameID == "TCOM") Composer	= tf.Text;
					else if(tf.FrameID == "TCON") ContentType	= tf.Text;
					else if(tf.FrameID == "TCOP") CopyrightMessage	= tf.Text;
					else if(tf.FrameID == "TDAT") Date	= tf.Text;
					else if(tf.FrameID == "TENC") EncodedBy = tf.Text;
					else if(tf.FrameID == "TFLT") FileType = tf.Text;
					else if(tf.FrameID == "TIME") Time	= tf.Text;
					else if(tf.FrameID == "TIT1") ContentGroupDescription = tf.Text;
					else if(tf.FrameID == "TIT2") Title	= tf.Text;
					else if(tf.FrameID == "TIT3") SubtitleDescriptionRefinement	= tf.Text;
					else if(tf.FrameID == "TKEY") InitialKey	= tf.Text;
					else if(tf.FrameID == "TLAN") Language = tf.Text;
					else if(tf.FrameID == "TLEN") Length	= tf.Text;
					else if(tf.FrameID == "TMED") MediaType = tf.Text;
					else if(tf.FrameID == "TOPE") OriginalArtistPerformer	= tf.Text;
					else if(tf.FrameID == "TPE1") LeadPerformer	= tf.Text;
					else if(tf.FrameID == "TPE2") BandOrchestraAccompaniment = tf.Text;
					else if(tf.FrameID == "TPOS") PartOfSet = tf.Text;
					else if(tf.FrameID == "TPUB") PublishersWebPage = tf.Text;
					else if(tf.FrameID == "TRCK") Track	= tf.Text;
					else if(tf.FrameID == "TSSE") SoftwareHardwareAndSettingsUsedForEncoding = tf.Text;
					else if(tf.FrameID == "TYER") Year	= tf.Text;
					else if(tf.FrameID == "WCOM") CommercialInformation	= tf.Text;
					else if(tf.FrameID == "WOAF") OfficialAudioFileWebpage = tf.Text;
					else if(tf.FrameID == "WOAR") OfficialArtistPerformerWebpage = tf.Text;
					else if(tf.FrameID == "WOAS") OfficialAudioSourceWebpage = tf.Text;
					else if(tf.FrameID == "WPAY") Payment = tf.Text;
					else System.Console.WriteLine("Could not handle TextFrame " + tf.FrameID + " for song " + path); //throw new NotImplementedException("Could not handle TextFrame " + tf.FrameID + " for song " + path);
				}
			}catch(System.ArgumentOutOfRangeException aoore){
				System.Console.WriteLine (path + Environment.NewLine + aoore.ToString ());
			}

			
			//Name = System.IO.Path.GetFileNameWithoutExtension (path);
			//throw new NotImplementedException();
		}
	}
}

