using System;

namespace SnippetCreator
{
	class SnippetVar
	{
		private static int _unsetIdCounter = 0;

		public string Id { get => _id; set {
			_unsetIdCounter--;
			_id = value;
		}}
		public string ToolTip { get => _toolTip; set => _toolTip = value; }
		public string DefaultValue { get => _defaultValue; set => _defaultValue = value; }

		private string _id;
		private string _toolTip;
		private string _defaultValue;
		
		public SnippetVar()
		{
			_id = $"id{_unsetIdCounter++}";
			_toolTip = "tool tip";
			_defaultValue = "default";
		}

		public string GenerateCode()
		{
			return $"<Literal><ID>{_id}</ID><Default>{_defaultValue}</ToolTip>{_toolTip}</ToolTip></Literal>";
		}
	}
}
