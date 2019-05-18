namespace Shelve.Core
{
    using System.Collections.Generic;

    internal struct LexedExpression
    {
        public string TargetSet;
        public readonly string Initial;
        public string ExpressionString;
        public Queue<Lexema> LexicalQueue;

        public LexedExpression(string expression)
        {
            TargetSet = string.Empty;
            Initial = ExpressionString = expression;
            LexicalQueue = new Queue<Lexema>();
        }

        public string GetTargetVariableName() => LexicalQueue.Peek().Represents;
    }
}
