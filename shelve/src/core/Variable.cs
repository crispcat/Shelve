using System;
using System.Numerics;
using System.Collections.Generic;

namespace Shelve.Core
{
    public enum VariableType
    {
        Sequence,
        Value,
    }

    [Serializable]
    public sealed class Variable
    {
        private double lastValue;

        private ExpressionChain sequence;

        public readonly string Name;
        public readonly VariableType Type;

        public double Value
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Variable(string name)
        {
            Name = name;

            lastValue = 0;

            sequence = new ExpressionChain(Config.MAX_EXPRESSION_COUNT);

            Type = VariableType.Sequence;
        }

        public Variable(double value)
        {
            Name = string.Empty;

            lastValue = value;

            Type = VariableType.Value;
        }

        public Variable(object value)
        {
            Name = string.Empty;

            bool suportedType = value is int || value is float || value is double;

            if (!suportedType)
            {
                throw new Exception(string.Format("type: \"{0}\" is not supported.", value.GetType().ToString()));
            }

            lastValue = (double)value;

            Type = VariableType.Value;
        }

        public void AddExpression(Expression expression)
        {
            sequence.Add(expression);
        }

        public void RemoveExpression(Expression expression)
        {
            sequence.Add(expression);
        }
    }
}
