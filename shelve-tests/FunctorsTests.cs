namespace Shelve.Tests
{
    using Shelve.Core;
    using NUnit.Framework;
    
    [TestFixture]
    public class FunctorsTests
    {
        [Test] public void MathMethods()
        {
            var sinWrapper = MathWrapper.GetFunctorFor("Sin");
            IValueHolder[] args1 = new IValueHolder[] { new ValueHolder(new Number(30)) };
            var result1 = sinWrapper.SetInnerArgs(args1).Calculate();

            Assert.IsTrue(result1.Value == System.Math.Sin(30));

            var logWrapper = MathWrapper.GetFunctorFor("Log");
            IValueHolder[] args2 = new IValueHolder[] { new ValueHolder(new Number(9)),
                                                        new ValueHolder(new Number(3))};
            var result2 = logWrapper.SetInnerArgs(args2).Calculate();

            Assert.IsTrue(result2.Value == System.Math.Log(9, 3));
        }

        [Test] public void Plus()
        {
            var opr = DefaultOperator.GetBySign("+");
            IValueHolder[] args = new IValueHolder[] { new ValueHolder(new Number(-5)),
                                                       new ValueHolder(new Number(3))};
            var result = opr.SetInnerArgs(args).Calculate();

            Assert.IsTrue(-5 + 3 == result.Value);
        }

        [Test]
        public void Minus()
        {
            var opr = DefaultOperator.GetBySign("-");
            IValueHolder[] args = new IValueHolder[] { new ValueHolder(new Number(5)),
                                                       new ValueHolder(new Number(3))};
            var result = opr.SetInnerArgs(args).Calculate();

            Assert.IsTrue(5 - 3 == result.Value);
        }

        [Test]
        public void UnarMinus()
        {
            var opr = DefaultOperator.GetBySign("-", isUnar: true);
            IValueHolder[] args = new IValueHolder[] { new ValueHolder(new Number(-5)) };
            var result = opr.SetInnerArgs(args).Calculate();

            Assert.IsTrue(5 == result.Value);
        }

        [Test]
        public void Mul()
        {
            var opr = DefaultOperator.GetBySign("*");
            IValueHolder[] args = new IValueHolder[] { new ValueHolder(new Number(5)),
                                                       new ValueHolder(new Number(3))};
            var result = opr.SetInnerArgs(args).Calculate();

            Assert.IsTrue(5 * 3 == result.Value);
        }

        [Test]
        public void Div()
        {
            var opr = DefaultOperator.GetBySign("/");
            IValueHolder[] args = new IValueHolder[] { new ValueHolder(new Number(5)),
                                                       new ValueHolder(new Number(3))};
            var result = opr.SetInnerArgs(args).Calculate();

            Assert.IsTrue((double)5 / 3 == result.Value);
        }

        [Test]
        public void Rdiv()
        {
            var opr = DefaultOperator.GetBySign("%");
            IValueHolder[] args = new IValueHolder[] { new ValueHolder(new Number(5)),
                                                       new ValueHolder(new Number(3))};
            var result = opr.SetInnerArgs(args).Calculate();

            Assert.IsTrue(5 / 3 == result.Value);
        }

        [Test]
        public void Pow()
        {
            var opr = DefaultOperator.GetBySign("^");
            IValueHolder[] args = new IValueHolder[] { new ValueHolder(new Number(5)),
                                                       new ValueHolder(new Number(3))};
            var result = opr.SetInnerArgs(args).Calculate();

            Assert.IsTrue(5 * 5 * 5 == result.Value);
        }
    }
}
