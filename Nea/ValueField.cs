using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea
{
	/// <summary>
	/// Defines a key/values relationship. An object of this class contains a key 
	/// and either a single string or one or more other ValueFields.
	/// </summary>
	public class ValueField : IKeyed, IEnumerable
	{
		public String Key { get; private set; }
		public String Value {
			get {
				if (value is String) {
					return value as String;
				}
				else {
					return value.ToString();
				}
			}
			set {
				try {
					this.value = int.Parse(value);
				}
				catch {
					try {
						this.value = decimal.Parse(value);
					}
					catch {
						this.value = value;
					}
				}
			}
		}
		private object value;
		private KeyedList<ValueField> values;

		/// <summary>
		/// Returns the ValueField with the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public ValueField this[String key, KeyedList<ValueField> alternative = null] {
			get {
				return values[key, alternative];
			}
		}

		/// <summary>
		/// Returns the ValueField at the given position in the list.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public ValueField this[int index] {
			get {
				return values[index];
			}
		}

		/// <summary>
		/// Returns the value as an object.
		/// </summary>
		public object AsObject {
			get {
				return value;
			}
			set {
				this.value = value;
			}
		}
		/// <summary>
		/// Returns the value as an Int32.
		/// </summary>
		public int AsInt {
			get {
				if (value is int)
					return (int)value;
				if (value is decimal)
					return (int)((decimal)value);
				return int.Parse((string)value);
			}
			set {
				this.value = value;
			}
		}
		/// <summary>
		/// Returns the value as a Double.
		/// </summary>
		public double AsDouble {
			get {
				if(value is double)
					return (double)value;
				return Double.Parse((string)value);
			}
			set {
				this.value = value;
			}
		}
		/// <summary>
		/// Returns the value as a Double.
		/// </summary>
		public decimal AsDecimal {
			get {
				if (value is decimal)
					return (decimal)value;
				if (value is int)
					return (decimal)((int)value);
				return Decimal.Parse((string)value);
			}
			set {
				this.value = value;
			}
		}
		/// <summary>
		/// Returns the value as a Boolean.
		/// </summary>
		public bool AsBoolean {
			get {
				if (value is bool)
					return (bool)value;
				return NeaReader.ParseBoolean(Value);
			}
			set {
				this.value = value;
			}
		}

		/// <summary>
		/// Returns the value as a list of strings
		/// </summary>
		public String[] AsStringList {
			get {
				List<String> list = new List<String>();
				NeaReader reader = new NeaReader(Value);
				while (reader.Peek() != -1) {
					list.Add(reader.ReadWord());
				}
				return list.ToArray();
			}
			set {
				Value = "";
				foreach (String s in value) {
					Value += s + " ";
				}
			}
		}

		/// <summary>
		/// Initializes a new ValueField with the given key and value.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public ValueField(String key, object value) {
			Key = key;
			this.value = value;
		}

		/// <summary>
		/// Initializes a new ValueField with the given key and ValueFields.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fields"></param>
		public ValueField(String key, params ValueField[] fields) {
			Key = key;
			values = new KeyedList<ValueField>();
			foreach (ValueField v in fields) {
				values.Add(v);
			}
		}

		/// <summary>
		/// Initializes a new ValueField with the data from the given string.
		/// </summary>
		/// <param name="data"></param>
		public ValueField(String data, bool recursively = true)
			: this(new NeaReader(data), recursively) { }

		/// <summary>
		/// Initializes a new ValueField with the data from the given NeaReader. If 
		/// recursively is set as false, any contained ValueFields will be ignored 
		/// and all saved as the Value string.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="recursively"></param>
		public ValueField(NeaReader reader, bool recursively = true) {
			reader.SkipWhiteSpace();
			Key = reader.ReadUntilAny("[:]", discard: false);
			if (reader.Peek() == ']') {
				Value = "";
				Key = "";
			}
			else if (reader.Peek() == ':') {
				reader.Read();
				reader.SkipWhiteSpace();
				Value = reader.ReadUntil(';');
			}
			else {
				String section = reader.ReadSection('[', ']');
				NeaReader r = new NeaReader(section);
				r.SkipWhiteSpace();
				Key = r.ReadUntil(':');
				if (recursively) {
					values = new KeyedList<ValueField>();

					r.SkipWhiteSpace();
					while (r.Peek() != -1) {
						values.Add(new ValueField(r));
						r.SkipWhiteSpace();
					}
					Value = null;
				}
				else {
					Value = r.ReadToEnd();
				}

			}
		}

		/// <summary>
		/// Returns whether the ValueField contains just a string.
		/// </summary>
		/// <returns></returns>
		public bool IsSimple() {
			if (value == null)
				return false;
			return true;
		}

		/// <summary>
		/// Adds one or more ValueFields.
		/// </summary>
		/// <param name="fields"></param>
		public void Add(params ValueField[] fields) {
			foreach (ValueField v in fields) {
				values.Add(v);
			}
		}

		/// <summary>
		/// Returns this ValueField serialized as a string.
		/// </summary>
		/// <param name="indent"></param>
		/// <returns></returns>
		public String Save(bool indent = false) {
			StringBuilder tekst = new StringBuilder(100);
			String[] list = GetSavedStrings();
			foreach (String s in list) {
				if (indent)
					tekst.Append(@"	");
				tekst.AppendLine(s);
			}
			return tekst.ToString();
		}

		/// <summary>
		/// Returns this ValueField serialized as a list of Strings.
		/// </summary>
		/// <returns></returns>
		public String[] GetSavedStrings() {
			if (IsSimple()) {
				return new String[] { Key + ": " + Value + ";" };
			}
			else {
				List<String> list = new List<String>();
				list.Add("[" + Key + ":");
				foreach (ValueField v in values) {
					foreach (String s in v.GetSavedStrings()) {
						list.Add(@"	" + s);
					}
				}
				list.Add("]");
				return list.ToArray();
			}
		}

		public IEnumerator GetEnumerator() {
			return values.GetEnumerator();
		}

		public ValueField[] ToArray() {
			return values.ToArray();
		}
	}
}
