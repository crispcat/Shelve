namespace Shelve.Core
{
    using System;

    [Serializable]
    public sealed class Variable : IParallelAccess, IAffector, IAffectable
    {
        public enum VariableType
        {
            Static,
            Dynamic,
        }

        public readonly string Name;

        public readonly VariableType Type;

        public double Value
        {
            get => Calculate();
            set
            {
                LastValue = value;
            }
        }

        public double LastValue { get; private set; }

        private HashedCircularConcurrentQueue<Expression> hashedSequence;

        public double Calculate()
        {
            if (Type == VariableType.Static)
            {
                return LastValue;
            }

            var count = hashedSequence.Count;
            var previousValue = LastValue;

            double result = LastValue;

            while (count --> 0)
            {
                var hashedExpression = hashedSequence.CircularInspect();

                result = hashedExpression.Value.Calculate(this);

                LastValue = result;
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

            LastValue = 0;

            hashedSequence = new HashedCircularConcurrentQueue<Expression>(Config.MAX_EXPRESSION_COUNT);

            Type = VariableType.Dynamic;
        }

        public Variable(double value)
        {
            Name = string.Empty;

            LastValue = value;

            Type = VariableType.Static;
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

            Type = VariableType.Static;
        }
    }
}
