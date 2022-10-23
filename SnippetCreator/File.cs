using System;
using System.IO;
using System.Collections.Generic;

namespace SnippetCreator
{
	internal static class File
	{
		public const string Defaults = "defaults.txt";
		public const string Forbidden = "forbidden-values.txt";
		public const string Languages = "languages.txt";
		
		public static List<string> Load(string filename)
		{
			List<string> fileContents = new List<string>{};
			using(StreamReader sr = new StreamReader(filename))
			{
				while(!sr.EndOfStream)
				{
					fileContents.Add(sr.ReadLine());
				}
				sr.Close();
			}
			return fileContents;
		}
		public static void Save(List<string> newFileContents, string filename, bool overwrite)
		{
			using(StreamWriter sw = new StreamWriter(filename, !overwrite))
			{
				foreach (string line in newFileContents)
				{
					sw.WriteLine(line);
				}
				sw.Close();
			}
		}
	}
}
