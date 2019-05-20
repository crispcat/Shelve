namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class DynamicValueHolder : IAffector, IValueHolder
    {
        public string Name { get; protected set; }

        private Number _value;

        public virtual Number Value
        {
            get => _value;
            set
            {
                _value = new Number(value);

                AffectAllDependencies();
            }
        }

        protected HashSet<IAffectable> affectsOn;

        public DynamicValueHolder(Number value)
        {
            Name = string.Empty;

            _value = value;

            affectsOn = new HashSet<IAffectable>();
        }

        public DynamicValueHolder(string name, Number value)
        {
            Name = name;

            _value = value;

            affectsOn = new HashSet<IAffectable>();
        }

        public void AffectAllDependencies()
        {
            var deprecated = new HashSet<IAffectable>();

            foreach (var dependency in affectsOn)
            {
                if (dependency == null)
                {
                    deprecated.Add(dependency);
                }
                else
                {
                    dependency.Affect();
                }
            }

            affectsOn.ExceptWith(deprecated);
        }

        public void AddAffectOn(IAffectable dependency)
        {
            affectsOn.Add(dependency);
        }
    }
}
