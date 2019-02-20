using System;
using Priority_Queue;
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
        private float priority;
        private BigFloat lastValue;

        private Dictionary<string, Expression> expressions;
        private FastPriorityQueue<Expression> sequence;

        public readonly string Name;
        public readonly VariableType Type;

        public BigFloat Value
        {
            get
            {
                sequence = new FastPriorityQueue<Expression>(expressions.Count);

                foreach (var expression in expressions)
                {
                    sequence.Enqueue(expression.Value, expression.Value.Priority);
                }

                var actions = sequence.Count;
                while (actions --> 0)
                {
                    lastValue = sequence.Dequeue().Calculate();
                }

                return lastValue;
            }
        }

        public Variable(string name)
        {
            Name = name;

            lastValue = new BigFloat(0);

            expressions = new Dictionary<string, Expression>();

            Type = VariableType.Sequence;
        }

        public Variable(BigFloat value)
        {
            Name = string.Empty;

            lastValue = new BigFloat(value);

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

            lastValue = new BigFloat((double)value);

            Type = VariableType.Value;
        }

        public void AddExpression(Expression expression)
        {
            if (expressions.ContainsKey(expression.Name))
            {
                throw new ArgumentException(string.Format("expression named \"{0}\" is already exist.", expression.Name));
            }

            expressions.Add(expression.Name, expression);
        }

        public void RemoveExpression(Expression expression)
        {
            RemoveExpression(expression.Name);
        }

        public void RemoveExpression(string name)
        {
            if (!expressions.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("expression named \"{0}\" is not exist.", name));
            }

            expressions.Remove(name);
        }
    }
}
