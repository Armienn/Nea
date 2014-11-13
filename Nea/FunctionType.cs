using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea
{
	/// <summary>
	/// Represents a function that may consist of assignments and calls to other functions
	/// </summary>
	public class FunctionType : IKeyed
	{
		public string Key { get; private set; }
		public ValueField this[String s] {
			get {
				return meta[s];
			}
		}

		ValueField meta = null;
		List<ValueField> parameters = new List<ValueField>();
		List<FunctionField> list = new List<FunctionField>();
		KeyedList<ValueField> locals = new KeyedList<ValueField>(4);

		public FunctionType(String data, CustomFunction[] functions) : this(new NeaReader(data), functions) { }

		public FunctionType(NeaReader reader, CustomFunction[] functions) {
			String section = reader.ReadSection('[', ']');
			NeaReader r = new NeaReader(section);
			r.SkipWhiteSpace();
			Key = r.ReadUntil(':');
			r.SkipWhiteSpace();

			if ((char)r.Peek() == '[') {
				meta = new ValueField(r);
			}
			r.SkipWhiteSpace();

			while (r.Peek() != -1) {
				String start = r.ReadUntilWhiteSpaceOr("[", false);

				char c = (char)r.Peek();
				if (char.IsWhiteSpace(c)) {
					r.SkipWhiteSpace();

					String name = r.ReadUntilAny(";:");
					if(!locals.Contains(name))
						locals.Add(new ValueField(name, null));
					switch (start) {
						case "param":
							parameters.Add(locals[name]);
							break;
						case "bool":
							list.Add(new AssignmentField(name, new ExpressionHolder(r.GetExpression(), ExpressionDataType.BOOL)));
							break;
						case "int":
							list.Add(new AssignmentField(name, new ExpressionHolder(r.GetExpression(), ExpressionDataType.INT)));
							break;
						case "decimal":
							list.Add(new AssignmentField(name, new ExpressionHolder(r.GetExpression(), ExpressionDataType.DECIMAL)));
							break;
					}
				}
				else if (c == '[') {
					r.Read();
					CustomFunction func = null;
					List<ExpressionHolder> exs = new List<ExpressionHolder>();
					foreach (CustomFunction f in functions) {
						if (f.Method.Name == start) {
							func = f;
							break;
						}
					}
					if (func == null) {
						continue;
					}
					String word = "";
					r.SkipWhiteSpace();
					while ((char)r.Peek() != ']') {
						word = r.ReadWord();
						switch (start) {
							case "bool":
								exs.Add(new ExpressionHolder(r.GetExpression(), ExpressionDataType.BOOL));
								break;
							case "int":
								exs.Add(new ExpressionHolder(r.GetExpression(), ExpressionDataType.INT));
								break;
							case "decimal":
								exs.Add(new ExpressionHolder(r.GetExpression(), ExpressionDataType.DECIMAL));
								break;
						}
						if ((char)r.Peek() == ';')
							r.Read();
						r.SkipWhiteSpace();
					}
					r.Read();
					list.Add(new CallField(func, exs));
				}
				
				r.SkipWhiteSpace();
			}
		}

		public void Run(List<String> paras, KeyedList<ValueField> variables) {
			if (paras.Count != parameters.Count)
				throw new Exception("Wrong amount of parameters");
			for (int i = 0; i < parameters.Count; i++) {
				parameters[i].Value = paras[i];
			}

			foreach (FunctionField f in list) {
				f.Run(variables, locals);
			}
		}
	}
}
