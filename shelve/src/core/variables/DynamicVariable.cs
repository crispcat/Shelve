namespace Shelve.Core
{
    using System;

    [Serializable]
    public class DynamicVariable : IValueHolder, IAffector, IAffectable
    {
        #region Metadata
        private readonly int id;
        public readonly string Name;
        private HashedCircularConcurrentQueue<Expression> hashedSequence;
        #endregion

        #region Value
        public Number Value
        {
            get => Calculate();
            set
            {
                LastValue = value;
            }
        }

        public Number LastValue { get; private set; }
        #endregion

        #region Construct
        /// <summary>
        /// Creates a dynamic variable.
        /// </summary>
        public DynamicVariable(string name, int expressionPriorityLevels)
        {
            Name = name;
            LastValue = new Number(0);
            hashedSequence = new HashedCircularConcurrentQueue<Expression>(expressionPriorityLevels);
        }
        #endregion

        #region Interface
        public Number Calculate()
        {
            var count = hashedSequence.Count;
            var previousValue = LastValue;

            while (count --> 0)
            {
                LastValue = hashedSequence.CircularInspect().Value.Calculate();
            }

            return LastValue;
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

        public override int GetHashCode()
        {
            return id;
        }
        #endregion
    }
}
