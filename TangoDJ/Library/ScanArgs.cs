using System;

namespace TangoDJ.Library
{
	public class ScanArgs
	{
		public Library Lib{get; private set;}
		public string Path{get; private set;}
		
		public ScanArgs (Library l, string p)
		{
			Lib = l;
			Path = p;
		}
	}
}

