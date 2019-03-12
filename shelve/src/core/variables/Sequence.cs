namespace Shelve.Core
{
    using System;

    [Serializable]
    public abstract class Sequence : ValueHolder
    {
        protected HashedCircularConcurrentQueue<Expression> hashedSequence;

        public readonly string Name;

        public override Number Value
        {
            get => Calculate();
            set
            {
                InitialValue = new Number(value);

                Reset();
                AffectAllDependencies();
            }
        }

        public abstract void Reset();

        public Number LastValue { get; protected set; }

        public Number InitialValue { get; protected set; }

        public Sequence(string name, Number value) : base(value)
        {
            Name = name;
            InitialValue = LastValue = value;
            hashedSequence = new HashedCircularConcurrentQueue<Expression>(2);
        }

        public virtual Number Calculate()
        {
            var count = hashedSequence.Count;

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
    }
}
