namespace Shelve.Core
{
    using System;

    [Serializable]
    public abstract class Sequence : DynamicValueHolder
    {
        internal HashedCircularConcurrentQueue<Expression> hashedSequence;

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

        protected Sequence(string name, Number value) : base(value)
        {
            Name = name;
            InitialValue = LastValue = value;
            hashedSequence = new HashedCircularConcurrentQueue<Expression>(6);
        }

        public virtual Number Calculate()
        {
            var count = hashedSequence.Count;

            while (count --> 0)
            {
                var expression = hashedSequence.CircularInspect().Value;

                if (expression.members.Count != 0)
                {
                    try /* hotfix */
                    {
                        LastValue = expression.Calculate();
                    }
                    catch
                    {
                        Reset();

                        throw new Exception();
                    }
                }
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
