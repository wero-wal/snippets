using System;
using System.Text;
using System.Collections.Generic;
namespace SnippetCreator
{
	internal class InputProcessor
	{
		public static ConsoleKey EscapeKey = ConsoleKey.Escape;
		public static string GetMultiLineInput()
		{
			ConsoleKeyInfo input;
			StringBuilder input_Str = new();
			int cursorPos = -1; // index of the current (the most recently typed) character
			do
			{
				input = Console.ReadKey();

				switch (input.Key)
				{
					// new line
					case ConsoleKey.Enter:
					    cursorPos += 2;
						input_Str.Append(Environment.NewLine);
						break;

					// deleting characters
					case ConsoleKey.Backspace:
						if (cursorPos >= 0) // is there even a character to delete?
						{
							int startPos = cursorPos;
							int removalLength = 1;

							// check if the previous 'char' was a new line
							if (input_Str[cursorPos] == Environment.NewLine[^1])
							{
								bool newLine = true;
								for (int i = 0; i < Environment.NewLine.Length; i++)
								{
									if (input_Str[cursorPos - (Environment.NewLine.Length - 1) + i] != Environment.NewLine[i])
									{
										newLine = false;
										break;
									}
								}
								if (newLine)
								{
									cursorPos -= (removalLength - 1);
									removalLength = Environment.NewLine.Length;
								}
							}
							input_Str.Remove(startPos, removalLength);
						}
						break;
					case ConsoleKey.Delete:
						if(cursorPos < input_Str.Length)
						{
							int removalLength = 1;
						}

						try
						{
							input_Str.Remove(cursorPos + 1, 1);
						}
						catch (ArgumentOutOfRangeException)
						{
							// do nothing
						}
						break;

					// navigating the string
					case ConsoleKey.LeftArrow:
						break;
					case ConsoleKey.RightArrow:
						break;

					// normal ASCII character
					default:
						input_Str.Append(input.KeyChar);
						break;
				}

				if (input.Key == ConsoleKey.Enter)
				{
				}
				else if (input.Key == ConsoleKey.Backspace)
				{
				}
				else if (input.Key)
				{
					
				}
			} while (input.Key != EscapeKey);

			return input_Str.ToString();
		}
	}
}
