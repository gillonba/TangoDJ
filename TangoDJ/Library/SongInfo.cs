using System;
using System.Collections.Generic;

namespace TangoDJ.Library
{
	/// <summary>
	/// Represents a song on the hard drive.  Additionally, exposes common properties that are useful for categorizing songs
	/// </summary>
	public class SongInfo
	{
		public static readonly Dictionary<string, string> _fieldMap = new Dictionary<string, string>() {
			{ "TALB", "Album" },
			{ "TBPM", "BPM" },
			{ "TCOM", "Composer" },
			{ "TCON", "Content Type" },
			{ "TCOP", "Copyright message" },
			{ "TDAT", "Date" },
			{ "TDEN", "Encoding time" },
			{ "TDRC", "Recording time" },
			{ "TDRL", "Release time" },
			{ "TENC", "Encoded by" },
			{ "TFLT", "File type" },
			{ "TIME", "Time" },
			{ "TIT1", "Content group description" },
			{ "TIT2", "Title/songname/content description" },
			{ "TIT3", "Subtitle/Description refinement" },
			{ "TKEY", "Initial key" },
			{ "TLAN", "Language(s)" },
			{ "TLEN", "Length" },
			{ "TMED", "Media Type" },
			{ "TOPE", "Original artist(s)/performer(s)" },
			{ "TPE1", "Lead performer(s)/Soloist(s)" },
			{ "TPE2", "Band/orchestra/accompaniment" },
			{ "TPE3", "Conductor/performer refinement" },
			{ "TPOS", "Part of a set" },
			{ "TPUB", "Publisher" },
			{ "TRCK", "Track number/Position in set" },
			{ "TSIZ", "Size" },
			{ "TSSE", "Software/Hardware and settings used for encoding" },
			{ "TYER", "Year" },
			{ "WCOM", "Commercial information" },
			{ "WOAF", "Official audio file webpage" },
			{ "WOAR", "Official artist/performer webpage" },
			{ "WOAS", "Official audio source webpage" },
			{ "WPAY", "Payment" }
		};
		private System.Collections.Generic.Dictionary<string, string> _fieldValues = new System.Collections.Generic.Dictionary<string, string>();

		public string Album		{get{return _fieldValues["TALB"];} private set{ _fieldValues.Add ("TALB", value);}}
		public string Artist	{
			get{
				if(_fieldValues.ContainsKey ("TOPE") && !String.IsNullOrWhiteSpace (_fieldValues["TOPE"]))
					return _fieldValues["TOPE"];
				else if(_fieldValues.ContainsKey ("TPE2") && !String.IsNullOrWhiteSpace (_fieldValues["TPE2"]))
					return _fieldValues["TPE2"];
				else if(_fieldValues.ContainsKey ("TPE1") && !String.IsNullOrWhiteSpace (_fieldValues["TPE1"]))
					return _fieldValues["TPE1"];
				else
					return "";
			} 
			//TODO: Remove set
			private set{ _fieldValues.Add ("TOPE", value);}
		}
		public string ContentType	{get{return (_fieldValues.ContainsKey ("TCON")) ? _fieldValues["TCON"] : "";} private set{ _fieldValues.Add ("TCON", value);}}
		public Dictionary<string,string>.KeyCollection FieldKeys	{get{return _fieldValues.Keys;}}
		public Dictionary<string,string>.ValueCollection FieldValues	{get{return _fieldValues.Values;}}
		public string Genre		{get{return "";}}
		public string LeadPerformer	{get{return _fieldValues["TPE1"];} private set{ _fieldValues.Add ("TPE1", value);}}
		public string Path		{get; private set;}
		public string Title		{get{return _fieldValues["TIT2"];} private set{ _fieldValues.Add ("TIT2", value);}}

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
					if(_fieldMap.ContainsKey(tf.FrameID)){
						if(_fieldValues.ContainsKey (tf.FrameID)) _fieldValues[tf.FrameID] = tf.Text; else _fieldValues.Add (tf.FrameID, tf.Text);
					} else {
						System.Console.WriteLine("Could not handle TextFrame " + tf.FrameID + " for song " + path);
					}
				}
				
				/*foreach(ID3.ID3v2Frames.TextFrames.TextFrame tf in i.ID3v2Info.TextFrames){
					if(tf.FrameID == "TALB") Album = tf.Text;
					else if(tf.FrameID == "TBPM") BPM	= tf.Text;
					else if(tf.FrameID == "TCOM") Composer	= tf.Text;
					else if(tf.FrameID == "TCON") ContentType	= tf.Text;
					else if(tf.FrameID == "TCOP") CopyrightMessage	= tf.Text;
					else if(tf.FrameID == "TDAT") Date	= tf.Text;
					else if(tf.FrameID == "TDEN") EncodingTime = tf.Text;
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
				} */
			}catch(System.ArgumentOutOfRangeException aoore){
				System.Console.WriteLine (path + Environment.NewLine + aoore.ToString ());
			}

			
			//Name = System.IO.Path.GetFileNameWithoutExtension (path);
			//throw new NotImplementedException();
		}

		public Newtonsoft.Json.Linq.JObject GetJSON(){
			Newtonsoft.Json.Linq.JObject ret = new Newtonsoft.Json.Linq.JObject();

			ret.Add ("Path", Path);

			foreach(string k in _fieldValues.Keys){
				ret.Add (k, _fieldValues[k]);
			}

			return ret;
			//throw new NotImplementedException();
		}
	}
}

