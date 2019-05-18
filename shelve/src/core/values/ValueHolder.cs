namespace Shelve.Core
{
    public class ValueHolder : IValueHolder
    {
        private Number _value;

        public virtual Number Value
        {
            get => _value;
            set
            {
                _value = new Number(value);
            }
        }

        public ValueHolder(Number value)
        {
            _value = value;
        }
    }
}
