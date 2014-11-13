using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nea
{
	/// <summary>
	/// A custom version the StreamReader class. It is able to read both from a
	/// StreamReader object, as well as from a String.
	/// </summary>
	public class NeaStreamReader
	{
		readonly bool isStream;
		StreamReader streamHolder;
		String textHolder = "";
		int index = 0;

		/// <summary>
		/// Creates a NeaStreamReader, which encapsulates a StreamReader.
		/// </summary>
		/// <param name="reader">The StreamReader object to read from.</param>
		public NeaStreamReader(StreamReader reader) {
			isStream = true;
			streamHolder = reader;
		}

		/// <summary>
		/// Creates a NeaStreamReader, which encapsulates a String.
		/// </summary>
		/// <param name="text">The String to read from.</param>
		public NeaStreamReader(String text) {
			isStream = false;
			textHolder = text;
		}

		/// <summary>
		/// Returns the next available character, but does not consume it.
		/// </summary>
		/// <returns>
		/// An integer representing the next character to be read, or -1 if there
		/// are no characters to be read or if the stream does not support seeking.
		/// </returns>
		public int Peek() {
			if (isStream) {
				return streamHolder.Peek();
			}
			else {
				if (index >= textHolder.Length) {
					return -1;
				}
				else {
					return textHolder.ElementAt(index);
				}
			}
		}

		/// <summary>
		/// Returns the next available character and advances the character position
		/// by one character.
		/// </summary>
		/// <returns>
		/// An integer representing the next character to be read, or -1 if there
		/// are no characters to be read.
		/// </returns>
		public int Read() {
			if (isStream) {
				return streamHolder.Read();
			}
			else {
				if (index >= textHolder.Length) {
					return -1;
				}
				else {
					return textHolder.ElementAt(index++);
				}
			}
		}

		/// <summary>
		/// Reads a line of characters from the current stream and returns the data
		/// as a string.
		/// </summary>
		/// <returns>
		/// The next line from the input stream, or null if the end of the input 
		/// stream is reached.
		/// </returns>
		public String ReadLine() {
			//Console.WriteLine("rdln :"+ textHolder+ ": "+index);
			//Console.ReadKey();
			if (isStream) {
				return streamHolder.ReadLine();
			}
			else {
				String result = "";
				if (index >= textHolder.Length)
					return null;
				while ((index < textHolder.Length) && (textHolder.ElementAt(index) != '\n')) {
					result += textHolder.ElementAt(index++);
				}
				index++;
				return result;
			}
		}

		/// <summary>
		/// Reads the stream from the current position to the end of the stream.
		/// </summary>
		/// <returns>
		/// The rest of the stream as a string, from the current position to the end.
		/// If the current position is at the end of the stream, returns the empty
		/// string("").
		/// </returns>
		public String ReadToEnd() {
			if (isStream) {
				return streamHolder.ReadToEnd();
			}
			else {
				return textHolder.Substring(index);
			}
		}

		/// <summary>
		/// Closes the underlying stream or clears the underlying String, and 
		/// releases any system resources associated with the reader.
		/// </summary>
		public void Close() {
			if (isStream) {
				streamHolder.Close();
			}
			else {
				textHolder = "";
			}
		}
	}
}
