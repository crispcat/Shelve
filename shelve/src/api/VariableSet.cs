namespace Shelve
{
    using System;
    using Shelve.Core;
    using Shelve.Runtime;
    using System.Collections.Generic;

    public class VariableSet
    {
        public readonly string Name;

        internal Dictionary<string, Iterator> iterators;
        internal Dictionary<string, Variable> variables;
        internal Dictionary<string, ValueHolder> staticValues;

        internal Dictionary<string, List<string>> declares;

        static public VariableSet Get(string setName) => DataManager.GetDataBySetName(setName);

        internal VariableSet()
        {
            iterators = new Dictionary<string, Iterator>();
            variables = new Dictionary<string, Variable>();
            staticValues = new Dictionary<string, ValueHolder>();

            declares = new Dictionary<string, List<string>>();
        }

        public Variable Variable(string name) =>
            GetDeclaredElement(name, variables);

        public Iterator Iterator(string name) =>
            GetDeclaredElement(name, iterators);

        public ValueHolder StaticValue(string name) => 
            GetDeclaredElement(name, staticValues);

        public List<Iterator> GetIteratorsInGroup(string declaredGroup) => 
            GroupByDeclares(declaredGroup, iterators);

        public List<ValueHolder> GetStaticValuesInGroup(string declaredGroup) =>
            GroupByDeclares(declaredGroup, staticValues);


        private T GetDeclaredElement<T>(string name, Dictionary<string, T> targetDictionary)
        {
            if (!targetDictionary.ContainsKey(name))
            {
                throw new ArgumentException($"Set \"{Name}\" do not contains \"{name}\" {typeof(T).Name}");
            }

            return targetDictionary[name];
        }

        private List<T> GroupByDeclares<T>(string declaredGroup, Dictionary<string, T> targetDictionary)
        {
            if (!declares.ContainsKey(declaredGroup))
            {
                throw new ArgumentException($"Set \"{Name}\" do not declares \"{declaredGroup}\" group");
            }

            var groupedItems = new List<T>();

            foreach (var name in declares[declaredGroup])
            {
                if (targetDictionary.ContainsKey(name))
                {
                    groupedItems.Add(targetDictionary[name]);
                }
            }

            return groupedItems;
        }
    }
}
