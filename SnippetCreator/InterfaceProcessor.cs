using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace SnippetCreator
{
	/// <summary>
	/// Handles user interface.
	/// </summary>
	internal class InterfaceProcessor
	{
		// -----Enums-----
		/// <summary>
		/// What are you trying to display?
		/// </summary>
		private enum DisplayType
		{
			SnippetCode, // The code the user enters
			Variable, // A variable
			Title,
			UserTitle,
			Author,
			UserAuthor,
		}

		// -----Fields-----
		public static ConsoleKey EscapeKey = ConsoleKey.Escape;

		private static int _multilineThing;

		// -----Methods-----
		/// <summary>
		/// Gets the user to enter an input that spans multiple lines.
		/// </summary>
		/// <param name="initialState">initial state of the input</param>
		/// <returns></returns>
		public static string GetMultilineInput(string initialState)
		{
			ConsoleKeyInfo input;
			StringBuilder input_Str = new StringBuilder(initialState);

			int cursorPos = -1; // index of the current (the most recently typed) character
			int removalLength;
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
						RemovePreviousCharacter(ref input_Str, ref cursorPos);
						break;
					case ConsoleKey.Delete:
						RemoveNextCharacter(ref input_Str, cursorPos);
						break;

					// navigating the string
					case ConsoleKey.LeftArrow:
						cursorPos = MoveLeft(input_Str, cursorPos);
						break;
					case ConsoleKey.RightArrow:
						cursorPos = MoveRight(input_Str, cursorPos);
						break;
					case ConsoleKey.UpArrow:
						cursorPos = MoveUp(input_Str, cursorPos);
						break;
					case ConsoleKey.DownArrow:
						cursorPos = MoveDown(input_Str, cursorPos);
						break;
					case ConsoleKey.Home:
						cursorPos = MoveToStartOfLine(input_Str, cursorPos);
						break;
					case ConsoleKey.End:
						cursorPos = MoveLeft(input_Str, MoveToStartOfNextLine(input_Str, cursorPos));
						break;

					// normal ASCII character
					default:
						input_Str.Append(input.KeyChar);
						break;
				}
				// Display
				Display(input_Str.ToString(), DisplayType.SnippetCode);

			} while (input.Key != EscapeKey);

			// Return
			return input_Str.ToString();

			// Local functions
			void RemoveNextCharacter(ref StringBuilder text, int cursorPos)
			{
				removalLength = 1;
				if (cursorPos == (text.Length - 1))
				{
					return;
				}
				if (CheckIfNextCharacterIsANewLine(text, cursorPos))
				{
					removalLength = Environment.NewLine.Length;
				}
				text = text.Remove(cursorPos, removalLength);
			}
			void RemovePreviousCharacter(ref StringBuilder text, ref int cursorPos)
			{
				removalLength = 1;
				if (cursorPos == 0) // is there even a character to delete?
                {
					return;
				}

				int startPos = cursorPos;
				if (CheckIfPreviousCharacterIsANewLine(text, cursorPos))
				{
					removalLength = Environment.NewLine.Length;
					cursorPos -= (removalLength - 1);
				}
				text = text.Remove(startPos, removalLength);
			}
			bool CheckIfPreviousCharacterIsANewLine(StringBuilder text, int cursorPos)
			{
				int startOfNewLine = cursorPos - Environment.NewLine.Length + 1;
				if (startOfNewLine < 0)
				{
					return false;
				}
				for (int i = 0; i < Environment.NewLine.Length; i++)
				{
					if (text[startOfNewLine + i] != Environment.NewLine[i])
					{
						return false;
					}
				}
				return true;
			}
			bool CheckIfNextCharacterIsANewLine(StringBuilder text, int cursorPos)
			{
				if ((cursorPos + Environment.NewLine.Length) > text.Length)
				{
					return false;
				}
				for (int i = 0; i < Environment.NewLine.Length; i++)
				{
					if (text[cursorPos + i] != Environment.NewLine[i])
					{
						return false;
					}
				}
				return true;
			}
			int MoveLeft(StringBuilder text, int cursorPos)
			{
				if (cursorPos == 0) // cursor is at start
				{
					return text.Length - 1;
				}
				if(CheckIfPreviousCharacterIsANewLine(text, cursorPos)) // new line
				{
					return cursorPos - Environment.NewLine.Length;
				}
				return cursorPos - 1; // normal
			}
			int MoveRight(StringBuilder text, int cursorPos)
			{
				if (cursorPos == (text.Length - 1)) // cursor is at end
				{
					return 0;
				}
				if (CheckIfNextCharacterIsANewLine(text, cursorPos)) // new line
				{
					return cursorPos + Environment.NewLine.Length;
				}
				return cursorPos + 1; // normal
			}
			int MoveUp(StringBuilder text, int cursorPos)
			{
				int startOfCurrLine = MoveToStartOfLine(text, cursorPos);
				int startOfPrevLine = MoveToStartOfLine(text, MoveLeft(text, cursorPos));
				return startOfPrevLine + (cursorPos - startOfCurrLine);
			}
			int MoveDown(StringBuilder text, int cursorPos)
			{
				int startOfCurrLine = MoveToStartOfLine(text, cursorPos);
				int startOfNextLine = MoveToStartOfNextLine(text, cursorPos);
				return startOfNextLine + (cursorPos - startOfCurrLine);
			}
			/// <returns>The position of the character directly to the right of the closest NewLine (going in the left direction).</returns>
			int MoveToStartOfLine(StringBuilder text, int cursorPos)
			{
				int startOfLine = cursorPos;
				while (!CheckIfPreviousCharacterIsANewLine(text, cursorPos))
				{
					startOfLine = MoveLeft(text, startOfLine);
					if (startOfLine == 0)
					{
						return 0;
					}
				}
				return startOfLine;
			}
			/// <returns>The position of the character directly to the right of the closest NewLine (going in the right direction).</returns>
			int MoveToStartOfNextLine(StringBuilder text, int cursorPos)
			{
				int startOfNextLine = cursorPos;
				while (!CheckIfNextCharacterIsANewLine(text, cursorPos))
				{
					startOfNextLine = MoveRight(text, startOfNextLine);
					if (startOfNextLine == (text.Length - 1)) // reached the end of the text
					{
						return startOfNextLine;
					}
				}
				return MoveRight(text, startOfNextLine);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text">text that you want to display</param>
		/// <param name="inputType">what the text represents</param>
		private static void Display(string text, DisplayType inputType)
		{
			int yPos = 0;
			switch (inputType)
			{
				case DisplayType.SnippetCode:
					yPos = _multilineThing;
					break;
				default:
					break;
			}
			Console.CursorTop = yPos;
			Console.Write(text);
		}
		// TODO: add the other display types and do an overall input manager
	}
}
