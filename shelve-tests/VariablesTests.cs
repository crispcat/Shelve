namespace Shelve.Tests
{
    using global::Shelve.Core;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class VariablesTests
    {
        [Test] public void CalculationsOverride()
        {
            var dics = new Dictionary<string, IValueHolder>();

            var variable1 = new Variable("var1", new Number(33.3));
            var variable2 = new Iterator("var2", new Number(-28.8));

            dics.Add(variable1.Name, variable1);
            dics.Add(variable2.Name, variable2);

            var var1 = dics[variable1.Name];
            var var2 = dics[variable2.Name];

            var val1 = var1.Value;
            var val2 = var2.Value;

            Assert.IsFalse(var1.GetType() == var2.GetType());
            Assert.IsFalse(ReferenceEquals(var1.GetType().GetProperty("Value"), var2.GetType().GetProperty("Value")));
            Assert.IsFalse(ReferenceEquals(var1.GetType().GetMethod("Calculate"), var2.GetType().GetMethod("Calculate")));
        }
    }
}
