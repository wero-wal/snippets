using System.Text;
namespace SnippetCreator
{
	internal class Snippet
	{
		// -----Enums-----
		public enum Properties{
			Title,
			Description,
			Author,
			Shortcut,
			Count,
		}

		// -----Properties-----
		public static string[] Languages => _languages;

		// -----Fields-----
		private const string _declarations = "Declarations";
		private const string _literal = "Literal";

		private static string[] _partsOfTheCode = {
			"<?xml version=\"1.0\" encoding=\"utf-8\"?><CodeSnippets xmlns=\"http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet\"><CodeSnippet Format=\"1.0.0\"><Header>",
			// Title, Author, Description, Shortcut
			"</Header><Snippet><Code Language=\"", // Language
			"\"><![CDATA[", // Code
			"]]></Code>", // all the Literals
			"</Snippet></CodeSnippet></CodeSnippets>",
		};
		private static string[] _languages = File.Load("languages.txt").ToArray();
		private static string[] _forbiddenWords = File.Load("forbidden-values.txt").ToArray();
		private static int _defaultLanguage;
		private static string _defaultAuthor;

		private List<SnippetVar> _variables;
		private string[] _properties = new string[] {
			"myTitle",
			"myDescription",
			_defaultAuthor,
			"myShortcut"
		};
		private string _userCode;
		private string _code;
		private int _languageIndex;


		// -----Constructors-----
		public Snippet()
		{
			int x = 0;
			List<string> defaults = File.Load("defaults.txt");
			_defaultLanguage = Convert.ToInt32(defaults[x++]);
			_defaultAuthor = defaults[x++];
			_variables = new List<SnippetVar>();
			_languageIndex = _defaultLanguage;
			SetProperty(Properties.Author, _defaultAuthor);
		}

		// -----Methods-----
		// static
		public static bool LoadDefaults()
		{
			List<string> defaults = File.Load(File.Defaults);
			if(int.TryParse(defaults[0] ?? "0", out _defaultLanguage))
			{
				_defaultLanguage = 0;
			}
			_defaultAuthor = defaults[0] ?? "myAuthor";
			return true;
		}
		private static void SaveDefaults()
		{
			File.Save(new List<string>{_defaultLanguage.ToString(), _defaultAuthor}, File.Defaults, true);
		}
		public static bool SetDefaultAuthor(string defaultAuthor)
		{
			if (CheckIfValid(defaultAuthor))
			{
				_defaultAuthor = defaultAuthor;
				return true;
			}
			else return false;
		}

		private static bool CheckIfValid(string text)
		{
			for (int i = 0; i < _forbiddenWords.Length; i++)
			{
				if (text.Contains(_forbiddenWords[i]))
				{
					return false;
				}
			}
			return true;
		}
		
		// instance
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
		public static bool SetProperty(Properties property, string newValue)
		{
			if (!CheckIfValid(newValue))
			{
				return false;
			}
			_properties[(int)property] = newValue;
			return true;
		}

		private string GenerateCode()
		{
			int currentPartOfCode = 0;
			StringBuilder code = new();
			AddNextPartOfCode();

			// Title, Author, Description, Shortcut
			for (int i = 0; i < (int)Properties.Count; i++)
			{
				code.Append(Encase(((Properties)i).ToString(), _properties[i]));
			}

			// Language
			AddNextPartOfCode();
			code.Append(_languages[_languageIndex]);
			AddNextPartOfCode();

			// User code
			code.Append(_userCode);
			AddNextPartOfCode();

			// Literals
			if (_variables is not null)
			{
				code.Append(Open(_declarations));
				foreach (SnippetVar variable in _variables)
				{
					code.Append(Encase(_literal, variable.GenerateCode()));
				}
				code.Append(Close(_declarations));
			}
			AddNextPartOfCode();

			return code.ToString();
			
			// local functions
			string Open(string sectionName)
			{
				return $"<{sectionName}>";
			}
			string Close(string sectionName)
			{
				return $"</{sectionName}>";
			}
			string Encase(string sectionName, string sectionContents)
			{
				return $"<{sectionName}>{sectionContents}</{sectionName}>";
			}
			void AddNextPartOfCode()
			{
				code.Append(_partsOfTheCode[currentPartOfCode++]);
			}
		}
	}
}
