using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nea
{
	/// <summary>
	/// A custom Reader class. It can read from either a StreamReader object, a 
	/// NeaStreamReader object or a String.
	/// </summary>
	public class NeaReader
	{
		String buffer;
		int pos = 0;
		NeaStreamReader textHolder;

		#region Constructors

		/// <summary>
		/// Creates a NeaReader, which encapsulates a StreamReader.
		/// </summary>
		/// <param name="reader">The StreamReader to read from.</param>
		public NeaReader(StreamReader reader) {
			textHolder = new NeaStreamReader(reader);
		}

		/// <summary>
		/// Creates a NeaReader, which encapsulates a NeaStreamReader.
		/// </summary>
		/// <param name="reader">The NeaStreamReader to read from.</param>
		public NeaReader(NeaStreamReader reader) {
			textHolder = reader;
		}

		/// <summary>
		/// Creates a NeaReader, which encapsulates a String.
		/// </summary>
		/// <param name="text">The String to read from.</param>
		public NeaReader(String text) {
			textHolder = new NeaStreamReader(text);
		}

		#endregion

		#region Public stuff

		/// <summary>
		/// Returns any next character. If it is the end of a line, returns '\n'. If 
		/// end of stream/file has been reached, -1 is returned.
		/// Advances the position of the stream.
		/// </summary>
		/// <returns></returns>
		public int Read() {
			int c = Peek();
			pos++;
			return c;
		}

		/// <summary>
		/// Returns any next character. If it is the end of a line, returns '\n'. If
		/// end of stream/file has been reached, the -1 is returned. 
		/// Does not advance the position of the stream.
		/// </summary>
		/// <returns></returns>
		public int Peek() {
			if (buffer == null) {
				buffer = textHolder.ReadLine();
				pos = 0;
			}
			if (buffer == null) {
				return -1;
			}
			// Den kommer kun herned, hvis der er noget i bufferen
			if (pos > buffer.Length) {
				buffer = textHolder.ReadLine();
				pos = 0;
				if (buffer == null) {
					return -1;
				}
				else if (buffer.Length < 1) {
					return '\n';
				}
				return buffer.ElementAt(0);
			}
			else if (pos == buffer.Length) {
				return '\n';
			}
			else {
				return buffer.ElementAt(pos);
			}
		}

		/// <summary>
		/// Skips the next whitespace characters in the stream, if any.
		/// </summary>
		public void SkipWhiteSpace() {
			while (Peek() != -1 && Char.IsWhiteSpace((char)Peek())) {
				Read();
			}
		}

		/// <summary>
		/// Closes the underlying NeaStreamReader
		/// </summary>
		public void Close() {
			textHolder.Close();
		}

		/// <summary>
		/// Reads the stream until char t appears (or end of the stream is reached), 
		/// and returns the read string without the final character. The value 
		/// discard determines whether the final character is discarded.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="discard"></param>
		/// <returns></returns>
		public String ReadUntil(char t, bool discard = true) {
			StringBuilder tekst = new StringBuilder(100);
			int c = Peek();
			while ((c != t) && (c != -1)) {
				tekst.Append((char)c);
				Read();
				c = Peek();
			}
			if (discard && c != -1) {
				Read();
			}
			return tekst.ToString();
		}

		/// <summary>
		/// Reads the stream until any of the characters in the String t appears (or 
		/// end of the stream is reached), and returns the read string without the 
		/// final character. The value discard determines whether the final character
		/// is discarded.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="discard"></param>
		/// <returns></returns>
		public String ReadUntilAny(String t, bool discard = true) {
			StringBuilder tekst = new StringBuilder(100);
			int c = Peek();
			while ((!t.Contains("" + (char)c)) && (c != -1)) {
				tekst.Append((char)c);
				Read();
				c = Peek();
			}
			if (discard) {
				Read();
			}
			return tekst.ToString();
		}

		/// <summary>
		/// Reads the stream until whitespace or any of the characters in the String 
		/// t appears (or end of the stream is reached), and returns the read string 
		/// without the final character. The value discard determines whether the 
		/// final character is discarded.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="discard"></param>
		/// <returns></returns>
		public String ReadUntilWhiteSpaceOr(String t, bool discard = true) {
			StringBuilder tekst = new StringBuilder(100);
			int c = Peek();
			while (!(t.Contains("" + (char)c) || (c == -1) || char.IsWhiteSpace((char)c))) {
				tekst.Append((char)c);
				Read();
				c = Peek();
			}
			if (discard) {
				Read();
			}
			return tekst.ToString();
		}

		/// <summary>
		/// Reads the stream until any of the strings in the String[] t appears (or 
		/// end of the stream is reached), and returns the read string without the 
		/// terminating string. The terminating string is available as an output
		/// parameter.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="terminator"></param>
		/// <returns></returns>
		public String ReadUntilAny(String[] t, out String terminator) {
			StringBuilder tekst = new StringBuilder(100);
			StringBuilder beginnings = new StringBuilder(t.Length);
			for (int i = 0; i < t.Length; i++) {
				beginnings[i] = t[i][0];
			}
			String be = beginnings.ToString();
			tekst.Append(ReadUntilAny(be, discard: false));

			String test = "";
			int c = Peek();
			while (c != -1) {
				c = Read();
				if (be.Contains((char)c)) {
					test = "" + (char)c;
					bool end = false;
					while (EarlyEquals(test, t, out end)) {
						if (end) {
							terminator = test;
							return tekst.ToString();
						}
						test += (char)Read();
					}
				}
				tekst.Append(test);
			}

			terminator = "";
			return tekst.ToString();
		}

		private static bool EarlyEquals(String test, String[] tests, out bool fullEqual) {
			fullEqual = false;
			bool result = false;
			foreach (String t in tests) {
				if (t.Length > test.Length) {
					String m = t.Substring(0, test.Length);
					if (m == test) {
						result = true;
					}
				}
				else if (t.Length == test.Length) {
					if (t == test) {
						fullEqual = true;
						return true;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Reads the stream as long as it only has the characters in the String t, 
		/// and returns the read string without any following character. The value
		/// discard determines whether the immediately following character is 
		/// discarded.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="discard"></param>
		/// <returns></returns>
		public String ReadUntilNot(String t, bool discard = true) {
			StringBuilder tekst = new StringBuilder(100);
			int c = Peek();
			while ((t.Contains("" + (char)c)) && (c != -1)) {
				tekst.Append((char)c);
				Read();
				c = Peek();
			}
			if (discard) {
				Read();
			}
			return tekst.ToString();
		}

		/// <summary>
		/// Reads the stream until it reaches the String t, and returns the result
		/// without the terminating string. The terminating string is removed from
		/// the stream.
		/// </summary>
		/// <param name="t"></param>
		public String ReadUntil(String t) {
			StringBuilder tekst = new StringBuilder(100);
			tekst.Append(ReadUntil(t[0], discard: false));
			int c = Peek();
			String test = "";

			while (c != -1) {
				c = Read();
				if (c == t[0]) {
					test = "" + (char)c;
					c = Peek();
					for (int i = 1; ((i < t.Length) && (c == t[i])); i++) {
						test += (char)c;
						Read();
						c = Peek();
					}
					if (t == test) {
						break;
					}
				}
				else
					test = "" + (char)c;
				tekst.Append(test);
			}
			return tekst.ToString();
		}

		/// <summary>
		/// Skips in the stream until the String t is reached. The character 
		/// position after this operation is at the character immediately following
		/// the first occurrence of the string t.
		/// </summary>
		/// <param name="t"></param>
		public void SkipUntil(String t) {
			int c = Peek();
			String test;

			while (c != -1) {
				c = Read();
				if (c == t[0]) {
					test = "" + (char)c;
					c = Peek();
					for (int i = 1; ((i < t.Length) && (c == t[i])); i++) {
						test += (char)c;
						Read();
						c = Peek();
					}
					if (t == test) {
						break;
					}
				}
			}
		}

		/// <summary>
		/// Skips in the stream until the char t is reached. The value discard
		/// determines whether the terminating character t is discarded.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="discard"></param>
		public void SkipUntil(char t, bool discard = true) {
			int c = Peek();

			while ((c != t) && (c != -1)) {
				Read();
				c = Peek();
			}
			if (discard) {
				Read();
			}
		}

		/// <summary>
		/// Reads the stream until next end of line, and returns the read string 
		/// without the final newline character. End of line is discarded and the 
		/// stream's position is the next line.
		/// </summary>
		/// <returns></returns>
		public String ReadLine() {
			return ReadUntil('\n');
		}

		/// <summary>
		/// Reads the stream from the current position to the end of the stream.
		/// </summary>
		/// <returns></returns>
		public String ReadToEnd() {
			StringBuilder tekst = new StringBuilder(100);
			tekst.Append(ReadLine());
			if (Peek() != -1)
				tekst.Append("\n" + textHolder.ReadToEnd());
			return tekst.ToString();
		}

		/// <summary>
		/// Skips whitespace and returns the characters until the next whitespace.
		/// </summary>
		/// <returns></returns>
		public String ReadWord() {
			SkipWhiteSpace();
			StringBuilder tekst = new StringBuilder(100);
			int c = Read();
			while ((!Char.IsWhiteSpace((char)c)) && (c != -1)) {
				tekst.Append((char)c);
				c = Read();
			}
			return tekst.ToString();
		}

		/// <summary>
		/// Skips whitespace and returns a character.
		/// </summary>
		/// <returns></returns>
		public int ReadChar() {
			SkipWhiteSpace();
			return Read();
		}

		/// <summary>
		/// Skips whitespace and tries to read an Int32.
		/// </summary>
		/// <returns></returns>
		public int ReadInt() {
			return Int32.Parse(ReadWord());
		}

		/// <summary>
		/// Skips whitespace and tries to read an Double.
		/// </summary>
		/// <returns></returns>
		public double ReadDouble() {
			return Double.Parse(ReadWord());
		}

		/// <summary>
		/// Skips whitespace and tries to read an Boolean.
		/// </summary>
		/// <returns></returns>
		public bool ReadBoolean() {
			return ParseBoolean(ReadWord());
		}

		/// <summary>
		/// Skips the stream until the first occurrence of the string start. Then 
		/// the text is read until there has been as many occurrences of the string
		/// end as the string start.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public String ReadSection(String start, String end) {
			SkipUntil(start);
			StringBuilder tekst = new StringBuilder(100);
			String terminator;
			int depth = 1;
			while (depth > 0) {
				tekst.Append(ReadUntilAny(new String[] { start, end }, out terminator));
				if (terminator == start) {
					depth++;
					tekst.Append(terminator);
				}
				else {
					depth--;
					if (depth > 0) {
						tekst.Append(terminator);
					}
				}
			}

			return tekst.ToString();
		}

		/// <summary>
		/// Skips the stream until the first occurrence of the char start. Then the
		/// text is read until there has been as many occurrences of the char end as
		/// the char start.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public String ReadSection(char start, char end) {
			SkipUntil(start);
			StringBuilder tekst = new StringBuilder(100);
			char terminator;
			int depth = 1;
			while (depth > 0) {
				tekst.Append(ReadUntilAny("" + start + end, discard: false));
				terminator = (char)Read();
				if (terminator == start) {
					depth++;
					tekst.Append(terminator);
				}
				else {
					depth--;
					if (depth > 0) {
						tekst.Append(terminator);
					}
				}
			}

			return tekst.ToString();
		}

		/// <summary>
		/// Skips whitespace and then tries to read an simple mathematical or 
		/// boolean expression from the stream.
		/// </summary>
		/// <returns></returns>
		public Expression GetExpression() {
			SkipWhiteSpace();
			Expression result, op1;

			if (Peek() == '(') {
				op1 = GetExpression(ReadSection('(', ')'));
			}
			else {
				op1 = new Expression(ReadUntilAny("+-*/&| ;\n", discard: false));
			}
			SkipWhiteSpace();

			if ("+-*/&|".Contains((char)Peek())) {
				result = GetExpression(op1);
			}
			else {
				result = op1;
			}
			return result;
		}

		private Expression GetExpression(Expression firstoperand) {
			SkipWhiteSpace();
			Expression result, op2;
			ExpressionType type;

			char operation = (char)Read();
			switch (operation) {
				case '+':
					type = ExpressionType.PLUS;
					break;
				case '-':
					type = ExpressionType.MINUS;
					break;
				case '*':
					type = ExpressionType.MULTI;
					break;
				case '/':
					type = ExpressionType.DIVIDE;
					break;
				case '&':
					type = ExpressionType.AND;
					break;
				case '|':
					type = ExpressionType.OR;
					break;
				default:
					throw new FormatException("This shouldn't happen");
			}
			SkipWhiteSpace();

			if (Peek() == '(') {
				op2 = GetExpression(ReadSection('(', ')'));
			}
			else {
				op2 = new Expression(ReadUntilAny("+-*/&| ;\n", discard: false));
			}
			SkipWhiteSpace();

			result = new Expression(type, firstoperand, op2);
			if ("+-*/&|".Contains((char)Peek())) {
				result = GetExpression(result);
			}
			return result;
		}

		#endregion

		#region Static public stuff

		/// <summary>
		/// Reads a section delimited by the characters start and end from the 
		/// String source.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static String ReadSection(String source, char start, char end) {
			int index = 0;
			for (; (index < source.Length && source[index] != start); index++) { } //skipper til vi finder start
			int depth = 1;
			StringBuilder tekst = new StringBuilder(100);

			while (depth > 0) {
				int previndex = index;
				for (; (index < source.Length && (source[index] != start) && (source[index] != end)); index++) { }
				tekst.Append(source.Substring(previndex + 1, index - previndex - 1));
				char terminator = source[index];
				if (terminator == start) {
					depth++;
					tekst.Append(terminator);
				}
				else {
					depth--;
					if (depth > 0) {
						tekst.Append(terminator);
					}
				}
			}
			return tekst.ToString();
		}

		/// <summary>
		/// Tries to read an mathematical or boolean expression from the String 
		/// source.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public Expression GetExpression(String source) {
			NeaReader reader = new NeaReader(source);
			return reader.GetExpression();
		}

		/// <summary>
		/// Tries to parse a Boolean from the String source. This function is more
		/// forgiving than the default Boolean.Parse.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static bool ParseBoolean(String source) {
			switch (source) {
				case "TRUE":
				case "True":
				case "true":
				case "YES":
				case "Yes":
				case "yes":
				case "Y":
				case "y":
					return true;
				case "FALSE":
				case "False":
				case "false":
				case "NO":
				case "No":
				case "no":
				case "N":
				case "n":
					return false;
				default:
					throw new FormatException();
			}
		}

		#endregion
	}
}
