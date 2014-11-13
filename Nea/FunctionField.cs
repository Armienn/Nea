using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea
{
	/// <summary>
	/// Defines a field that is runnable
	/// </summary>
	interface FunctionField
	{
		void Run(KeyedList<ValueField> variables, KeyedList<ValueField> alternative = null);
	}

	/// <summary>
	/// Assigns a value from some expression to a variable in a KeyedList collection when run
	/// </summary>
	class AssignmentField : FunctionField
	{
		String variable;
		ExpressionHolder expression;

		public AssignmentField(String var, ExpressionHolder ex) {
			variable = var;
			expression = ex;
		}

		public void Run(KeyedList<ValueField> variables, KeyedList<ValueField> alternative = null) {
			switch (expression.type) {
				case ExpressionDataType.BOOL:
					variables[variable, alternative].AsBoolean = expression.expression.EvaluateBoolean(variables, alternative);
					break;
				case ExpressionDataType.DECIMAL:
					variables[variable, alternative].AsDecimal = expression.expression.EvaluateNumber(variables, alternative);
					break;
				case ExpressionDataType.INT:
					variables[variable, alternative].AsInt = (int)expression.expression.EvaluateNumber(variables, alternative);
					break;
			}
		}
	}

	/// <summary>
	/// Calls given function with the given parameters when run
	/// </summary>
	class CallField : FunctionField
	{
		CustomFunction function;
		List<ExpressionHolder> parameters;

		public CallField(CustomFunction func, List<ExpressionHolder> para) {
			function = func;
			parameters = para;
		}

		public void Run(KeyedList<ValueField> variables, KeyedList<ValueField> alternative = null) {
			object[] paras = new object[parameters.Count];
			for (int i = 0; i < paras.Length; i++) {
				switch (parameters[i].type) {
					case ExpressionDataType.BOOL:
						paras[i] = parameters[i].expression.EvaluateBoolean(variables, alternative);
						break;
					case ExpressionDataType.DECIMAL:
						paras[i] = parameters[i].expression.EvaluateNumber(variables, alternative);
						break;
					case ExpressionDataType.INT:
						paras[i] = (int)parameters[i].expression.EvaluateNumber(variables, alternative);
						break;
				}
			}
			function(paras);
		}
	}

	/// <summary>
	/// Holds an expression along with the datatype it is supposed to be evaluated to
	/// </summary>
	public struct ExpressionHolder
	{
		public Expression expression;
		public ExpressionDataType type;

		public ExpressionHolder(Expression ex, ExpressionDataType t) {
			type = t;
			expression = ex;
		}
	}

	public enum ExpressionDataType { INT, DECIMAL, BOOL }

	public delegate void CustomFunction(object[] para);
}
