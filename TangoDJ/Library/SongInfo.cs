namespace TangoDJ.Library
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents a song on the hard drive.  Additionally, exposes common properties that are useful for categorizing songs
	/// </summary>
	public class SongInfo
	{
		/// <summary>
		/// This dictionary contains all valid ID3v2 tag codes along with their names
		/// </summary>
		public static readonly Dictionary<string, string> FieldMap = new Dictionary<string, string>() 
		{
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
			{ "TEXT", "Lyricist/Text writer" },
			{ "TFLT", "File type" },
			{ "TIME", "Time" },
			{ "TIT1", "Content group description" },
			{ "TIT2", "Title/songname/content description" },
			{ "TIT3", "Subtitle/Description refinement" },
			{ "TKEY", "Initial key" },
			{ "TLAN", "Language(s)" },
			{ "TLEN", "Length" },
			{ "TMED", "Media Type" },
			{ "TOAL", "Original album/movie/show title" },
			{ "TOLY", "Original lyricist(s)/text writer(s)" },
			{ "TOPE", "Original artist(s)/performer(s)" },
			{ "TPE1", "Lead performer(s)/Soloist(s)" },
			{ "TPE2", "Band/orchestra/accompaniment" },
			{ "TPE3", "Conductor/performer refinement" },
			{ "TPE4", "Interpreted, remixed, or otherwise modified by" },
			{ "TPOS", "Part of a set" },
			{ "TPUB", "Publisher" },
			{ "TRCK", "Track number/Position in set" },
			{ "TRSN", "Internet Radio Station Name" },
			{ "TSIZ", "Size" },
			{ "TSSE", "Software/Hardware and settings used for encoding" },
			{ "TYER", "Year" },
			{ "WCOM", "Commercial information" },
			{ "WOAF", "Official audio file webpage" },
			{ "WOAR", "Official artist/performer webpage" },
			{ "WOAS", "Official audio source webpage" },
			{ "WORS", "Official Internet radio station homepage" },
			{ "WPAY", "Payment" }
		};

		/// <summary>
		/// This field contains all the ID3v2 tags found in the corresponding file.
		/// </summary>
		private System.Collections.Generic.Dictionary<string, string> _fieldValues = new System.Collections.Generic.Dictionary<string, string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="TangoDJ.Library.SongInfo"/> class.
		/// </summary>
		/// <param name='path'>
		/// The full path to the file
		/// </param>
		public SongInfo(string path)
		{
			this.Path = path;
			
			try
			{
				ID3.ID3Info i = new ID3.ID3Info(path, true);
				
				this.Artist = i.ID3v1Info.Artist;
				this.Title = i.ID3v1Info.Title;
				
				foreach (ID3.ID3v2Frames.TextFrames.TextFrame tf in i.ID3v2Info.TextFrames)
				{
					if (FieldMap.ContainsKey(tf.FrameID))
					{
						if (this._fieldValues.ContainsKey(tf.FrameID)) { this._fieldValues[tf.FrameID] = tf.Text; } 
						else { this._fieldValues.Add(tf.FrameID, tf.Text); }
					} 
					else 
					{
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
			} 
			catch (System.ArgumentOutOfRangeException aoore)
			{
				System.Console.WriteLine(path + Environment.NewLine + aoore.ToString());
			}
		}

		/// <summary>
		/// Gets the album name
		/// </summary>
		/// <value>
		/// The album name
		/// </value>
		public string Album		
		{
			get { return this._fieldValues["TALB"]; } 
			private set { this._fieldValues.Add("TALB", value); }
		}

		/// <summary>
		/// Gets the artist name
		/// </summary>
		/// <value>
		/// The artist name
		/// </value>
		public string Artist
		{
			get
			{
				if (this._fieldValues.ContainsKey("TOPE") && !string.IsNullOrWhiteSpace(this._fieldValues["TOPE"]))
				{ return this._fieldValues["TOPE"]; }
				else if (this._fieldValues.ContainsKey("TPE2") && !string.IsNullOrWhiteSpace(this._fieldValues["TPE2"]))
				{ return this._fieldValues["TPE2"]; }
				else if (this._fieldValues.ContainsKey("TPE1") && !string.IsNullOrWhiteSpace(this._fieldValues["TPE1"]))
				{ return this._fieldValues["TPE1"]; }
				else
				{ return string.Empty; }
			} 

			////TODO: Remove set
			private set 
			{ 
				this._fieldValues.Add("TOPE", value); 
			}
		}

		/// <summary>
		/// Gets the type of the content.
		/// </summary>
		/// <value>
		/// The type of the content.
		/// </value>
		public string ContentType	
		{ 
			get { return this._fieldValues.ContainsKey("TCON") ? this._fieldValues["TCON"] : string.Empty; } 
			private set { this._fieldValues.Add("TCON", value); } 
		}

		/// <summary>
		/// Gets the field keys.
		/// </summary>
		/// <value>
		/// The field keys.
		/// </value>
		public Dictionary<string, string>.KeyCollection FieldKeys	
		{
			get { return this._fieldValues.Keys; }
		}

		/// <summary>
		/// Gets the field values.
		/// </summary>
		/// <value>
		/// The field values.
		/// </value>
		public Dictionary<string, string>.ValueCollection FieldValues 
		{ 
			get { return this._fieldValues.Values; } 
		}

		/// <summary>
		/// Gets the genre.
		/// </summary>
		/// <value>
		/// The genre.
		/// </value>
		public string Genre	
		{ 
			get { return string.Empty; } 
		}

		/// <summary>
		/// Gets the lead performer.
		/// </summary>
		/// <value>
		/// The lead performer.
		/// </value>
		public string LeadPerformer	
		{ 
			get { return this._fieldValues["TPE1"]; } 
			private set { this._fieldValues.Add("TPE1", value); } 
		}

		/// <summary>
		/// Gets the path.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public string Path		{ get; private set; }

		/// <summary>
		/// Gets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		public string Title
		{
			get { return this._fieldValues["TIT2"]; } 
			private set { this._fieldValues.Add("TIT2", value); }
		}

		/// <summary>
		/// Gets a JSON object containing the relevant information on this SongInfo object
		/// </summary>
		/// <returns>
		/// The JSON object
		/// </returns>
		public Newtonsoft.Json.Linq.JObject GetJSON()
		{
			Newtonsoft.Json.Linq.JObject ret = new Newtonsoft.Json.Linq.JObject();

			ret.Add("Path", this.Path);

			foreach (string k in this._fieldValues.Keys)
			{
				ret.Add(k, this._fieldValues[k]);
			}

			return ret;
		}
	}
}