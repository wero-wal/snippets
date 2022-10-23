using System;

namespace SnippetCreator
{
	class SnippetVar
	{
		// -----Properties-----
		public string Id { get => _id; set {
			_unsetIdCounter--;
			_id = value;
		}}
		public string ToolTip { get => _toolTip; set => _toolTip = value; }
		public string DefaultValue { get => _defaultValue; set => _defaultValue = value; }
		
		// -----Fields-----
		// static
		private static int _unsetIdCounter = 0;

		// instance
		private string _id;
		private string _toolTip;
		private string _defaultValue;


		// -----Constructors-----
		public SnippetVar()
		{
			_id = $"id{_unsetIdCounter++}";
			_toolTip = "tool tip";
			_defaultValue = "default";
		}

		// -----Methods-----
		public string GenerateCode()
		{
			return $"<Literal><ID>{_id}</ID><Default>{_defaultValue}</ToolTip>{_toolTip}</ToolTip></Literal>";
		}
	}
}
