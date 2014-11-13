using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea
{
	public class Expression
	{
		public ExpressionType type;
		public String value;
		public Expression operand1, operand2;

		public Expression(ExpressionType type, Expression op1, Expression op2) {
			this.type = type;
			operand1 = op1;
			operand2 = op2;
			value = null;
		}

		public Expression(ExpressionType type, String op1, String op2)
			: this(type, new Expression(op1), new Expression(op2)) { }

		public Expression(ExpressionType type, String op1, Expression op2)
			: this(type, new Expression(op1), op2) { }

		public Expression(ExpressionType type, Expression op1, String op2)
			: this(type, op1, new Expression(op2)) { }

		public Expression(String value) {
			this.value = value;
			type = ExpressionType.VALUE;
			operand1 = default(Expression);
			operand2 = default(Expression);
		}

		public bool IsSimple() {
			return value != null;
		}

		public bool EvaluateBoolean(KeyedList<ValueField> variables, KeyedList<ValueField> alternative = null) {
			if (this.IsSimple()) {
				bool result;
				if (!Boolean.TryParse(value, out result)) {
					result = variables[value, alternative].AsBoolean;
				}
				return result;
			}
			switch (type) {
				case ExpressionType.AND:
					return operand1.EvaluateBoolean(variables)
						&& operand2.EvaluateBoolean(variables);
				case ExpressionType.OR:
					return operand1.EvaluateBoolean(variables)
						|| operand2.EvaluateBoolean(variables);
				case ExpressionType.EQUAL:
					return operand1.EvaluateNumber(variables)
						== operand2.EvaluateNumber(variables);
				case ExpressionType.UNEQUAL:
					return operand1.EvaluateNumber(variables)
						!= operand2.EvaluateNumber(variables);
				default:
					throw new Exception("This cannot evaluate to a boolean");
			}
		}

		public decimal EvaluateNumber(KeyedList<ValueField> variables, KeyedList<ValueField> alternative = null) {
			if (this.IsSimple()) {
				decimal result;
				if (!Decimal.TryParse(value, out result)) {
					result = variables[value, alternative].AsDecimal;
				}
				return result;
			}
			switch (type) {
				case ExpressionType.PLUS:
					return operand1.EvaluateNumber(variables)
						+ operand2.EvaluateNumber(variables);
				case ExpressionType.MINUS:
					return operand1.EvaluateNumber(variables)
						- operand2.EvaluateNumber(variables);
				case ExpressionType.MULTI:
					return operand1.EvaluateNumber(variables)
						* operand2.EvaluateNumber(variables);
				case ExpressionType.DIVIDE:
					return operand1.EvaluateNumber(variables)
						/ operand2.EvaluateNumber(variables);
				default:
					throw new Exception("This cannot evaluate to a number");
			}
		}
	}

	public enum ExpressionType { VALUE, NEGATIVEVALUE, AND, OR, EQUAL, UNEQUAL, PLUS, MINUS, MULTI, DIVIDE }
}
