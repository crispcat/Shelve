namespace Shelve.Core
{
    using System;

    public enum VariableType
    {
        Value,
        Sequence,
    }

    [Serializable]
    public sealed class Variable : IParallelAccess, IAffectable
    {
        public readonly string Name;

        public readonly VariableType Type;

        public double Value => Calculate();

        public double InitialValue { get; set; }

        public double LastValue { get; private set; }

        private HashedCircularConcurrentQueue<Expression> hashedSequence;

        private double Calculate()
        {
            if (Type == VariableType.Value)
            {
                return LastValue;
            }

            var count = hashedSequence.Count;
            var previousValue = LastValue;

            double result = LastValue = InitialValue;

            while (count --> 0)
            {
                var hashedExpression = hashedSequence.CircularMoveNext();

                result = hashedExpression.Value.Calculate(this);

                LastValue = result;
            }

            if (result != previousValue)
            {

            }

            return result;
        }

        public void AddExpression(Expression expression)
        {
            hashedSequence.Add(expression, expression.priority);
        }

        public void RemoveExpression(Expression expression)
        {
            var hashedExpression = new HashedNode<Expression>(expression, expression.priority);

            hashedSequence.Remove(hashedExpression);
        }

        public Variable(string name)
        {
            Name = name;

            LastValue = InitialValue = 0;

            hashedSequence = new HashedCircularConcurrentQueue<Expression>(Config.MAX_EXPRESSION_COUNT);

            Type = VariableType.Sequence;
        }

        public Variable(double value)
        {
            Name = string.Empty;

            LastValue = InitialValue = value;

            Type = VariableType.Value;
        }

        public Variable(object value)
        {
            Name = string.Empty;

            bool suportedType = value is int || value is float;

            if (!suportedType)
            {
                throw new Exception($"type: \"{value.GetType().ToString()}\" is not supported.");
            }

            LastValue = (double)value;

            Type = VariableType.Value;
        }
    }
}
