namespace Shelve
{
    using System;
    using Shelve.Core;
    using Shelve.Runtime;
    using System.Collections.Generic;

    public class VariableSet
    {
        public readonly string Name;
        
        internal HashedVariables members;
        internal Dictionary<string, List<string>> declares;
        
        static public VariableSet GetInstance(string setName) => DataManager.GetDataBySetName(setName);

        internal VariableSet(string name)
        {
            if (!Lexica.variableRegex.IsMatch(name))
            {
                throw new ArgumentException($"Name format exception for set {name}");
            }

            Name = name;
            declares = new Dictionary<string, List<string>>();
            members = null;
        }

        // For Unity
        public float this[string key]
        {
            get => (float) members[key].Value;
            set => members[key].Value = value;
        }

        public T Get<T>(string name) where T : Sequence => members[name] as T;

        public IValueHolder __(string key) => GetDeclaredElement(key);

        public IEnumerable<IValueHolder> _(string declaredGroup) => GroupByDeclares(declaredGroup);

        private IValueHolder GetDeclaredElement(string name)
        {
            if (!members.Contains(name))
            {
                throw new ArgumentException($"Set \"{Name}\" has no definition for \"{name}\".");
            }

            return members[name];
        }

        private IEnumerable<IValueHolder> GroupByDeclares(string declaredGroup)
        {
            if (!declares.ContainsKey(declaredGroup))
            {
                throw new ArgumentException($"Set \"{Name}\" has no declaration for \"{declaredGroup}\" group.");
            }

            var elements = new List<IValueHolder>();

            foreach (var name in declares[declaredGroup])
            {
                elements.Add(members[name]);
            }

            return elements;
        }

        public VariableSet Merge(VariableSet another)
        {
            members.Merge(another.members);
            return this;
        }
    }

    public static class GroupExtenssion
    {
        public static void MoveAllIterators(this IEnumerable<IValueHolder> group)
        {
            foreach (var element in group)
            {
                if (element is Iterator iterator)
                {
                    iterator.MoveNextValue();
                }
            }
        }
    }
}
