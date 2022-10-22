using System;
namespace SnippetCreator
{
	class Snippet
	{
		// -----Enums-----
		// -----Properties-----
		public static string[] Languages => _languages;
		public static string[] PropertyNames => _propertyNames;

		// -----Fields-----
		private const string _declarations = "Declarations";
		private const string _literal = "Literal";

		private static string[] _bitsOfTheCode = {
			"<?xml version=\"1.0\" encoding=\"utf-8\"?><CodeSnippets xmlns=\"http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet\"><CodeSnippet Format=\"1.0.0\"><Header>",
			// Title, Author, Description, Shortcut
			"</Header><Snippet><Code Language=\"", // Language
			"\"><![CDATA[", // Code
			"]]></Code>", // all the Literals
			"</Snippet></CodeSnippet></CodeSnippets>",
		};
		private static string[] _languages = File.Load("languages.txt").ToArray();
		private static string[] _propertyNames = File.Load("properties.txt").ToArray();
		private static string[] _forbiddenWords = File.Load("forbidden-values.txt").ToArray();

		private List<SnippetVar> _variables;
		private string[] _properties = new string[] {
			"Title",
			"Description",
			"Author",
			"Shortcut"
		};
		private string _userCode;
		private string _code;
		private int _languageIndex;

		// -----Constructors-----
		public Snippet(int language, string[] properties, string code, List<SnippetVar> variables)
		{
			
		}

		// -----Methods-----
		///<summary>
		///<para>Checks if the <paramref name="languageIndex"> is between zero and the number of languages,
		///      assigning the new value and returnung true if it is; returning false if not.</para>
		///</summary>
		///<returns>true if assignment was successful, false if assignment was unsuccessful</returns>
		public bool SetLanguage(int languageIndex)
		{
			if (!((languageIndex >= 0) && 
			(languageIndex < _languages.Length)))
			{
				return false;
			}
			_languageIndex = languageIndex;
			return true;
		}
		///<summary>
		///<para>Checks whether the new value is valid, assigning the new value and returnung true if it is; returning false if not.</para>
		///</summary>
		///<returns>true if assignment was successful, false if assignment was unsuccessful</returns>
		public bool SetProperty(int index, string newValue)
		{
			for (int i = 0; i < _forbiddenWords.Length; i++)
			{
				if (newValue.Contains(_forbiddenWords[i]))
				{
					return false;
				}
			}
			_properties[index] = newValue;
			return true;
		}
		private string GenerateCode()
		{
			int x = 0;
			string code = _bitsOfTheCode[x++];

			// Title, Author, Description, Shortcut
			for (int i = 0; i < _propertyNames.Length; i++)
			{
				code += Encase(_propertyNames[i], _properties[i]);
			}

			// Language
			code += _bitsOfTheCode[x++];
			code += _languages[_languageIndex];
			code += _bitsOfTheCode[x++];

			// User code
			code += _userCode;
			code += _bitsOfTheCode[x++];

			// Literals
			if (_variables is not null)
			{
				code += Open(_declarations);
				foreach (SnippetVar variable in _variables)
				{
					code += Encase(_literal, variable.GenerateCode());
				}
				code += Close(_declarations);
			}
			code += _bitsOfTheCode[x++];

			return code;
		}
		private string Open(string sectionName)
		{
			return $"<{sectionName}>";
		}
		private string Close(string sectionName)
		{
			return $"</{sectionName}>";
		}
		private string Encase(string sectionName, string sectionContents)
		{
			return $"<{sectionName}>{sectionContents}</{sectionName}>";
		}
	}
}
